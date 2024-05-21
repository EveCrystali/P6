-- Insérer les données dans la table Produit et récupérer les IDs
INSERT INTO Produit (nom)
SELECT DISTINCT Produit
FROM #TempTickets
WHERE Produit NOT IN (SELECT nom FROM Produit);

-- Insérer les données dans la table Systeme_exploitation et récupérer les IDs
INSERT INTO Systeme_exploitation (nom)
SELECT DISTINCT SystemeExploitation
FROM #TempTickets
WHERE SystemeExploitation NOT IN (SELECT nom FROM Systeme_exploitation);

-- Insérer les données dans la table Statut et récupérer les IDs
INSERT INTO Statut (nom)
SELECT DISTINCT Statut
FROM #TempTickets
WHERE Statut NOT IN (SELECT nom FROM Statut);

-- Insérer les données dans la table Version et récupérer les IDs
INSERT INTO Version (produit_id, nom)
SELECT DISTINCT p.id, t.Version
FROM #TempTickets t
JOIN Produit p ON t.Produit = p.nom
WHERE (p.id, t.Version) NOT IN (SELECT produit_id, nom FROM Version);

-- Insérer les données dans la table Systeme_exploitation_has_Version
INSERT INTO Systeme_exploitation_has_Version (version_id, systeme_exploitation_id)
SELECT DISTINCT v.id, se.id
FROM #TempTickets t
JOIN Version v ON t.Version = v.nom
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
JOIN Produit p ON t.Produit = p.nom AND v.produit_id = p.id
WHERE (v.id, se.id) NOT IN (SELECT version_id, systeme_exploitation_id FROM Systeme_exploitation_has_Version);

-- Insérer les données dans la table Ticket
INSERT INTO Ticket (date_de_creation, date_de_resolution, produit_id, version_id, systeme_exploitation_id, statut_id, probleme, resolution)
SELECT 
    TRY_CONVERT(DATETIME, t.DateCreation, 103), -- Convertir la date au format DATETIME
    TRY_CONVERT(DATETIME, t.DateResolution, 103), -- Convertir la date au format DATETIME
    p.id, v.id, se.id, s.id, t.Probleme, t.Resolution
FROM #TempTickets t
JOIN Produit p ON t.Produit = p.nom
JOIN Version v ON t.Version = v.nom AND v.produit_id = p.id
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
JOIN Statut s ON t.Statut = s.nom;

-- Supprimer la table temporaire
DROP TABLE #TempTickets;
