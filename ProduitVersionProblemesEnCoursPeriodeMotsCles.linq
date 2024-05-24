<Query Kind="Program">
  <Connection>
    <ID>26bcb8e7-783c-445b-aad9-6fd561ac681a</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>nexaworks_db</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
    string produitNom = Util.ReadLine("Entrez le nom du produit (optionnel : laissez vide pour ignorer ce filtre) :");
    string versionNom = Util.ReadLine("Entrez la version du produit (optionnel : laissez vide pour ignorer ce filtre) :");
    string periodeDebutStr = Util.ReadLine("Entrez la date de début de période (format : jj/MM/aaaa, optionnel : laissez vide pour ignorer ce filtre) :");
    string periodeFinStr = Util.ReadLine("Entrez la date de fin de période (format : jj/MM/aaaa, optionnel : laissez vide pour ignorer ce filtre) :");
    string motsClesStr = Util.ReadLine("Entrez les mots-clés séparés par des virgules :");

    DateTime periodeDebut;
    DateTime periodeFin;

    int? productId = null;
    int? versionId = null;
    int? statutId = GetStatusId("En cours");
    var motsCles = motsClesStr.Split(',').Select(k => k.Trim()).ToList();

    if (!string.IsNullOrWhiteSpace(produitNom))
    {
        productId = GetProductId(produitNom);
    }
    if (!string.IsNullOrWhiteSpace(versionNom))
    {
        versionId = GetVersionId(versionNom);
    }

    bool periodeDebutValide = DateTime.TryParse(periodeDebutStr, out periodeDebut);
    bool periodeFinValide = DateTime.TryParse(periodeFinStr, out periodeFin);

    var resultats = from t in Tickets
                    where t.Statut_id == statutId.Value
                    select t;

    if (productId.HasValue)
    {
        resultats = resultats.Where(t => t.Produit_id == productId.Value);
    }
    if (versionId.HasValue)
    {
        resultats = resultats.Where(t => t.Version_id == versionId.Value);
    }
    if (periodeDebutValide && periodeFinValide)
    {
        resultats = resultats.Where(t => t.Date_de_creation >= periodeDebut && t.Date_de_creation <= periodeFin);
    }
    else if (periodeDebutValide)
    {
        resultats = resultats.Where(t => t.Date_de_creation >= periodeDebut);
    }
    else if (periodeFinValide)
    {
        resultats = resultats.Where(t => t.Date_de_creation <= periodeFin);
    }

    var resultatsListe = resultats.ToList();

    if (motsCles.Any())
    {
        resultatsListe = resultatsListe.Where(t => motsCles.Any(k => t.Probleme.Contains(k))).ToList();
    }

	var resultatsFinaux = from r in resultats
					join p in Produits on r.Produit_id equals p.Id
                    join v in Versions on r.Version_id equals v.Id
					join o in Systeme_exploitations on r.Version_id equals o.Id
                    join s in Statuts on r.Statut_id equals s.Id
					select new { 
									r.Id, 
									r.Date_de_creation, 
									Produit = p.Nom, 
									Version = v.Nom, 
									Systeme_exploitation = o.Nom, 
									Statut = s.Nom, 
									Probleme = r.Probleme, 
									r.Date_de_resolution, 
									r.Resolution};
									
	if (!resultatsListe.Any())
    {
        Console.WriteLine("Aucun résultat trouvé.");
    }
    else
    {
        resultatsFinaux.Dump();
    }
}

// Méthode pour obtenir l'ID du produit à partir de son nom
int? GetProductId(string produitNom)
{
    var resultat = (from p in Produits
                    where p.Nom == produitNom
                    select p.Id).FirstOrDefault();
    return resultat;
}

// Méthode pour obtenir l'ID du statut à partir de son nom
int? GetStatusId(string statutNom)
{
    var resultat = (from s in Statuts
                    where s.Nom == statutNom
                    select s.Id).FirstOrDefault();
    return resultat;
}

// Méthode pour obtenir l'ID de la version à partir de son nom
int? GetVersionId(string versionNom)
{
    var resultat = (from v in Versions
                    where v.Nom == versionNom
                    select v.Id).FirstOrDefault();
    return resultat;
}
