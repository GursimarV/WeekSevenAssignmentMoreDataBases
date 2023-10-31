using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using WeekSevenAssignmentMoreDataBases.StuffForFiles;

namespace WeekSevenAssignmentMoreDataBases.SQLConnection
{
    //Most of the code is taken from Assignment 6
    internal class CharacterAccess
    {
        private string sqlConnectString = string.Empty;
        private DataBaseConnect connect;
        public CharacterAccess()
        {
            connect = DataBaseConnect.Instance();
            sqlConnectString = connect.PrepareDBConnect();
        }
        //Learned from Week 6 Powerpoint Prenstation
        //Insert the data about the types
        public void PutInType(string type) 
        {
            using(SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQL = @$"INSERT [dbo].[Types] ([Type]) VALUES('{type}')";
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var query = command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        //Learned from Week 6 Powerpoint Prenstation
        //Insert the data about the location
        public void PutInLocation(string location) 
        {
            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQL = @$"INSERT [dbo].[Map_Locations] ([Map_Location]) VALUES('{location}')";
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var query = command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        //Reads the CharacterData on the table and will output the info
        public void PutInCharacter(CharacterData character) 
        {
            string typeID = GetTypeIDFromTypeName(character.Type);
            string locationID = GetLocationIDFromLocationName(character.Map_Location);
            string isMagic = "NULL";
            string isSword = "NULL";
            string isOriginal = "NULL";

            if (character.Magic_User != null)
            {
                isMagic = $@"'{character.Magic_User}'";
            }

            if (character.Sword_Fighter != null)
            {
                isSword = $@"'{character.Sword_Fighter}'";
            }

            if (character.Original_character != null)
            {
                isOriginal = $@"'{character.Original_character}'";
            }
            //Learned from Week 6 Powerpoint Prenstation
            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQLInsertCharacter = $@"INSERT INTO [dbo].[Character] ([Name], [Type_Id], [Original_Character], [Sword_Fighter], [Magic_User], [Map_ID]) 
            VALUES('{character.Character}', {typeID}, {isOriginal}, {isSword}, {isMagic}, {locationID})";
                using (var command = new SqlCommand(inlineSQLInsertCharacter, conn))
                {
                    var query = command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        
        //Learned from Week 6 Powerpoint Prenstation
        public string GetTypeIDFromTypeName(string typeName)
        {
            string typeID = string.Empty;

            //This opens the connection to the table and creates a string using a SQL command
            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQLGetTypeID = $@"SELECT TOP 1 ID FROM [dbo].[Type] WHERE [dbo].[Type].[Type_Name] = '{typeName}'";
                // With the using statement, it creates a SQL Command object and will go through as a string and object
                using (var command = new SqlCommand(inlineSQLGetTypeID, conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        typeID = reader.GetValue(0).ToString();
                    }

                    reader.Close();
                }

                conn.Close();
            }

            if (typeID == string.Empty)
            {
                typeID = "NULL";
            }

            return typeID;
        }
        //Learned from Week 6 Powerpoint Prenstation
        public string GetLocationIDFromLocationName(string locationName)
        {
            string locationID = string.Empty;

            //This opens the connection to the table and creates a string using a SQL command
            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                if (locationName.Contains("'"))
                {
                    locationName = locationName.Replace("'", "''");
                }
                string inlineSQLGetLocationID = $@"SELECT TOP 1 ID FROM [dbo].[Location] WHERE [dbo].[Location].[Location_Name] = '{locationName}'";

                // With the using statement, it creates a SQL Command object and will go through as a string and object
                using (var command = new SqlCommand(inlineSQLGetLocationID, conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        locationID = reader.GetValue(0).ToString();
                    }

                    reader.Close();
                }

                conn.Close();
            }

            if (locationID == string.Empty)
            {
                locationID = "NULL";
            }

            return locationID;
        }
        public List<string> SwordUserNotHuman()
        {
            //using a directory path to the sql file similarly to the directory for csv file
            var NotHumanSwordUser = Path.Combine(Directory.GetCurrentDirectory(), "SQLFiles", "SwordNonHuman.sql");
            List<string> result = new List<string>();
            try
            {
                // With the using statement, it creates a SQL Command object and will go through as a string and object
                using (SqlConnection conn = new SqlConnection(sqlConnectString))
                {
                    conn.Open();

                    string inlineSQL = File.ReadAllText(NotHumanSwordUser);
                    using (var command = new SqlCommand(inlineSQL, conn))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"].ToString());
                        }
                    }
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public List<string> HumanWithNoLocation()
        {
            //using a directory path to the sql file similarly to the directory for csv file
            var LostHuman = Path.Combine(Directory.GetCurrentDirectory(), "SQLFiles", "Lost.sql");
            List<string> result = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(sqlConnectString))
                {
                    conn.Open();

                    string inlineSQL = File.ReadAllText(LostHuman);
                    using (var command = new SqlCommand(inlineSQL, conn))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"].ToString());
                        }
                    }
                    conn.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public List<CharacterData> EveryCharInfo()
        {
            //using a directory path to the sql file similarly to the directory for csv file
            List<CharacterData> listofCharInfo = new List<CharacterData>();
            var CharacterStats = Directory.GetCurrentDirectory() + @"\SQLFiles\FullReport.sql";

            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQL = File.ReadAllText(CharacterStats);
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();

                    //Reads out the Data about every character and everything about them
                    while (reader.Read())
                    {
                        CharacterData characterdata = new CharacterData();
                        characterdata.Character = reader.GetString(0);
                        characterdata.Type = reader.GetString(1);
                        characterdata.Map_Location = reader.GetString(2);
                        characterdata.Original_character = reader.GetBoolean(3);
                        characterdata.Sword_Fighter = reader.GetBoolean(4);
                        characterdata.Magic_User = reader.GetBoolean(5);
                        listofCharInfo.Add(characterdata);
                    }
                }
                conn.Close();
            }
            return listofCharInfo;
        }
    }
}
