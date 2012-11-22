using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    /// <summary>
    /// Analyzes and enforces consistent standards across an entire project.
    /// </summary>
    public class Enforcer
    {
        /// <summary>
        /// A list of all file extensions supported by QualityEnforcer
        /// </summary>
        public static readonly string[] FileFilters = new string[]
            {
                "*.cs", "*.c", "*.asm", "*.S", "*.dasm", "*.vb", "*.py", "*.html", "*.css", "*.js", "*.xml",
                "*.json", "*.sh", "*.bat", "*.rb", "*.java", "*.php", "*.cpp", "*.pl", "*.in", "*.h", "*.m",
                "*.as", "*.d", "*.lua", "*.less", "*.scss"
            };

        public static readonly string[] VersionControlFolders = new string[]
        {
            ".git", ".svn", ".hg"
        };

        public static Project AnalyzeDirectory(string path)
        {
            var project = new Project(path);

            // Get a file list
            project.CodeFiles = new List<string>();
            foreach (var ext in FileFilters)
                project.CodeFiles.AddRange(Directory.GetFiles(project.Path, ext, SearchOption.AllDirectories));

            // Omit version control
            project.CodeFiles = new List<string>(project.CodeFiles.Where(f =>
                {
                    foreach (var vcs in VersionControlFolders)
                    {
                        if (f.StartsWith(vcs))
                            return false;
                    }
                    return true;
                }));

            int lfCount = 0, crlfCount = 0, spaceCount = 0, tabCount = 0;
            double averageSpaces = 0;

            foreach (var file in project.CodeFiles)
            {
                var ext = Path.GetExtension(file);
                var lang = LanguageMap.GetLanguageForExtension(ext);
                if (!project.LanguageDistribution.ContainsKey(lang))
                    project.LanguageDistribution[lang] = 1;
                else
                    project.LanguageDistribution[lang]++;
                var map = FileMap.GenerateMap(file);
                lfCount += map.LFUsed;
                crlfCount += map.CRLFUsed;
                spaceCount += map.SpacesUsed;
                tabCount += map.TabsUsed;
                averageSpaces += map.AverageSpaces;
            }

            if (lfCount < crlfCount)
                project.LineEndings = LineEndingStyle.CRLF;
            else
                project.LineEndings = LineEndingStyle.LF;

            if (spaceCount < tabCount)
                project.Indentation = IndentationStyle.Tabs;
            else
                project.Indentation = IndentationStyle.Spaces;

            project.NumberOfSpaces = (int)(averageSpaces / spaceCount);

            return project;
        }

        public static ChangeSummary EnforceQuality(Project project, QualityRules rules)
        {
            ChangeSummary summary = new ChangeSummary();
            string indent, lineEnding;
            // Set up indent and line ending stles
            if (rules.Indentation == IndentationStyle.Detect)
            {
                if (project.Indentation == IndentationStyle.Spaces)
                {
                    indent = "";
                    int spaces = project.NumberOfSpaces;
                    if (rules.NumberOfSpaces != null)
                        spaces = rules.NumberOfSpaces.Value;
                    for (int i = 0; i < spaces; i++) indent += " ";
                }
                else
                    indent = "\t";
            }
            else if (rules.Indentation == IndentationStyle.Spaces)
            {
                indent = "";
                for (int i = 0; i < rules.NumberOfSpaces; i++) indent += " ";
                project.Indentation = IndentationStyle.Spaces;
            }
            else
            {
                indent = "\t";
                project.Indentation = IndentationStyle.Tabs;
            }
            if (rules.LineEndings == LineEndingStyle.Detect)
            {
                if (project.LineEndings == LineEndingStyle.CRLF)
                    lineEnding = "\r\n";
                else
                    lineEnding = "\n";
            }
            else if (rules.LineEndings == LineEndingStyle.CRLF)
            {
                lineEnding = "\r\n";
                project.LineEndings = LineEndingStyle.CRLF;
            }
            else
            {
                lineEnding = "\n";
                project.LineEndings = LineEndingStyle.LF;
            }
            // Apply changes
            foreach (var file in project.CodeFiles)
            {
                var map = FileMap.GenerateMap(file);
                // Detect changes
                if (map.LFUsed != map.CRLFUsed && summary.LineEndingStyleChange == LineEndingStyleChange.None)
                {
                    if (project.LineEndings == LineEndingStyle.CRLF)
                        summary.LineEndingStyleChange = LineEndingStyleChange.ToCRLF;
                    else
                        summary.LineEndingStyleChange = LineEndingStyleChange.ToLF;
                }
                if (map.TabsUsed != map.SpacesUsed && summary.IndentationStyleChange == IndentationStyleChange.None)
                {
                    if (project.Indentation == IndentationStyle.Spaces)
                        summary.IndentationStyleChange = IndentationStyleChange.ToSpaces;
                    else
                        summary.IndentationStyleChange = IndentationStyleChange.ToTabs;
                }
                // Create new file based on old
                File.Delete(file);
                var writer = new StreamWriter(file);
                int end = map.Lines.Length;
                if (rules.TrimTrailingLines)
                {
                    if (map.TrailingLines != 0)
                    {
                        summary.TrimTrailingLines = true;
                        end -= map.TrailingLines;
                    }
                }
                for (int i = 0; i < end; i++)
                {
                    // Reconstruct file
                    var line = map.Lines[i];
                    if (rules.TrimTrailingWhitespace)
                    {
                        if (summary.TrimTrailingWhitespace == false)
                            summary.TrimTrailingWhitespace = line.EndsWith(" ") || line.EndsWith("\t");
                        line = line.TrimEnd(' ', '\t');
                    }
                    line = line.TrimStart(' ', '\t');
                    for (int j = 0; j < map.Indentation[i]; j++)
                        line = indent + line;
                    if (i != end - 1)
                        writer.Write(line + lineEnding);
                    else
                        writer.Write(line);
                }
                writer.Close();
            }
            return summary;
        }
    }
}
