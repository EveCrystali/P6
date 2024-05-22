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
    // Obtenir l'ID du produit correspondant au nom donné
    int? productId = GetProductId(produitNom);
	int? statutId = GetStatusId("En cours");
	int? versionId = GetVersionId(versionNom);

    // Si l'ID du produit est trouvé, effectuer la requête sur les tickets
    if (productId.HasValue)
    {
        var resultats = from t in Tickets
                        where t.Produit_id == productId.Value && t.Statut_id == statutId.Value
                        select t;

        // Afficher les résultats
        resultats.Dump();
    }
    else
    {
        Console.WriteLine("Produit non trouvé.");
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

int? GetStatusId(string statutNom)
{
    var resultat = (from s in Statuts
                    where s.Nom == statutNom
                    select s.Id).FirstOrDefault();
    return resultat;
}

int? GetVersionId(string versionNom)
{
    var resultat = (from v in Versions
                    where v.Nom == versionNom
                    select v.Id).FirstOrDefault();
    return resultat;
}