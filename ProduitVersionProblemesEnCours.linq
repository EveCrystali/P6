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
    string produitNom = Util.ReadLine("Entrez le nom du produit :");
    string versionNom = Util.ReadLine("Entrez la version du produit :");

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

    if (productId.HasValue && versionId.HasValue)
    {
        // Cas 4 : tous les paramètres sont renseignés
        var resultats = from t in Tickets
                        where t.Produit_id == productId.Value && t.Version_id == versionId.Value && t.Statut_id == statutId.Value
                        select t;
        resultats.Dump();
    }
    else if (productId.HasValue)
    {
        // Cas 3 : seul le paramètre nom du produit est renseigné
        var resultats = from t in Tickets
                        where t.Produit_id == productId.Value && t.Statut_id == statutId.Value
                        select t;
        resultats.Dump();
    }
	else if (versionId.HasValue)
    {
        // Cas 2 : seul le paramètre de la version est renseigné
        var resultats = from t in Tickets
                        where t.Version_id == versionId.Value && t.Statut_id == statutId.Value
                        select t;
        resultats.Dump();
    }
    else if (string.IsNullOrWhiteSpace(produitNom) && string.IsNullOrWhiteSpace(versionNom))
    {
        // Cas 1 : aucun paramètre n'est renseigné
        var resultats = from t in Tickets
                        where t.Statut_id == statutId.Value
                        select t;
        resultats.Dump();
    }
    else
    {
        Console.WriteLine("Produit ou version non trouvés.");
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