using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal interface IFile
    {
        string FilePath { get; set; }
        string? Delimiter { get; set; }
        string Extension { get; set; }
    }
}
