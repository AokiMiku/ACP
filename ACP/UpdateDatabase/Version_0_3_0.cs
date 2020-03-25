namespace ACP.Datenbankversionen
{
	using ApS.Databases.Firebird;

	internal class Version_0_3_0 : UpdateDatabase
	{
		private readonly string ToDos = "EXECUTE BLOCK AS BEGIN IF(NOT EXISTS(SELECT 1 FROM RDB$RELATIONS R WHERE R.RDB$RELATION_NAME = 'TODOS')) THEN BEGIN EXECUTE STATEMENT 'CREATE TABLE ToDos(Nummer integer NOT NULL PRIMARY KEY, Cosplan_Nr integer NOT NULL, Bezeichnung varchar(100), ProzentErledigt integer DEFAULT 0 NOT NULL, Kategorie_Nr integer NOT NULL, Kosten decimal (10,2), Zeit time);'; EXECUTE STATEMENT 'CREATE SEQUENCE GEN_ToDos_ID;'; EXECUTE STATEMENT 'create trigger ToDos_bi for ToDos active before insert position 0 as begin if (new.nummer is null) then new.nummer = gen_id(GEN_ToDos_ID,1); end'; END END;";
		private readonly string ToDosAddForeignKeyCosplans = "EXECUTE BLOCK AS BEGIN EXECUTE STATEMENT 'ALTER TABLE ToDos ADD FOREIGN KEY (Cosplan_Nr) REFERENCES Cosplans (Nummer) ON DELETE CASCADE ON UPDATE CASCADE;;'; END;";
		private readonly string ToDoKategorien = "EXECUTE BLOCK	AS BEGIN IF(NOT EXISTS(SELECT 1 FROM RDB$RELATIONS R WHERE R.RDB$RELATION_NAME = 'TODOKATEGORIEN')) THEN BEGIN EXECUTE STATEMENT 'CREATE TABLE ToDoKategorien(Nummer integer NOT NULL PRIMARY KEY, Bezeichnung varchar(40));'; EXECUTE STATEMENT 'CREATE SEQUENCE GEN_ToDoKategorien_ID;'; EXECUTE STATEMENT 'create trigger ToDoKategorien_bi for ToDoKategorien active before insert position 0 as begin if (new.nummer is null) then new.nummer = gen_id(GEN_ToDoKategorien_ID,1); end'; END END;";
		private readonly string ToDosAddForeignKeyKategorien = "EXECUTE BLOCK AS BEGIN EXECUTE STATEMENT 'ALTER TABLE ToDos ADD FOREIGN KEY (Kategorie_Nr) REFERENCES ToDoKategorien (Nummer) ON DELETE CASCADE ON UPDATE CASCADE;;'; END;";
		private readonly string ToDoKategorienInsertKaufen = "EXECUTE BLOCK AS BEGIN IF(NOT EXISTS(SELECT 1 FROM ToDoKategorien E WHERE E.Bezeichnung = 'Kaufen')) THEN BEGIN EXECUTE STATEMENT 'INSERT INTO ToDoKategorien (Bezeichnung) VALUES (''Kaufen'')'; END END;";
		private readonly string ToDoKategorienInsertMachen = "EXECUTE BLOCK AS BEGIN IF(NOT EXISTS(SELECT 1 FROM ToDoKategorien E WHERE E.Bezeichnung = 'Machen')) THEN BEGIN EXECUTE STATEMENT 'INSERT INTO ToDoKategorien (Bezeichnung) VALUES (''Machen'')'; END END;";

		public Version_0_3_0() : base()
		{
			base.AddStatements(ToDos);
			base.AddStatements(ToDosAddForeignKeyCosplans);
			base.AddStatements(ToDoKategorien);
			base.AddStatements(ToDosAddForeignKeyKategorien);
			base.AddStatements(ToDoKategorienInsertKaufen);
			base.AddStatements(ToDoKategorienInsertMachen);
		}
	}
}