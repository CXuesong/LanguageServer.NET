using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using LanguageServer.VsCode.Contracts;

namespace LanguageServer.VsCode.Server
{
    /// <summary>
    /// Represents a collection of <see cref="TextDocument"/>s that can be indexed by their URI.
    /// </summary>
    public class TextDocumentCollection : KeyedCollection<Uri, TextDocument>
    {
        /// <summary>
        /// Gets the element with the specified key.
        /// </summary>
        public TextDocument this[TextDocumentIdentifier key] => this[key.Uri];

        /// <summary>
        /// Removes the element with the specified key.
        /// </summary>
        public bool Remove(TextDocumentIdentifier key)
            => this.Remove(key.Uri);

        /// <inheritdoc />
        protected override void InsertItem(int index, TextDocument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            base.InsertItem(index, item);
            item.AddOwner(this);
        }

        /// <inheritdoc />
        protected override void RemoveItem(int index)
        {
            Items[index].RemoveOwner(this);
            base.RemoveItem(index);
        }

        /// <inheritdoc />
        protected override void SetItem(int index, TextDocument item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Items[index].RemoveOwner(this);
            base.SetItem(index, item);
            item.AddOwner(this);
        }

        internal void NotifyUriChanging(TextDocument document, Uri newUri)
        {
            this.ChangeItemKey(document, newUri);
        }

        /// <inheritdoc />
        protected override Uri GetKeyForItem(TextDocument item)
        {
            return item.Uri;
        }
    }
}
