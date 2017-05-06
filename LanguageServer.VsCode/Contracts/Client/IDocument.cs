using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;

namespace LanguageServer.VsCode.Contracts.Client
{

    [JsonRpcScope(MethodPrefix = "textDocument/")]
    public interface IDocument
    {
        /// <summary>
        /// Diagnostics notification are sent from the server to the client to signal results of validation runs.
        /// </summary>
        /// <param name="uri">The URI for which diagnostic information is reported.</param>
        /// <param name="diagnostics">An array of diagnostic information items.
        /// To clear the existing diagnostics, pass <see cref="Diagnostic.EmptyDiagnostics"/> instead of <c>null</c>.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task PublishDiagnostics(Uri uri, IEnumerable<Diagnostic> diagnostics);
    }

}
