using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var project = Enforcer.AnalyzeDirectory("Test/");
            Enforcer.EnforceQuality(project, new QualityRules());
        }
    }
}
