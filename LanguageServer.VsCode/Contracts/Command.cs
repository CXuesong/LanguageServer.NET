using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Use <see cref="EditorCommand"/> instead of this class.
    /// </summary>
    [Obsolete("Use EditorCommand instead of this class.")]
    public static class Command
    {
        
    }

    // Sadlys, we cannot have a Command property in Command class.
    /// <summary>
    /// Represents a reference to a VS Code command. (<c>Command</c> in language protocol.)
    /// </summary>
    public class EditorCommand
    {
        /// <summary>
        /// Arguments that the command handler should be invoked with.
        /// </summary>
        public IList<object> Arguments { get; set; }

        /// <summary>
        /// The identifier of the actual command handler.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Title of the command, like `save`.
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// Execute command registration options.
    /// </summary>
    public class ExecuteCommandRegistrationOptions : RegistrationOptions
    {
        [JsonConstructor]
        public ExecuteCommandRegistrationOptions() : this(null)
        {
        }

        public ExecuteCommandRegistrationOptions(IEnumerable<string> commands)
        {
            Commands = commands;
        }

        /// <summary>
        /// The commands to be executed on the server.
        /// </summary>
        [JsonProperty]
        public IEnumerable<string> Commands { get; set; }
    }
}
