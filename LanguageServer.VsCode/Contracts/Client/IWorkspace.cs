using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;

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
    }
}
