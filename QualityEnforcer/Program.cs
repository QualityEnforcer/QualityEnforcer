using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string directory = null;
            string analysis = null;
            string summary = null;
            string rules = null;
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
                        case "--summary":
                            summary = args[++i];
                            break;
                        case "--rules":
                            rules = args[++i];
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
                File.WriteAllText(analysis, GenerateAnalysis(project));
                return;
            }
            else
            {
                // Enforce style
                QualityRules qualityRules = new QualityRules();
                if (rules != null)
                    qualityRules = QualityRules.FromFile(rules);
                else if (File.Exists(Path.Combine(directory, "CONTRIBUTING.md")))
                    qualityRules = QualityRules.FromFile(Path.Combine(directory, "CONTRIBUTING.md"));
                var changes = Enforcer.EnforceQuality(project, qualityRules);
                if (summary != null)
                    File.WriteAllText(summary, GenerateSummary(project, changes));
            }
        }

        public static string GenerateAnalysis(Project project)
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

            return template;
        }

        public static string GenerateSummary(Project project, ChangeSummary changes)
        {
            var reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream("QualityEnforcer.SummaryTemplate.txt"));
            var template = reader.ReadToEnd();
            reader.Close();

            var changeList = "";
            if (changes.IndentationStyleChange == IndentationStyleChange.ToSpaces)
                changeList += "* Converted tabs to spaces" + Environment.NewLine;
            if (changes.IndentationStyleChange == IndentationStyleChange.ToTabs)
                changeList += "* Converted spaces to tabs" + Environment.NewLine;
            if (changes.LineEndingStyleChange == LineEndingStyleChange.ToCRLF)
                changeList += "* Converted LF to CRLF" + Environment.NewLine;
            if (changes.LineEndingStyleChange == LineEndingStyleChange.ToLF)
                changeList += "* Converted CRLF to LF" + Environment.NewLine;
            if (changes.TrimTrailingLines)
                changeList += "* Removed trailing newlines" + Environment.NewLine;
            if (changes.TrimTrailingWhitespace)
                changeList += "* Remoted trailing whitespace" + Environment.NewLine;

            template = template.Replace("{changes}", changeList);

            return template;
        }
    }
}
