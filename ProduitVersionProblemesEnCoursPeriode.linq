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

    DateTime periodeDebut;
    DateTime periodeFin;

    int? productId = null;
    int? versionId = null;
    int? statutId = GetStatusId("En cours");

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

    if (!resultats.Any())
    {
        Console.WriteLine("Aucun résultat trouvé.");
    }
    else
    {
        resultats.Dump();
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
