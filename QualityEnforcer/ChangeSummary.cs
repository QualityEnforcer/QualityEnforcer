using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityEnforcer
{
    public class ChangeSummary
    {
        public TabStyleChange TabStyleChange { get; set; }
        public LineEndingStyleChange LineEndingStyleChange { get; set; }
        public bool TrimTrailingWhitespace { get; set; }
        public bool TrimTrailingLines { get; set; }
    }

    public enum TabStyleChange
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
