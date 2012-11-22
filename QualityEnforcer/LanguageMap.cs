using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QualityEnforcer
{
    public class LanguageMap
    {
        private static Dictionary<string, string> Map;

        static LanguageMap()
        {
            Map = new Dictionary<string, string>();
            Map.Add("cs", "C#");
            Map.Add("c", "C");
            Map.Add("asm", "Assembly");
            Map.Add("S", "Assembly");
            Map.Add("dasm", "DCPU-16 Assembly");
            Map.Add("vb", "Visual Basic");
            Map.Add("py", "Python");
            Map.Add("html", "HTML");
            Map.Add("css", "CSS");
            Map.Add("js", "JavaScript");
            Map.Add("xml", "XML");
            Map.Add("json", "JSON");
            Map.Add("sh", "Shell Script");
            Map.Add("bat", "Batch Script");
            Map.Add("rb", "Ruby");
            Map.Add("java", "Java");
            Map.Add("php", "PHP");
            Map.Add("cpp", "C++");
            Map.Add("pl", "Perl");
            Map.Add("in", "");
            Map.Add("h", "C");
            Map.Add("m", "");
            Map.Add("as", "");
            Map.Add("d", "D");
            Map.Add("lua", "Lua");
            Map.Add("less", "LESS");
            Map.Add("scss", "SCSS");
        }

        public static string GetLanguageForExtension(string extension)
        {

        }
    }
}
