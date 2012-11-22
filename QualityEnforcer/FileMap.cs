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
        internal double AverageSpaces { get; set; } // Internal because it's deceptively named
        public int TrailingLines { get; set; }

        /// <summary>
        /// Indentation for each line in the file, where a higher number means more indentation.
        /// -1 is for empty lines.
        /// </summary>
        public int[] Indentation { get; set; }

        public string[] Lines { get; set; }

        public static FileMap GenerateMap(string path)
        {
            var map = new FileMap();
            var text = File.ReadAllText(path);

            map.LFUsed = text.Count(c => c == '\n');
            map.CRLFUsed = text.Count(c => c == '\r');
            map.LFUsed -= map.CRLFUsed;

            map.Lines = text.Replace("\r", "").Split('\n');
            map.Indentation = new int[map.Lines.Length];

            map.AverageSpaces = 0;

            int previousIndent = 0, currentIndent = 0;

            for (int i = 0; i < map.Lines.Length; i++)
            {
                string line = map.Lines[i];
                int indent;
                if (string.IsNullOrWhiteSpace(line))
                {
                    map.Indentation[i] = -1;
                    continue;
                }
                int spaces = 0;
                if (line.StartsWith(" "))
                {
                    map.SpacesUsed++;
                    indent = CountStart(line, ' ');
                    if (map.SpacesUsed != 0 &&
                        Math.Abs(indent - previousIndent) <
                        (map.AverageSpaces / map.SpacesUsed) * 0.5)
                    {
                        // If it's less than half of the average away from the last indent, assume it's a mistake
                        indent = previousIndent;
                    }
                    spaces = indent;
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
                if (spaces != 0 && currentIndent != 0)
                    map.AverageSpaces += spaces / currentIndent;
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
