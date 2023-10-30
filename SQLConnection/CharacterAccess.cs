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
    internal class CharacterAccess
    {
        private string sqlConnectString = string.Empty;
        private DataBaseConnect connect;
        public CharacterAccess()
        {
            connect = DataBaseConnect.Instance();
            sqlConnectString = connect.PrepareDBConnect();
        }
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
        public string GetTypeIDFromTypeName(string typeName)
        {
            string typeID = string.Empty;

            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQLGetTypeID = $@"SELECT TOP 1 ID FROM [dbo].[Type] WHERE [dbo].[Type].[Type_Name] = '{typeName}'";
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
        public string GetLocationIDFromLocationName(string locationName)
        {
            string locationID = string.Empty;

            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                if (locationName.Contains("'"))
                {
                    locationName = locationName.Replace("'", "''");
                }
                string inlineSQLGetLocationID = $@"SELECT TOP 1 ID FROM [dbo].[Location] WHERE [dbo].[Location].[Location_Name] = '{locationName}'";

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
            var NotHumanSwordUser = Path.Combine(Directory.GetCurrentDirectory(), "SQLFiles", "SwordNonHuman.sql");
            List<string> result = new List<string>();
            try
            {
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
            List<CharacterData> listofCharInfo = new List<CharacterData>();
            var CharacterStats = Directory.GetCurrentDirectory() + @"\SQLFiles\FullReport.sql";

            using (SqlConnection conn = new SqlConnection(sqlConnectString))
            {
                conn.Open();

                string inlineSQL = File.ReadAllText(CharacterStats);
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CharacterData character = new CharacterData();
                        character.Character = reader.GetString(0);
                        character.Type = reader.GetString(1);
                        character.Map_Location = reader.GetString(2);
                        character.Original_character = reader.GetBoolean(3);
                        character.Sword_Fighter = reader.GetBoolean(4);
                        character.Magic_User = reader.GetBoolean(5);
                        listofCharInfo.Add(character);
                    }
                }
                conn.Close();
            }
            return listofCharInfo;
        }
    }
}
