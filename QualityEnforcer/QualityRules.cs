﻿using System;
using System.Collections.Generic;
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
    }
}
