using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LanguageServer.VsCode.JsonRpc
{
    /// <summary>
    /// Reads messages from console. This class is used for diagnostic purpose.
    /// </summary>
    /// <remarks>To make it possible for user to enter a long message, we use a single line containing a dot "." to indicate the end of message.</remarks>
    public class ConsoleMessageReader : MessageReader
    {
        /// <inheritdoc />
        public override Message Read()
        {
            var sb = new StringBuilder();
            string line;
            while ((line = Console.ReadLine()) != ".")
            {
                sb.AppendLine(line);
            }
            using (var sr = new StringReader(sb.ToString()))
            {
                return RpcSerializer.DeserializeMessage(sr);
            }
        }
    }
}
