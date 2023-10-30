using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekSevenAssignmentMoreDataBases.StuffForFiles
{
    internal class CharacterData
    {
        public string Character { get; set; }
        public string Type { get; set; }
        public string Map_Location { get; set; }
        public bool? Original_character { get; set; }
        public bool? Sword_Fighter { get; set; }
        public bool? Magic_User { get; set; }

        public CharacterData(string character, string type, string map_Location)
        {
            Character = character;
            Type = type;
            Map_Location = map_Location;
        }

        public override string ToString()
        {
            return $"{Character},{Type},{Map_Location},{Original_character.ToString()},{Sword_Fighter.ToString()},{Magic_User.ToString()}";
        }
    }
}
