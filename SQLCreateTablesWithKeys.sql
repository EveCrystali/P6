CREATE TABLE Produit (
    id INT PRIMARY KEY IDENTITY(1,1),
    nom NVARCHAR(50) NOT NULL
);

CREATE TABLE Version (
    id INT PRIMARY KEY IDENTITY(1,1),
    produit_id INT NOT NULL,
    nom NVARCHAR(50) NOT NULL,
    FOREIGN KEY (produit_id) REFERENCES Produit(id)
);

CREATE TABLE Systeme_exploitation (
    id INT PRIMARY KEY IDENTITY(1,1),
    nom NVARCHAR(50) NOT NULL
);

CREATE TABLE Systeme_exploitation_has_Version (
    version_id INT NOT NULL,
    systeme_exploitation_id INT NOT NULL,
    PRIMARY KEY (version_id, systeme_exploitation_id),
    FOREIGN KEY (version_id) REFERENCES Version(id),
    FOREIGN KEY (systeme_exploitation_id) REFERENCES Systeme_exploitation(id)
);

CREATE TABLE Statut (
    id INT PRIMARY KEY IDENTITY(1,1),
    nom NVARCHAR(50) NOT NULL
);

CREATE TABLE Ticket (
    id INT PRIMARY KEY IDENTITY(1,1),
    date_de_creation DATETIME NOT NULL,
    date_de_resolution DATETIME,
    produit_id INT NOT NULL,
    version_id INT NOT NULL,
    systeme_exploitation_id INT NOT NULL,
    statut_id INT NOT NULL,
    probleme TEXT NOT NULL,
    resolution TEXT,
    FOREIGN KEY (produit_id) REFERENCES Produit(id),
    FOREIGN KEY (version_id) REFERENCES Version(id),
    FOREIGN KEY (systeme_exploitation_id) REFERENCES Systeme_exploitation(id),
    FOREIGN KEY (statut_id) REFERENCES Statut(id)
);
