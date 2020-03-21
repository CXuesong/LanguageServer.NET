using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// A textual edit operation applicable to a text document.
    /// </summary>
    [JsonObject]
    public class TextEdit
    {

        public TextEdit(Range range, string newText)
        {
            Range = range;
            NewText = newText;
        }

        /// <summary>
        /// The range of the text document to be manipulated. To insert
        /// text into a document create a range where start === end.
        /// </summary>
        public Range Range { get; set; }

        /// <summary>
        /// The string to be inserted.For delete operations use an empty string.
        /// </summary>
        public string NewText { get; set; }

    }

    /// <summary>
    /// Describes textual changes on a single text document.
    /// The text document is referred to as a <see cref="VersionedTextDocumentIdentifier"/> to allow clients
    /// to check the text document version before an edit is applied.
    /// </summary>
    [JsonObject]
    public class TextDocumentEdit
    {

        public TextDocumentEdit(VersionedTextDocumentIdentifier textDocument, ICollection<TextEdit> edits)
        {
            TextDocument = textDocument;
            Edits = edits;
        }

        /// <summary>
        /// The text document to change.
        /// </summary>
        public VersionedTextDocumentIdentifier TextDocument { get; set; }

        /// <summary>
        /// The edits to be applied.
        /// </summary>
        public ICollection<TextEdit> Edits { get; set; }
    }

    /// <summary>
    /// A workspace edit represents changes to many resources managed in the workspace.
    /// The edit should either provide changes or documentChanges.
    /// If <see cref="DocumentChanges"/> are present they are preferred over <see cref="Changes"/>
    /// if the client can handle versioned document edits.
    /// </summary>
    [JsonObject]
    public class WorkspaceEdit
    {

        public WorkspaceEdit(IDictionary<Uri, ICollection<TextEdit>> changes) : this(null, changes)
        {
        }

        public WorkspaceEdit(ICollection<TextDocumentEdit> documentChanges) : this(documentChanges, null)
        {
        }

        [JsonConstructor]
        public WorkspaceEdit(ICollection<TextDocumentEdit> documentChanges, IDictionary<Uri, ICollection<TextEdit>> changes)
        {
            DocumentChanges = documentChanges;
            Changes = changes;
        }

        /// <summary>
        /// Holds changes to existing resources.
        /// </summary>
        public IDictionary<Uri, ICollection<TextEdit>> Changes { get; set; }

        /// <summary>
        /// An array of `TextDocumentEdit`s to express changes to specific a specific
        /// version of a text document. Whether a client supports versioned document
        /// edits is expressed via `WorkspaceClientCapabilities.versionedWorkspaceEdit`.
        /// </summary>
        public ICollection<TextDocumentEdit> DocumentChanges { get; set; }
    }

}
