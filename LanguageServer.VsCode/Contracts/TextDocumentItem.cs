using System;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// An item to transfer a text document from the client to the server.
    /// </summary>
    public class TextDocumentItem
    {
        /// <summary>
        /// The text document's language identifier.
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// The content of the opened text document.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The text document's URI.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// The version number of this document (it will strictly increase after each change, including undo/redo).
        /// </summary>
        public int Version { get; set; }
    }

    /// <summary>
    /// Text documents are identified using a URI. 
    /// </summary>
    public struct TextDocumentIdentifier : IEquatable<TextDocumentIdentifier>
    {
        public TextDocumentIdentifier(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }

        /// <inheritdoc />
        public override string ToString() => Uri?.ToString();

        /// <inheritdoc />
        public bool Equals(TextDocumentIdentifier other)
        {
            return Uri == other.Uri;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TextDocumentIdentifier && Equals((TextDocumentIdentifier) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Uri != null ? Uri.GetHashCode() : 0);
        }

        public static bool operator == (TextDocumentIdentifier x, TextDocumentIdentifier y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(TextDocumentIdentifier x, TextDocumentIdentifier y)
        {
            return !x.Equals(y);
        }
    }

    /// <summary>
    /// An identifier to denote a specific version of a text document.
    /// </summary>
    public struct VersionedTextDocumentIdentifier : IEquatable<VersionedTextDocumentIdentifier>
    {
        public VersionedTextDocumentIdentifier(Uri uri, int version)
        {
            Uri = uri;
            Version = version;
        }

        public Uri Uri { get; }

        public int Version { get; }

        public override string ToString() => $"{Uri}({Version})";

        /// <inheritdoc />
        public bool Equals(VersionedTextDocumentIdentifier other)
        {
            return Equals(Uri, other.Uri) && Version == other.Version;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VersionedTextDocumentIdentifier && Equals((VersionedTextDocumentIdentifier) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Uri != null ? Uri.GetHashCode() : 0) * 397) ^ Version;
            }
        }
    }
}
