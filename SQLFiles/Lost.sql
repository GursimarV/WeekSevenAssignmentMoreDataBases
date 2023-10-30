SELECT Characters.Character
FROM Characters
LEFT JOIN Map_Locations
ON Characters.MapLocationID = Map_Locations.ID
WHERE Map_Locations.Map_Location is NULL