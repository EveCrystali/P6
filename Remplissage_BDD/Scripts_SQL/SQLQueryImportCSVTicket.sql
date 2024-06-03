-- Cr�er une table temporaire pour importer le CSV
-- DROP TABLE #TempTickets;
CREATE TABLE TempTickets (
    Produit NVARCHAR(50),
    Version NVARCHAR(50),
    SystemeExploitation NVARCHAR(50),
    DateCreation NVARCHAR(50),  -- Stocker la date comme cha�ne
    DateResolution NVARCHAR(50), -- Stocker la date comme cha�ne
    Statut NVARCHAR(50),
    Probleme NVARCHAR(MAX),
    Resolution NVARCHAR(MAX)
);

-- Utiliser BULK INSERT pour charger les donn�es du CSV dans la table temporaire
BULK INSERT TempTickets
FROM 'J:\Open Classrooms\P6\ODTTicketToCSV\output.csv'
WITH (
    FIELDTERMINATOR = ';',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2,
    CODEPAGE = '65001', -- UTF-8
    FORMAT = 'CSV'
);

