using System.Collections.Generic;
using Avalonia.Controls.Documents;

namespace VeloxDev.Wiki.Views.WikiCOMs.Tools.SyntaxAnalyzer
{
    public interface ISyntaxHighlighter
    {
        public IEnumerable<Inline> Highlight(string code);
    }
}