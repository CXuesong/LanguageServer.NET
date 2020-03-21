using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Contracts;

namespace LanguageServer.VsCode.Contracts.Client
{
    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public interface ITextDocument
    {
        /// <summary>
        /// Diagnostics notification are sent from the server to the client to signal results of validation runs.
        /// </summary>
        /// <param name="uri">The URI for which diagnostic information is reported.</param>
        /// <param name="diagnostics">An array of diagnostic information items.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task PublishDiagnostics(Uri uri, IEnumerable<Diagnostic> diagnostics);
    }
}
