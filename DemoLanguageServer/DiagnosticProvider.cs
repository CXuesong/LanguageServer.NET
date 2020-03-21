using System;
using System.Collections.Generic;
using System.Text;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;
using Range = LanguageServer.VsCode.Contracts.Range;

namespace DemoLanguageServer
{
    public class DiagnosticProvider
    {

        public DiagnosticProvider()
        {

        }
        
        private static readonly string[] Keywords =
            {".NET Framework", ".NET Core", ".NET Standard", ".NET Compact", ".NET"};

        public ICollection<Diagnostic> LintDocument(TextDocument document, int maxNumberOfProblems)
        {
            var diag = new List<Diagnostic>();
            var content = document.Content;
            if (string.IsNullOrWhiteSpace(content))
            {
                diag.Add(new Diagnostic(DiagnosticSeverity.Hint,
                    new Range(new Position(0, 0), document.PositionAt(content?.Length ?? 0)),
                    "DLS", "DLS0001", "Empty document. Try typing something, such as \".net core\"."));
                return diag;
            }
            foreach (var kw in Keywords)
            {
                int pos = 0;
                while (pos < content.Length)
                {
                    pos = content.IndexOf(kw, pos, StringComparison.CurrentCultureIgnoreCase);
                    if (pos < 0) break;
                    var separatorPos = pos + kw.Length;
                    if (separatorPos < content.Length && char.IsLetterOrDigit(content, separatorPos))
                        continue;
                    var inputKw = content.Substring(pos, kw.Length);
                    if (inputKw != kw)
                    {
                        diag.Add(new Diagnostic(DiagnosticSeverity.Warning,
                            new Range(document.PositionAt(pos), document.PositionAt(separatorPos)), "DLS", "DLS1001", $"\"{inputKw}\" should be \"{kw}\"."));
                        if (diag.Count >= maxNumberOfProblems)
                        {
                            diag.Add(new Diagnostic(DiagnosticSeverity.Information,
                                new Range(document.PositionAt(pos), document.PositionAt(separatorPos)), "DLS", "DLS2001", "Too many messages, exiting…"));
                            return diag;
                        }
                    }
                    pos++;
                }
            }
            return diag;
        }
    }
}
