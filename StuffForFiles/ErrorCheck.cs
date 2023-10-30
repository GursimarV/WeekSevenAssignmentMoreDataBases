using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal class ErrorCheck
    {
        //Learned from the in class demo in week 5 to put out error message and where the source is from
        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }

        public ErrorCheck(string message, string source)
        {
            ErrorMessage = message;
            ErrorSource = source;
        }
    }
}
