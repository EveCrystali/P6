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
	
	DateTime? periodeDebut = null;
    DateTime? periodeFin = null;
    int? productId = null;
    int? versionId = null;
	var motsCles = !string.IsNullOrWhiteSpace(motsClesStr) ? motsClesStr.Split(',').Select(k => k.Trim()).ToList() : new List<string>();
    int? statutId = GetStatusId("En cours");

    if (!string.IsNullOrWhiteSpace(produitNom))
    {
        productId = GetProductId(produitNom);
    }
	
    if (!string.IsNullOrWhiteSpace(versionNom))
    {
        versionId = GetVersionId(versionNom);
    }

    periodeDebut = TryParseDate(periodeDebutStr);
    periodeFin = TryParseDate(periodeFinStr);

	var debugInfo = $"Les résultats pour la recherche de tickets \"En cours\" de {(productId.HasValue ? $"produit: {produitNom}" : "tous les produits")}, " +
                    $"{(versionId.HasValue ? $"version: {versionNom}" : "toutes les versions")}, " +
                    $"{(periodeDebut.HasValue ? $"de la date de début: {periodeDebut.Value.ToString("dd/MM/yyyy")}" : "sans date de début")}, " +
                    $"{(periodeFin.HasValue ? $"à la date de fin: {periodeFin.Value.ToString("dd/MM/yyyy")}" : "sans date de fin")}, " +
                    $"{(motsCles.Any() ? $"avec les mots-clés: {string.Join(", ", motsCles)}" : "sans mots-clés")} sont :";
    Console.WriteLine(debugInfo);
	
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
    if (periodeDebut.HasValue && periodeFin.HasValue)
    {
        resultats = resultats.Where(t => t.Date_de_creation >= periodeDebut && t.Date_de_creation <= periodeFin);
    }
    else if (periodeDebut.HasValue)
    {
        resultats = resultats.Where(t => t.Date_de_creation >= periodeDebut);
    }
    else if (periodeFin.HasValue)
    {
        resultats = resultats.Where(t => t.Date_de_creation <= periodeFin);
    }

    var resultatsListe = resultats.ToList();

    if (motsCles.Any())
    {
        resultatsListe = resultatsListe.Where(t => motsCles.Any(k => t.Probleme.Contains(k))).ToList();	
    }

    var resultatsFinaux = from r in resultatsListe
                          join p in Produits on r.Produit_id equals p.Id
                          join v in Versions on r.Version_id equals v.Id
                          join o in Systeme_exploitations on r.Systeme_exploitation_id equals o.Id
                          join s in Statuts on r.Statut_id equals s.Id
                          select new
                          {
                              r.Id,
                              r.Date_de_creation,
                              Produit = p.Nom,
                              Version = v.Nom,
                              Systeme_exploitation = o.Nom,
                              Statut = s.Nom,
                              Probleme = r.Probleme,
                              r.Date_de_resolution,
                              r.Resolution
                          };

    if (!resultatsFinaux.Any())
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

DateTime? TryParseDate(string dateStr)
{
    if (DateTime.TryParse(dateStr, out var date))
    {
        return date;
    }
    return null;
}
