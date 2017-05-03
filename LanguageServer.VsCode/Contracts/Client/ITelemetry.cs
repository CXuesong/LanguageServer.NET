using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsonRpc.Standard.Contracts;

namespace LanguageServer.VsCode.Contracts.Client
{
    /// <summary>
    /// The telemetry notification is sent from the server to the client to ask the client to log a telemetry event.
    /// </summary>
    [JsonRpcScope(MethodPrefix = "telemetry/")]
    public interface ITelemetry
    {
        // TODO implement some real ANY params.
        /// <summary>
        /// The telemetry notification is sent from the server to the client to ask
        /// the client to log a telemetry event.
        /// </summary>
        /// <param name="data">Any number of data to be sent.</param>
        [JsonRpcMethod(IsNotification = true)]
        Task Event(params object[] data);
    }
}
