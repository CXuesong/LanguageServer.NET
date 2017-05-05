using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// General paramters to register for a capability.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Registration
    {

        private static int counter = 0;

        private static string NextId()
        {
            var id = Interlocked.Increment(ref counter);
            return "registration" + id;
        }

        /// <summary>
        /// Initialize an empty registration entry.
        /// </summary>
        [JsonConstructor]
        public Registration()
        {
            
        }

        /// <summary>
        /// Initialize with automatic generated <see cref="Id"/> and other given parameters.
        /// </summary>
        public Registration(string method, RegistrationOptions options) : this(NextId(), method, options)
        {
        }

        /// <summary>
        /// Initialize with given parameters.
        /// </summary>
        public Registration(string id, string method, RegistrationOptions registerOptions)
        {
            Id = id;
            Method = method;
            RegisterOptions = registerOptions;
        }

        /// <summary>
        /// The id used to register the request. The id can be used to deregister
        /// the request again.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The method / capability to register for.
        /// </summary>
        [JsonProperty]
        public string Method { get; set; }

        /// <summary>
        /// Options necessary for the registration.
        /// </summary>
        [JsonProperty]
        public RegistrationOptions RegisterOptions { get; set; }
    }

    /// <summary>
    /// Registration options for <see cref="Registration"/> in <c>client/registerCapability</c>.
    /// You may derive your own options class from here, if necessary.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RegistrationOptions
    {

    }

    /// <summary>
    /// Since most of the registration options require to specify a document selector,
    /// there is a base interface that can be used.
    /// </summary>
    public class TextDocumentRegistrationOptions : RegistrationOptions
    {

        [JsonConstructor]
        public TextDocumentRegistrationOptions()
        {
            
        }

        public TextDocumentRegistrationOptions(IEnumerable<DocumentFilter> documentSelector)
        {
            DocumentSelector = documentSelector;
        }

        /// <summary>
        /// A document selector to identify the scope of the registration. If set to null
        ///	the document selector provided on the client side will be used.
        /// </summary>
        [JsonProperty]
        public IEnumerable<DocumentFilter> DocumentSelector { get; set; }
    }

    /// <summary>
    /// General parameters to unregister a capability.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Unregistration
    {
        /// <summary>
        /// Initialize an empty class.
        /// </summary>
        [JsonConstructor]
        public Unregistration()
        {

        }

        /// <summary>
        /// Initialize with given parameters.
        /// </summary>
        public Unregistration(string id, string method)
        {
            Id = id;
            Method = method;
        }

        /// <summary>
        /// The id used to unregister the request or notification. Usually an id
        /// provided during the register request.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The method / capability to unregister for.
        /// </summary>
        [JsonProperty]
        public string Method { get; set; }
    }
}