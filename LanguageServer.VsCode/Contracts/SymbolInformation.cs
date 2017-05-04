using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Represents information about programming constructs like variables, classes,
    /// interfaces etc.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SymbolInformation
    {
        [JsonConstructor]
        public SymbolInformation()
        {

        }

        public SymbolInformation(string name, SymbolKind kind, Location location, string containerName)
        {
            Name = name;
            Kind = kind;
            Location = location;
            ContainerName = containerName;
        }

        /// <summary>
        /// The name of this symbol.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The kind of this symbol.
        /// </summary>
        [JsonProperty]
        public SymbolKind Kind { get; set; }

        /// <summary>
        /// The location of this symbol.
        /// </summary>
        [JsonProperty]
        public Location Location { get; set; }

        /// <summary>
        /// The name of the symbol containing this symbol.
        /// </summary>
        [JsonProperty]
        public string ContainerName { get; set; }
    }

    /// <summary>
    /// A symbol kind.
    /// </summary>
    public enum SymbolKind
    {
        File = 1,
        Module = 2,
        Namespace = 3,
        Package = 4,
        Class = 5,
        Method = 6,
        Property = 7,
        Field = 8,
        Constructor = 9,
        Enum = 10,
        Interface = 11,
        Function = 12,
        Variable = 13,
        Constant = 14,
        String = 15,
        Number = 16,
        Boolean = 17,
        Array = 18,
    }
}
