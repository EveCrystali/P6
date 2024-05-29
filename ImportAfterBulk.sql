-- Insérer les données uniques dans la table Produit
INSERT INTO Produit (nom)
SELECT DISTINCT t.Produit
FROM TempTickets t
WHERE NOT EXISTS (
    SELECT 1 
    FROM Produit p 
    WHERE p.nom = t.Produit
);

-- Insérer les données uniques dans la table Systeme_exploitation
INSERT INTO Systeme_exploitation (nom)
SELECT DISTINCT t.SystemeExploitation
FROM TempTickets t
WHERE NOT EXISTS (
    SELECT 1 
    FROM Systeme_exploitation se 
    WHERE se.nom = t.SystemeExploitation
);

-- Insérer les données uniques dans la table Statut
INSERT INTO Statut (nom)
SELECT DISTINCT t.Statut
FROM TempTickets t
WHERE NOT EXISTS (
    SELECT 1 
    FROM Statut s 
    WHERE s.nom = t.Statut
);

-- Insérer les données uniques dans la table Version
INSERT INTO Version (nom, produit_id)
SELECT DISTINCT t.Version, p.id
FROM TempTickets t
JOIN Produit p ON t.Produit = p.nom
WHERE NOT EXISTS (
    SELECT 1 
    FROM Version v 
    WHERE v.nom = t.Version AND v.produit_id = p.id
);

-- Insérer les données dans la table Systeme_exploitation_has_Version
INSERT INTO Systeme_exploitation_has_Version (version_id, systeme_exploitation_id)
SELECT DISTINCT v.id, se.id
FROM TempTickets t
JOIN Version v ON t.Version = v.nom
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
WHERE NOT EXISTS (
    SELECT 1 
    FROM Systeme_exploitation_has_Version sv 
    WHERE sv.version_id = v.id AND sv.systeme_exploitation_id = se.id
);

-- Insérer les données dans la table Ticket
INSERT INTO Ticket (date_de_creation, date_de_resolution, produit_id, version_id, systeme_exploitation_id, statut_id, probleme, resolution)
SELECT 
    TRY_CONVERT(DATETIME, t.DateCreation, 103), -- Convertir la date au format DATETIME
    TRY_CONVERT(DATETIME, t.DateResolution, 103), -- Convertir la date au format DATETIME
    p.id, v.id, se.id, s.id, t.Probleme, t.Resolution
FROM TempTickets t
JOIN Produit p ON t.Produit = p.nom
JOIN Version v ON t.Version = v.nom AND v.produit_id = p.id
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
JOIN Statut s ON t.Statut = s.nom;

-- Supprimer la table TempTickets
DROP TABLE TempTickets;
