using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QualityEnforcer
{
    public class QualityRules
    {
        public QualityRules()
        {
            // Default rules
            TrimTrailingLines = true;
            TrimTrailingWhitespace = true;
            LineEndings = LineEndingStyle.Detect;
            Indentation = IndentationStyle.Detect;
            NumberOfSpaces = null;
        }

        /// <summary>
        /// If DetectLineEndings is false, this value is used to determine line ending style.
        /// </summary>
        public LineEndingStyle LineEndings { get; set; }
        /// <summary>
        /// If DetectIndentation is false, this value is used to determine indent style.
        /// </summary>
        public IndentationStyle Indentation { get; set; }
        /// <summary>
        /// If Indentation is set to use spaces, this number determines how many to use.
        /// </summary>
        public int? NumberOfSpaces { get; set; }
        /// <summary>
        /// If true, excess empty lines at the end of a file are removed.
        /// </summary>
        public bool TrimTrailingLines { get; set; }
        /// <summary>
        /// If true, excess tabs and spaces are trimmed from the end of each line.
        /// </summary>
        public bool TrimTrailingWhitespace { get; set; }

        public static QualityRules FromFile(string file)
        {
            QualityRules rules = new QualityRules();
            var lines = File.ReadAllLines(file);
            foreach (var line in lines)
            {
                if (line.StartsWith("* "))
                {
                    // Attempt to interpret as a rule
                    string key = line.Substring(2);
                    string value = null;
                    if (key.Contains(":"))
                    {
                        value = key.Substring(key.IndexOf(":") + 1).Trim().ToLower();
                        key = key.Remove(key.IndexOf(":"));
                    }
                    key = key.Trim().ToLower();
                    switch (key)
                    {
                        case "line endings":
                            if (value == null) break;
                            if (value == "crlf") rules.LineEndings = LineEndingStyle.CRLF;
                            if (value == "lf") rules.LineEndings = LineEndingStyle.LF;
                            break;
                        case "indentation":
                            if (value == null) break;
                            if (value == "tabs") rules.Indentation = IndentationStyle.Tabs;
                            int spaces;
                            if (value.EndsWith("spaces") && int.TryParse(
                                value.Remove(value.IndexOf(' ')), out spaces))
                            {
                                rules.Indentation = IndentationStyle.Spaces;
                                rules.NumberOfSpaces = spaces;
                            }
                            break;
                        case "trim trailing lines":
                            rules.TrimTrailingLines = true;
                            break;
                        case "trim trailing whitespace":
                            rules.TrimTrailingWhitespace = true;
                            break;
                    }
                }
            }
            return rules;
        }
    }
}
