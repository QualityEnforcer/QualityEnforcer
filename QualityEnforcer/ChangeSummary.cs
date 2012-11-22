using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityEnforcer
{
    public class ChangeSummary
    {
        public ChangeSummary()
        {
            IndentationStyleChange = IndentationStyleChange.None;
            LineEndingStyleChange = LineEndingStyleChange.None;
            TrimTrailingLines = TrimTrailingWhitespace = false;
        }

        public bool Any
        {
            get
            {
                return IndentationStyleChange != QualityEnforcer.IndentationStyleChange.None ||
                    LineEndingStyleChange != QualityEnforcer.LineEndingStyleChange.None ||
                    TrimTrailingWhitespace || TrimTrailingLines;
            }
        }
        public IndentationStyleChange IndentationStyleChange { get; set; }
        public LineEndingStyleChange LineEndingStyleChange { get; set; }
        public bool TrimTrailingWhitespace { get; set; }
        public bool TrimTrailingLines { get; set; }
    }

    public enum IndentationStyleChange
    {
        None,
        ToSpaces,
        ToTabs
    }

    public enum LineEndingStyleChange
    {
        None,
        ToLF,
        ToCRLF
    }
}
