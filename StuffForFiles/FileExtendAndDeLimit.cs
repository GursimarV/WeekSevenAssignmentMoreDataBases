using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal class FileExtendAndDeLimit
    {
        //The extensions of the files in the files folder
        public sealed class FileExtensions
        {
            public static string CSV => ".csv";
        }

        //The delimiters that affects the pipe file used in assignment 4
        public sealed class FileDelimiter
        {
            public static string CSV => "|";
        }
    }
}
