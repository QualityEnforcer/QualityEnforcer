using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    public enum LineEndingStyle
    {
        Detect,
        LF,
        CRLF
    }

    public enum IndentationStyle
    {
        Detect,
        Tabs,
        Spaces
    }
}
