using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonRpc.Standard;
using JsonRpc.Standard.Server;

namespace LanguageServer.VsCode
{
    /// <summary>
    /// Provides extension methods for implementing a Language Server.
    /// </summary>
    public static class LanguageServerExtensions
    {
        /// <summary>
        /// Interprets any <see cref="OperationCanceledException" /> thrown by the service
        /// as <c>RequestCancelled</c> error per definition in Language Server Protocol.
        /// </summary>
        public static void UseCancellationHandling(this JsonRpcServiceHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            builder.Intercept(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (OperationCanceledException ex)
                {
                    if (ex.CancellationToken == context.CancellationToken
                        || ex.CancellationToken == CancellationToken.None)
                    {
                        context.Response.Error = new ResponseError(Utility.RequestCancelledErrorCode, ex.Message);
                    }
                }
            });
        }

        /// <summary>
        /// Determines whether the specified document URI indeicates an "untitled" document.
        /// </summary>
        /// <remarks>
        /// The URI of an untitled document has the following structure: <c>untitled:xxxxx</c>.
        /// </remarks>
        public static bool IsUntitled(this Uri documentUri)
        {
            return documentUri.Scheme == "untitled";
        }
    }
}
