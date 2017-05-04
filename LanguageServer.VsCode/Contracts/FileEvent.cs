using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// An event describing a file change.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FileEvent
    {
        /// <summary>
        /// The file's URI.
        /// </summary>
        [JsonProperty]
        public Uri Uri { get; set; }

        /// <summary>
        /// The change type.
        /// </summary>
        [JsonProperty]
        public FileChangeType Type { get; set; }
    }

    /// <summary>
    /// The file event type.
    /// </summary>
    public enum FileChangeType
    {
        /// <summary>
        /// The file got created.
        /// </summary>
        Created = 1,
        /// <summary>
        /// The file got changed.
        /// </summary>
        Changed = 2,
        /// <summary>
        /// The file got deleted.
        /// </summary>
        Deleted = 3,
    }
}
