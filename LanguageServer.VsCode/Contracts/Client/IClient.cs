using System;
using System.Collections.Generic;
using System.Text;
using JsonRpc.Standard.Contracts;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts.Client
{
    [JsonRpcScope(MethodPrefix = "client/")]
    public interface IClient
    {
        /// <summary>
        /// Registers for a new capability on the client side.
        /// Not all clients need to support dynamic capability registration.
        /// A client opts in via the ClientCapabilities.dynamicRegistration property.
        /// </summary>
        [JsonRpcMethod]
        void RegisterCapability(IEnumerable<Registration> registrations);

        /// <summary>
        /// Unregisters a previously registered capability.
        /// </summary>
        [JsonRpcMethod]
        void UnregisterCapability(IEnumerable<Unregistration> unregisterations);
    }
}
