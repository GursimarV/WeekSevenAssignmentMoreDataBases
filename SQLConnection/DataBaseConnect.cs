using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeekSevenAssignmentMoreDataBases.StuffForFiles;

namespace WeekSevenAssignmentMoreDataBases.SQLConnection
{
    internal class DataBaseConnect
    {
        private DataBaseConnect()
        {

        }

        //Help from: https://www.tutorialsteacher.com/csharp/singleton
        private static DataBaseConnect instance = null;

        public static DataBaseConnect Instance()
        {
            if (instance == null)
            {
                instance = new DataBaseConnect();
            }
            return instance;
        }

        //Learned from Week 6 Powerpoint presentation
        public string PrepareDBConnect()
        {
            //This is used to make the connection into string
            SqlConnectionStringBuilder connectBuild = new SqlConnectionStringBuilder();
            connectBuild["server"] = @"(localdb)\MSSQLLocalDB";
            connectBuild["Trusted_Connection"] = true;
            connectBuild["Integrated Security"] = "SSPI";
            connectBuild["Initial Catalog"] = "PROG260FA23";

            return connectBuild.ToString();
        }
    }
}
