CREATE TABLE Franchises
(
	Nummer			integer NOT NULL PRIMARY KEY,
	Name			varchar (100) NOT NULL
);

CREATE TABLE Cosplans
(
    Nummer			integer NOT NULL PRIMARY KEY,
    Franchise_Nr	integer NOT NULL,
	Bild_Nr			integer,
    Name			varchar(100) NOT NULL,
    ErstelltAm		date DEFAULT CURRENT_DATE NOT NULL,
    Erledigt		BOOLEAN DEFAULT 0 NOT NULL,
    ErledigtAm		date
);

CREATE TABLE Bilder
(
	Nummer			integer NOT NULL PRIMARY KEY,
	Cosplan_Nr		integer NOT NULL,
	Bild			BLOB SUB_TYPE 0 SEGMENT SIZE 80
);

CREATE TABLE Einstellungen
(
	Nummer			integer NOT NULL PRIMARY KEY,
	SettingKey				varchar(40) NOT NULL,
	SettingValue			varchar(40) NOT NULL
);