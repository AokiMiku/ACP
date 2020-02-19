CREATE OR ALTER VIEW CosplansV
AS
SELECT c.Nummer, f.Name as Franchise, c.Name as Cosplan, c.Erledigt
            FROM Cosplans c
            LEFT OUTER JOIN Franchises f ON c.franchise_nr = f.nummer
            ORDER BY c.nummer
;
