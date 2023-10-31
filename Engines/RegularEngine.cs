using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeekSevenAssignmentMoreDataBases.SQLConnection;
using WeekSevenAssignmentMoreDataBases.StuffForFiles;

namespace WeekSevenAssignmentMoreDataBases.Engines
{
    internal abstract class RegularEngine
    {
        public CharacterAccess characterAccess;
        public RegularEngine()
        {
            characterAccess = new CharacterAccess();
        }
        internal virtual List<ErrorCheck> ProcessFile(IFile file)
        {
            //To help check for any errors in the code and was used from Assignment #5
            List<ErrorCheck> ShowError = new List<ErrorCheck>();

            try
            {
                List<CharacterData> characterData = CreateCharacter(file);
                List<string> typeData = PutTypeOnCharacter(characterData);
                List<string> locationData = PutCharacterLocation(characterData);

                ChangesToSQL(characterData, typeData, locationData);
                CreateResultFiles();
            }
            //To catch errors that are in the code, learned from in class demo in week 5
            catch (IOException ioe)
            {
                ShowError.Add(new ErrorCheck(ioe.Message, ioe.Source ?? "Unknown"));
            }
            catch (Exception ex)
            {
                ShowError.Add(new ErrorCheck(ex.Message, ex.Source ?? "Unknown"));
            }
            return ShowError;
        }
        //Adds the data of the location of character as a string
        List<string> PutCharacterLocation(List<CharacterData> characterData)
        {
            List<string> locationData = new List<string>();

            foreach (var location in characterData)
            {
                if (!locationData.Contains(location.Map_Location) && (location.Map_Location != ""))
                {
                    locationData.Add(location.Map_Location);
                }
            }
            return locationData;
        }
        //Adds the data of the type of character as a string
        List<string> PutTypeOnCharacter(List<CharacterData> characterData)
        {
            List<string> typeData = new List<string>();

            foreach (var type in characterData)
            {
                if (!typeData.Contains(type.Type) && (type.Type != ""))
                {
                    typeData.Add(type.Type);
                }
            }
            return typeData;
        }
        private List<CharacterData> CreateCharacter(IFile file)
        {
            List<CharacterData> characterData = new List<CharacterData>();

            using (StreamReader sourceRead = new StreamReader(file.FilePath))
            {
                var line = sourceRead.ReadLine();
                if (line.StartsWith("Character,"))
                {
                    line = sourceRead.ReadLine();
                }

                while (line != null)
                {
                    //Spliting the data based off the 
                    var props = line.Split(',');

                    string name = props[0];
                    string type = props[1];
                    string location = props[2];

                    bool? isOriginal = null;
                    if (props[3].Trim() != "")
                    {
                        isOriginal = Convert.ToBoolean(props[3]);
                    }


                    bool? ismagic = null;
                    if (props[5].Trim() != "")
                    {
                        ismagic = Convert.ToBoolean(props[5]);
                    }

                    bool? isSword = null;
                    if (props[4].Trim() != "")
                    {
                        isSword = Convert.ToBoolean(props[4]);
                    }
                    //Spits out the character data from the table
                    CharacterData temp = new CharacterData();
                    temp.Character = name.Trim();
                    temp.Type = type.Trim().ToUpper();
                    temp.Map_Location = location.Trim().ToUpper();
                    temp.Original_character = isOriginal;
                    temp.Sword_Fighter = isSword;
                    temp.Magic_User = ismagic;
                    characterData.Add(temp);
                    line = sourceRead.ReadLine();
                }
            }
            return characterData;
        }
        //Runs every SQL commands that are called to maintain the Produce table
        //The List is the List of Produce objects named produce
        public void ChangesToSQL(List<CharacterData> character, List<string> Type, List<string> Location)
        {
            CharacterAccess characterAccess = new CharacterAccess();
            //Insert items from Produce.txt into DB
            foreach (CharacterData characters in character)
            {
                characterAccess.PutInCharacter(characters);
            }
            foreach (string types in Type)
            {
                characterAccess.PutInType(types);
            }
            foreach (string locations in Location)
            {
                characterAccess.PutInLocation(locations);
            }
        }

        //Creates the about files and writes the data of the results after the parsing of the files
        public void CreateResultFiles()
        {
            if (Directory.Exists("./Results") == false)
            {
                Directory.CreateDirectory("./Results");
            }

            using (StreamWriter sourceWrite = new StreamWriter("./Results/Full Report.txt"))
            {
                sourceWrite.WriteLine("Character, Type, Map_Location, Original_Character, Sword_Fighter, Magic_User");
                List<CharacterData> result = characterAccess.EveryCharInfo();

                foreach (CharacterData characterData in result)
                {
                    sourceWrite.WriteLine(characterData.ToString());
                }
            }

            using (StreamWriter sourceWrite = new StreamWriter("./Results/Lost.txt"))
            {
                sourceWrite.WriteLine("Character, Type, Map_Location, Original_Character, Sword_Fighter, Magic_User");
                List<string> result = characterAccess.HumanWithNoLocation();

                foreach (string typeData in result)
                {
                    sourceWrite.WriteLine(typeData);
                }
            }

            using (StreamWriter sourceWrite = new StreamWriter("./Results/SwordNonHuman.txt"))
            {
                sourceWrite.WriteLine("Character, Type, Map_Location, Original_Character, Sword_Fighter, Magic_User");
                List<string> result = characterAccess.SwordUserNotHuman();

                foreach (string locationData in result)
                {
                    sourceWrite.WriteLine(locationData);
                }
            }
        }
    }
}
