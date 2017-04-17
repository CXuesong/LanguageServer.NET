using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.Server;

namespace LanguageServer.VsCode
{
    internal class JsonUtility
    {
        public TraceLevel ParseTraceLevel(string expr)
        {
            switch (expr)
            {
                case "off": return TraceLevel.Off;
                case "messages": return TraceLevel.Messages;
                case "verbose": return TraceLevel.Verbose;
                default: throw new ArgumentException("Invalid TraceLevel: " + expr + ".", nameof(expr));
            }
        }
    }
}
