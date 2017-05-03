using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using LanguageServer.VsCode.Contracts;

namespace LanguageServer.VsCode.Server
{

    public abstract class TextDocument
    {
        public static T Load<T>(TextDocumentItem doc) where T : TextDocument, new()
        {
            var inst = new T();
            inst.Load(doc);
            return inst;
        }

        private readonly List<TextDocumentCollection> owners = new List<TextDocumentCollection>();

        private Uri _Uri;

        public virtual Uri Uri
        {
            get { return _Uri; }
            private set
            {
                foreach (var o in owners) o.NotifyUriChanging(this, value);
                _Uri = value;
            }
        }

        public virtual string LanguageId { get; private set; }

        /// <summary>
        /// Revision number of the document.
        /// </summary>
        public virtual int Version { get; private set; }

        /// <summary>
        /// The full content of the document.
        /// </summary>
        public abstract string Content { get; set; }

        /// <summary>
        /// Total lines count of the document.
        /// </summary>
        public abstract int LinesCount { get; }

        /// <summary>
        /// Fills this instance with the given <see cref="TextDocumentItem"/>.
        /// </summary>
        public virtual void Load(TextDocumentItem doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            Content = doc.Text;
            Uri = doc.Uri;
            LanguageId = doc.LanguageId;
            Version = doc.Version;
        }

        internal void AddOwner(TextDocumentCollection owner)
        {
            Debug.Assert(owner != null);
            owners.Add(owner);
        }

        internal void RemoveOwner(TextDocumentCollection owner)
        {
            Debug.Assert(owner != null);
            owners.Remove(owner);
        }

        /// <summary>
        /// Gets a range of the content.
        /// </summary>
        public abstract string GetRange(int offset, int length);

        public abstract Position PositionAt(int offset);

        public abstract int OffsetAt(Position position);
    }

    /// <summary>
    /// Represents a plain-text document.
    /// </summary>
    public class FullTextDocument: TextDocument
    {

        private static readonly IList<int> EmptyLineStarts = new[] {0};

        private string _Content = "";

        private IList<int> _LineStarts = EmptyLineStarts;

        /// <inheritdoc />
        public override string Content
        {
            get { return _Content; }
            set
            {
                if (_Content == value) return;
                if (string.IsNullOrEmpty(value))
                {
                    _Content = "";
                    _LineStarts = EmptyLineStarts;
                    return;
                }
                var starts = new List<int> {0};
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == '\r')
                    {
                        if (i + 1 < value.Length && value[i + 1] == '\n')
                        {
                            // \r\n
                            starts.Add(i + 2);
                            i++;
                            continue;
                        }
                        // \r
                        starts.Add(i + 1);
                    } else if (value[i] == '\n')
                    {
                        // \n
                        starts.Add(i + 1);
                    }
                }
                _Content = value;
                _LineStarts = starts;
            }
        }

        /// <inheritdoc />
        public override string GetRange(int offset, int length)
        {
            return _Content.Substring(offset, length);
        }

        /// <inheritdoc />
        public override int LinesCount => _LineStarts.Count;

        public override Position PositionAt(int offset)
        {
            // TODO some optimizations…
            int line;
            for (int i = 1; i < _LineStarts.Count; i++)
            {
                if (offset < _LineStarts[i])
                {
                    line = i - 1;
                    goto RET;
                }
            }
            line = _LineStarts.Count - 1;
            RET:
            return new Position(line, offset - _LineStarts[line]);
        }

        public override int OffsetAt(Position position)
        {
            if (position.Line < 0 || position.Line >= _LineStarts.Count)
                throw new ArgumentOutOfRangeException(nameof(position));
            return _LineStarts[position.Line] + position.Character;
        }

    }
}
