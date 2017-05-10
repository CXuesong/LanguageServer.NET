using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    /// <summary>
    /// Signature help represents the signature of something
    /// callable.There can be multiple signature but only one
    /// active and only one active parameter.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SignatureHelp
    {

        [JsonConstructor]
        public SignatureHelp()
        {

        }

        public SignatureHelp(IList<SignatureInformation> signatures) : this(signatures, 0, 0)
        {
        }

        public SignatureHelp(IList<SignatureInformation> signatures, int activeSignature, int activeParameter)
        {
            Signatures = signatures;
            ActiveSignature = activeSignature;
            ActiveParameter = activeParameter;
        }

        /// <summary>
        /// One or more signatures.
        /// </summary>
        [JsonProperty]
        public IList<SignatureInformation> Signatures { get; set; }

        /// <summary>
        /// The active signature. If omitted or the value lies outside the
        /// range of `signatures` the value defaults to zero or is ignored if
        /// `signatures.length === 0`. Whenever possible implementors should 
        /// make an active decision about the active signature and shouldn't 
        /// rely on a default value.
        /// In future version of the protocol this property might become
        /// mandantory to better express this.
        /// </summary>
        [JsonProperty]
        public int ActiveSignature { get; set; }

        /// <summary>
        /// The active parameter of the active signature. If omitted or the value
        /// lies outside the range of `signatures[activeSignature].parameters` 
        /// defaults to 0 if the active signature has parameters. If 
        /// the active signature has no parameters it is ignored. 
        /// In future version of the protocol this property might become
        /// mandantory to better express the active parameter if the
        /// active signature does have any.
        /// </summary>
        [JsonProperty]
        public int ActiveParameter { get; set; }
    }

    /// <summary>
    /// Represents the signature of something callable.A signature
    /// can have a label, like a function-name, a doc-comment, and
    /// a set of parameters.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class SignatureInformation
    {

        [JsonConstructor]
        public SignatureInformation()
        {

        }

        public SignatureInformation(string label) : this(label, null, null)
        {
        }

        public SignatureInformation(string label, string documentation) : this(label, documentation, null)
        {
        }

        public SignatureInformation(string label, string documentation, IList<ParameterInformation> parameters)
        {
            Label = label;
            Documentation = documentation;
            Parameters = parameters;
        }

        /// <summary>
        /// The label of this signature. Will be shown in
        /// the UI.
        /// </summary>
        [JsonProperty]
        public string Label { get; set; }

        /// <summary>
        /// The human-readable doc-comment of this signature. Will be shown
        /// in the UI but can be omitted.
        /// </summary>
        [JsonProperty]
        public string Documentation { get; set; }

        /// <summary>
        /// The parameters of this signature.
        /// </summary>
        [JsonProperty]
        public IList<ParameterInformation> Parameters { get; set; }
    }

    /// <summary>
    /// Represents a parameter of a callable-signature.A parameter can
    /// have a label and a doc-comment.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ParameterInformation
    {

        [JsonConstructor]
        public ParameterInformation()
        {

        }

        public ParameterInformation(string label, string documentation)
        {
            Label = label;
            Documentation = documentation;
        }

        /// <summary>
        /// The label of this parameter. Will be shown in
        /// the UI.
        /// </summary>
        [JsonProperty]
        public string Label { get; set; }

        /// <summary>
        /// The human-readable doc-comment of this parameter. Will be shown
        /// in the UI but can be omitted.
        /// </summary>
        [JsonProperty]
        public string Documentation { get; set; }
    }

    /// <summary>
    /// Descibe options to be used when registered for signature help events.
    /// </summary>
    public class SignatureHelpRegistrationOptions : TextDocumentRegistrationOptions
    {
        [JsonConstructor]
        public SignatureHelpRegistrationOptions()
            : this(null, null)
        {
        }

        public SignatureHelpRegistrationOptions(IEnumerable<char> triggerCharacters)
            : this(triggerCharacters, null)
        {
        }


        public SignatureHelpRegistrationOptions(IEnumerable<char> triggerCharacters,
            IEnumerable<DocumentFilter> documentSelector)
            : base(documentSelector)
        {
            TriggerCharacters = triggerCharacters;
        }

        /// <summary>
        /// The characters that trigger signature help automatically.
        /// </summary>
        [JsonProperty]
        public IEnumerable<char> TriggerCharacters { get; set; }
    }
}
