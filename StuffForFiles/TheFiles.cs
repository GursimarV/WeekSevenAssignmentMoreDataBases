using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal class TheFiles : IFile
    {
        //Learned from assignment 4 
        public string FilePath { get; set; }
        //Learned about the public string? Delimiter { get; set; } = null; from In Class Demo week 4/5
        public string? Delimiter { get; set; } = null;
        public string Extension { get; set; }

        public TheFiles(string filepath, string extension, string delimiter = null)
        {
            FilePath = filepath;
            Delimiter = delimiter;
            Extension = extension;
        }
    }
}
