using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QualityEnforcer
{
    /// <summary>
    /// Represents indentation in a given file.
    /// </summary>
    public class FileMap
    {
        private FileMap()
        {
            SpacesUsed = TabsUsed = CRLFUsed = LFUsed = 0;
        }

        public const int IndentationThreshold = 8;
        public const int GuessedTabSize = 4; // Four spaces

        public int SpacesUsed { get; set; }
        public int TabsUsed { get; set; }
        public int CRLFUsed { get; set; }
        public int LFUsed { get; set; }
        public int TrailingLines { get; set; }

        /// <summary>
        /// Indentation for each line in the file, where a higher number means more indentation.
        /// -1 is for empty lines.
        /// </summary>
        public int[] Indentation { get; set; }

        public static FileMap GenerateMap(string path)
        {
            var map = new FileMap();
            var text = File.ReadAllText(path);

            map.LFUsed = text.Count(c => c == '\n');
            map.CRLFUsed = text.Count(c => c == '\r');
            map.LFUsed -= map.CRLFUsed;

            var lines = text.Replace("\r", "").Split('\n');
            map.Indentation = new int[lines.Length];

            int previousIndent = 0, currentIndent = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int indent;
                if (string.IsNullOrWhiteSpace(line))
                {
                    map.Indentation[i] = -1;
                    continue;
                }
                if (line.StartsWith(" "))
                {
                    map.SpacesUsed++;
                    indent = CountStart(line, ' ');
                }
                else if (line.StartsWith("\t"))
                {
                    map.TabsUsed++;
                    indent = CountStart(line, '\t') * GuessedTabSize;
                }
                else
                {
                    previousIndent = currentIndent = 0;
                    map.Indentation[i] = 0;
                    continue;
                }
                if (previousIndent < indent && indent - previousIndent < IndentationThreshold) // Ignore indentation too far out there
                    currentIndent++;
                else if (previousIndent > indent)
                    currentIndent--;
                map.Indentation[i] = currentIndent;
                previousIndent = indent;
            }

            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == '\n')
                    map.TrailingLines++;
                else if (text[i] == '\r') ;
                else
                    break;
            }

            return map;
        }

        private static int CountStart(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != c)
                    return i;
            }
            return s.Length;
        }
    }
}
