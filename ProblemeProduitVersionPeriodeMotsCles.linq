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
    string produitNom = Util.ReadLine("Entrez le nom du produit (obligatoire) : ");
    string versionNom = Util.ReadLine("Entrez la version du produit (optionnel : laissez vide pour ignorer ce filtre) :");
    string periodeDebutStr = Util.ReadLine("Entrez la date de début de période (format : jj/MM/aaaa, optionnel : laissez vide pour ignorer ce filtre) :");
    string periodeFinStr = Util.ReadLine("Entrez la date de fin de période (format : jj/MM/aaaa, optionnel : laissez vide pour ignorer ce filtre) :");
    string motsClesStr = Util.ReadLine("Entrez les mots-clés séparés par des virgules :");

    DateTime? periodeDebut = null;
    DateTime? periodeFin = null;
    int? productId = null;
    List<int> versionIds = new List<int>();
	var motsCles = !string.IsNullOrWhiteSpace(motsClesStr) ? motsClesStr.Split(',').Select(k => k.Trim()).ToList() : new List<string>();

    if (!string.IsNullOrWhiteSpace(produitNom))
    {
	    productId = GetProductId(produitNom);
	    
	    if (!string.IsNullOrWhiteSpace(versionNom))
	    {
	        versionIds = GetVersionIds(productId, versionNom);
	    }

	    periodeDebut = TryParseDate(periodeDebutStr);
        periodeFin = TryParseDate(periodeFinStr);
		
		var debugInfo = $"Les résultats pour la recherche de {produitNom} " +
                    $"{(versionNom.Length > 0 ? $"version: {versionNom}" : "toutes les versions")}, " +
                    $"{(periodeDebut.HasValue ? $"de la date de début: {periodeDebut.Value.ToString("dd/MM/yyyy")}" : "sans date de début")}, " +
                    $"{(periodeFin.HasValue ? $"à la date de fin: {periodeFin.Value.ToString("dd/MM/yyyy")}" : "sans date de fin")}, " +
                    $"{(motsCles.Any() ? $"avec les mots-clés: {string.Join(", ", motsCles)}" : "sans mots-clés")} sont :";
    	Console.WriteLine(debugInfo);

	    var resultats = from t in Tickets
	                    select t;

	    if (productId.HasValue)
	    {
	        resultats = resultats.Where(t => t.Produit_id == productId.Value);
	    }
	    if (!string.IsNullOrWhiteSpace(versionNom) && !versionIds.Any())
	    {
	        // Si une version a été spécifiée mais qu'aucune ID de version n'a été trouvée, retourner une séquence vide
	        resultats = Enumerable.Empty<Ticket>().AsQueryable();
	    }
	    else if (versionIds.Any())
	    {
	        resultats = resultats.Where(t => versionIds.Contains(t.Version_id));
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
	else
	{
		Console.WriteLine("Vous n'avez pas entré de nom pour le produit.");
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

// Méthode pour obtenir les IDs de la version à partir du produitId et du nom de version
List<int> GetVersionIds(int? produitId, string versionNom)
{
    if (produitId.HasValue)
    {
        var resultats = (from v in Versions
                         where v.Nom == versionNom && v.Produit_id == produitId
                         select v.Id).ToList();
        return resultats;
    }
    else
    {
        var resultats = (from v in Versions
                         where v.Nom == versionNom
                         select v.Id).ToList();
        return resultats;
    }
}

DateTime? TryParseDate(string dateStr)
{
    if (DateTime.TryParse(dateStr, out var date))
    {
        return date;
    }
    return null;
}
