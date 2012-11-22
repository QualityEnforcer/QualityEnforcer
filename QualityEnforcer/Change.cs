using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityEnforcer
{
    public enum Change
    {
        LFtoCRLF,
        CRLFtoLF,
        TabsToSpaces,
        SpacesToTabs,
        TrimWhitespace,
        TrimEmptyLines
    }
}
