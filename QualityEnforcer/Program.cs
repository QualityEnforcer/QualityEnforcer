using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string directory = null;
            string analysis = null;
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    switch (arg)
                    {
                        case "--analysis":
                            analysis = args[++i];
                            break;
                        default:
                            Console.WriteLine("Invalid parameters. Use QualityEnforcer.exe --help for more information.");
                            return;
                    }
                }
                else
                {
                    if (directory == null)
                        directory = arg;
                    else
                    {
                        Console.WriteLine("Invalid parameters. Use QualityEnforcer.exe --help for more information.");
                        return;
                    }
                }
            }
            if (directory == null)
            {
                Console.WriteLine("Invalid parameters. Use QualityEnforcer.exe --help for more information.");
                return;
            }
            var project = Enforcer.AnalyzeDirectory(directory);
            if (analysis != null)
            {
                GenerateAnalysis(project, analysis);
                return;
            }
            else
            {
                // Enforce style
                var changes = Enforcer.EnforceQuality(project, new QualityRules());
            }
        }

        private static void GenerateAnalysis(Project project, string analysis)
        {
            var reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("QualityEnforcer.AnalysisTemplate.txt"));
            var template = reader.ReadToEnd();
            reader.Close();

            // Handle language list
            var languages = "";
            var languageList = project.LanguageDistribution.OrderByDescending(kvp => kvp.Value);
            foreach (var language in languageList)
                languages += "* " + language.Key + ": " + language.Value + " file" + 
                    (language.Value > 1 ? "s" : "") + Environment.NewLine;
            template = template.Replace("{languages}", languages);

            // Static variables
            template = template.Replace("{line-endings}", project.LineEndings.ToString());
            template = template.Replace("{indentation}", project.Indentation.ToString());

            File.WriteAllText(analysis, template);
        }
    }
}
