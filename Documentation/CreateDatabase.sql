-- NOTE: This is just a temporary stub showing how we might want to create the database

DROP TABLE Effects;

CREATE TABLE Effects
(
ID int PRIMARY KEY IDENTITY,
Attribute_ID int NOT NULL FOREIGN KEY REFERENCES Attributes(ID),
Magnitude int NOT NULL
);

INSERT INTO Attributes Values(0,nullEffect);
INSERT INTO Attributes Values(0,defaultAttack);