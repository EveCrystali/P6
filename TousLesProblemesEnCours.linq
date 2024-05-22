<Query Kind="Statements">
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

// Sélectionner tous les tickets avec le statut "En cours"
var ticketsResolu = from t in Tickets
                    where t.Statut.Nom == "En cours"
                    select t;

// Afficher les résultats
ticketsResolu.Dump();
