using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    public class Project
    {
        public string Path { get; set; }
        public List<string> CodeFiles { get; set; }
        public Dictionary<string, int> LanguageDistribution { get; set; }
        public LineEndingStyle LineEndings { get; set; }
        public IndentationStyle Indentation { get; set; }
        public int NumberOfSpaces { get; set; }

        public Project(string path)
        {
            Path = path;
            CodeFiles = new List<string>();
            LanguageDistribution = new Dictionary<string, int>();
            NumberOfSpaces = 4;
        }
    }
}
