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

            foreach (var file in project.CodeFiles)
            {
                var map = FileMap.GenerateMap(file);
                lfCount += map.LFUsed;
                crlfCount += map.CRLFUsed;
                spaceCount += map.SpacesUsed;
                tabCount += map.TabsUsed;
            }

            if (lfCount < crlfCount)
                project.LineEndings = LineEndingStyle.CRLF;
            else
                project.LineEndings = LineEndingStyle.LF;

            if (spaceCount < tabCount)
                project.Indentation = IndentationStyle.Tabs;
            else
                project.Indentation = IndentationStyle.Spaces;

            return project;
        }

        public static void EnforceQuality(Project project, QualityRules rules)
        {
            foreach (var file in project.CodeFiles)
            {
                var map = FileMap.GenerateMap(file);
                // Create new file based on old
            }
        }
    }
}
