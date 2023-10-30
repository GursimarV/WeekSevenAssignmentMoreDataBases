SELECT Characters.ID, Characters.Character, Types.Type, Map_Locations.Map_Location, Character_Info.Original_Character, Character_Info.Sword_Fighter, Character_Info.Magic_User
FROM Character_Info
INNER JOIN dbo.Characters 
ON Character_Info.CharacterID = Characters.ID
LEFT OUTER JOIN dbo.Map_Locations 
ON Characters.MapLocationID = Map_Locations.ID
LEFT OUTER JOIN dbo.Types 
ON Characters.TypeID = Types.ID
