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
INSERT INTO Version (nom)
SELECT DISTINCT t.Version
FROM TempTickets t
WHERE NOT EXISTS (
    SELECT 1 
    FROM Version v 
    WHERE v.nom = t.Version
);

-- Insérer les données dans la table ProduitVersionSysteme
INSERT INTO ProduitVersionSysteme (produit_id, version_id, systeme_exploitation_id)
SELECT DISTINCT p.id, v.id, se.id
FROM TempTickets t
JOIN Produit p ON t.Produit = p.nom
JOIN Version v ON t.Version = v.nom
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
WHERE NOT EXISTS (
    SELECT 1 
    FROM ProduitVersionSysteme pvs
    WHERE pvs.produit_id = p.id 
      AND pvs.version_id = v.id 
      AND pvs.systeme_exploitation_id = se.id
);

-- Insérer les données dans la table Problèmes
INSERT INTO Probleme (date_de_creation, date_de_resolution, produit_version_systeme_id, statut_id, probleme, resolution)
SELECT 
    TRY_CONVERT(DATETIME, t.DateCreation, 103), -- Convertir la date au format DATETIME
    TRY_CONVERT(DATETIME, t.DateResolution, 103), -- Convertir la date au format DATETIME
    pvs.id, s.id, t.Probleme, t.Resolution
FROM TempTickets t
JOIN Produit p ON t.Produit = p.nom
JOIN Version v ON t.Version = v.nom
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
JOIN ProduitVersionSysteme pvs ON pvs.produit_id = p.id 
                              AND pvs.version_id = v.id 
                              AND pvs.systeme_exploitation_id = se.id
JOIN Statut s ON t.Statut = s.nom;

-- Supprimer la table TempTickets
DROP TABLE TempTickets;
