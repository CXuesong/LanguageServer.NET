namespace VSCode.JsonRpc
{
    /// <summary>
    /// Error codes defined by the JSON-RPC 2.0 specification.
    /// </summary>
    public static class ErrorCodes
    {
        /// <summary>
        /// Internal JSON-RPC error.
        /// </summary>
        public const int InternalError = -32603;

        /// <summary>
        /// Invalid method parameter(s).
        /// </summary>
        public const int InvalidParams = -32602;

        /// <summary>
        /// The JSON sent is not a valid Request object.
        /// </summary>
        public const int InvalidRequest = -32600;

        /// <summary>
        /// The method does not exist / is not available.
        /// </summary>
	    public const int MethodNotFound = -32601;

        /// <summary>
        /// Invalid JSON was received by the server. An error occurred on the server while parsing the JSON text.
        /// </summary>
        public const int ParseError = -32700;
    }
}
