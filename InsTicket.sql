-- Insérer les données dans la table Ticket
INSERT INTO Ticket (date_de_creation, date_de_resolution, produit_id, version_id, systeme_exploitation_id, statut_id, probleme, resolution)
SELECT 
    TRY_CONVERT(DATETIME, t.DateCreation, 103), -- Convertir la date au format DATETIME
    TRY_CONVERT(DATETIME, t.DateResolution, 103), -- Convertir la date au format DATETIME
    p.id, v.id, se.id, s.id, t.Probleme, t.Resolution
FROM TempTickets t
JOIN Produit p ON t.Produit = p.nom
JOIN Version v ON t.Version = v.nom
JOIN Systeme_exploitation se ON t.SystemeExploitation = se.nom
JOIN Statut s ON t.Statut = s.nom;
