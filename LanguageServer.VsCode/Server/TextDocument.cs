using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LanguageServer.VsCode.Contracts;

namespace LanguageServer.VsCode.Server
{
    /// <summary>
    /// Represents the basic traits of an immutable plain-text document.
    /// </summary>
    public abstract class TextDocument
    {
        /// <summary>
        /// Creates a new instance derived from <see cref="TextDocument"/>, and
        /// fills it with the specified <see cref="TextDocumentItem"/>.
        /// </summary>
        /// <typeparam name="T">A derived class of <see cref="TextDocument"/>.</typeparam>
        /// <param name="doc">The document source to load information from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="doc"/> is <c>null</c>.</exception>
        public static T Load<T>(TextDocumentItem doc) where T : TextDocument, new()
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            var inst = new T();
            inst.OnLoad(doc);
            return inst;
        }

        /// <summary>
        /// URI of the document.
        /// </summary>
        /// <remarks>
        /// You may use <see cref="LanguageServerExtensions.IsUntitled"/> to determine whether
        /// this document is untitled.
        /// </remarks>
        public virtual Uri Uri { get; protected set; }

        /// <summary>
        /// Document's language identifier.
        /// </summary>
        public string LanguageId { get; protected set; }

        /// <summary>
        /// Revision number of the document.
        /// </summary>
        public int Version { get; protected set; }

        /// <summary>
        /// The full content of the document.
        /// </summary>
        public string Content { get; protected set; }

        /// <summary>
        /// Total lines count of the document.
        /// </summary>
        public abstract int LinesCount { get; }

        /// <summary>
        /// Fills this instance with the given <see cref="TextDocumentItem"/>.
        /// </summary>
        protected virtual void OnLoad(TextDocumentItem doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            Content = doc.Text ?? "";
            Uri = doc.Uri;
            LanguageId = doc.LanguageId;
            Version = doc.Version;
        }

        /// <summary>
        /// Applies a series of <see cref="TextDocumentContentChangeEvent"/>s to the current document.
        /// </summary>
        public abstract TextDocument ApplyChanges(IList<TextDocumentContentChangeEvent> changes);

        /// <summary>
        /// Gets a part of the content by the specified line/column-based range.
        /// </summary>
        public virtual string GetRange(Range range)
        {
            var start = OffsetAt(range.Start);
            return GetRange(start, OffsetAt(range.End) - start);
        }

        /// <summary>
        /// Gets a part of the content by the specified offset range.
        /// </summary>
        public abstract string GetRange(int offset, int length);

        /// <summary>
        /// Converts the specified 0-based document offset into <see cref="Position"/>.
        /// </summary>
        public abstract Position PositionAt(int offset);

        /// <summary>
        /// Converts the specified <see cref="Position"/> into 0-based document offset.
        /// </summary>
        public abstract int OffsetAt(Position position);
    }

    /// <summary>
    /// A plain-text document object that always store the full content in it.
    /// </summary>
    public class FullTextDocument: TextDocument
    {

        private static readonly IList<int> emptyLineStarts = new[] {0};

        private IList<int> _LineStarts = emptyLineStarts;

        /// <inheritdoc />
        public override string GetRange(int offset, int length)
        {
            return Content.Substring(offset, length);
        }

        /// <inheritdoc />
        public override int LinesCount => _LineStarts.Count;

        /// <inheritdoc />
        protected override void OnLoad(TextDocumentItem doc)
        {
            base.OnLoad(doc);
            if (string.IsNullOrEmpty(Content))
            {
                _LineStarts = emptyLineStarts;
                return;
            }
            _LineStarts = GetLineStarts(Content).ToArray();
        }

        /// <summary>
        /// Gets all the offsets that are the starts of new lines.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is <c>null</c>.</exception>
        public static IEnumerable<int> GetLineStarts(string expr)
        {
            if (expr == null) throw new ArgumentNullException(nameof(expr));
            yield return 0;
            for (int i = 0; i < expr.Length; i++)
            {
                if (expr[i] == '\r')
                {
                    if (i + 1 < expr.Length && expr[i + 1] == '\n')
                    {
                        // \r\n
                        yield return i + 2;
                        i++;
                        continue;
                    }
                    // \r
                    yield return i + 1;
                }
                else if (expr[i] == '\n')
                {
                    // \n
                    yield return i + 1;
                }
            }
        }

        /// <inheritdoc />
        public override TextDocument ApplyChanges(IList<TextDocumentContentChangeEvent> changes)
        {
            if (changes == null || changes.Count == 0) return this;
            var newInst = (FullTextDocument) MemberwiseClone();
            var newContent = Content;
            List<int> newLineStarts = null;
            int firstEffectiveChange = 0;
            for (int i = changes.Count - 1; i >= 0; i--)
            {
                if (!changes[i].HasRange)
                {
                    firstEffectiveChange = i;
                    break;
                }
            }
            for (int i = firstEffectiveChange; i < changes.Count; i++)
            {
                var change = changes[i];
                if (change.HasRange)
                {
                    // In case we need to evaluate offset
                    if (newLineStarts == null)
                    {
                        Debug.Assert(newContent == Content);
                        newLineStarts = _LineStarts.ToList();
                    }
                    var startPos = change.Range.Start;
                    var endPos = change.Range.End;
                    // Update the content first.
                    var startOffset = newLineStarts[change.Range.Start.Line] + change.Range.Start.Character;
                    newContent = newContent.Substring(0, startOffset) + change.Text +
                                 newContent.Substring(startOffset + change.RangeLength);
                    // Some lines have been removed.
                    if (endPos.Line > startPos.Line)
                        newLineStarts.RemoveRange(startPos.Line + 1, endPos.Line - startPos.Line);
                    // Offset the bottom part of the document.
                    var deltaLength = (change.Text?.Length ?? 0) - change.RangeLength;
                    for (int j = startPos.Line + 1; j < newLineStarts.Count; j++)
                        newLineStarts[j] += deltaLength;
                    // Some lines have been added.
                    if (!string.IsNullOrEmpty(change.Text))
                    {
                        var nls = GetLineStarts(change.Text).Skip(1).Select(o => o + startOffset).ToList();
                        if (nls.Count > 0) newLineStarts.InsertRange(startPos.Line + 1, nls);
                    }
                    foreach (var start in newLineStarts)
                    {
                        Debug.Assert(start == 0 || newContent[start - 1] == '\n' || newContent[start - 1] == '\r' && newContent[start] != '\n');
                    }
                }
                else
                {
                    newContent = change.Text ?? "";
                    newLineStarts = GetLineStarts(newContent).ToList();
                }
            }
            newInst.Content = newContent;
            newInst._LineStarts = newLineStarts;
            return newInst;
        }

        private static Position PositionAt(IList<int> lineStarts, int offset)
        {
            Debug.Assert(lineStarts != null);
            // TODO some optimizations…
            int line;
            for (int i = 1; i < lineStarts.Count; i++)
            {
                if (offset < lineStarts[i])
                {
                    line = i - 1;
                    goto RET;
                }
            }
            line = lineStarts.Count - 1;
            RET:
            return new Position(line, offset - lineStarts[line]);
        }

        /// <inheritdoc />
        public override Position PositionAt(int offset)
        {
            return PositionAt(_LineStarts, offset);
        }

        /// <inheritdoc />
        public override int OffsetAt(Position position)
        {
            if (position.Line < 0 || position.Line >= _LineStarts.Count)
                throw new ArgumentOutOfRangeException(nameof(position));
            return _LineStarts[position.Line] + position.Character;
        }

    }
}
