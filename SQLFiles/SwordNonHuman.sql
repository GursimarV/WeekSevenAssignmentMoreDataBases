SELECT Characters.TypeID, Characters.Character
FROM Characters
LEFT JOIN Types
ON Characters.TypeID = Types.ID
LEFT JOIN Character_Info
ON Character_Info.CharacterID = Characters.ID
WHERE Character_Info.Sword_Fighter = 'TRUE' and Types.Type != 'Human'