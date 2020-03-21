using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Contracts;
using Newtonsoft.Json.Linq;

namespace LanguageServer.VsCode.Contracts.Client
{
    [JsonRpcScope(MethodPrefix = "workspace/")]
    public interface IWorkspace
    {
        /// <summary>
        /// Modifies resource on the client side.
        /// </summary>
        /// <param name="edit">The edits to apply.</param>
        /// <returns>Indicates whether the edit was applied or not.</returns>
        [JsonRpcMethod]
        Task<bool> ApplyEdit(WorkspaceEdit edit);

        /// <summary>
        /// Fetch configuration settings from the client. The request can fetch several configuration settings in one roundtrip. (LSP 3.6)
        /// </summary>
        /// <param name="items"></param>
        /// <returns>
        /// The order of the returned configuration settings correspond to the order of the passed <see cref="ConfigurationItem"/> (e.g. the first item in the response is the result for the first configuration item in the params).
        /// If a scope URI is provided the client should return the setting scoped to the provided resource.
        /// If the client for example uses EditorConfig to manage its settings the configuration should be returned for the passed resource URI.
        /// If the client can’t provide a configuration setting for a given scope then null need to be present in the returned array.
        /// </returns>
        /// <remarks>
        /// The configuration section ask for is defined by the server and doesn’t necessarily need to correspond to the configuration store used be the client.
        /// So a server might ask for a configuration <c>cpp.formatterOptions</c> but the client stores the configuration in a XML store layout differently.
        /// It is up to the client to do the necessary conversion.
        /// </remarks>
        [JsonRpcMethod]
        Task<JArray> Configuration(IEnumerable<ConfigurationItem> items);
    }
}
