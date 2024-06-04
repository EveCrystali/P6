# Projet P6

Lien du repository : [https://github.com/EveCrystali/P6.git](https://github.com/EveCrystali/P6.git)

## Répertoire Racine

- **Fichier de sauvegarde de la base de données**: `nexaworks_db`
- **Modèle entité-association**: `modele_EA.pdf`

## Structure des Répertoires

### /Remplissage_BDD

- **/ODTTicketToCSV**: Éléments du programme permettant de transformer le fichier texte contenant les tickets en fichier .csv organisé
- **/Scripts_SQL**: Ensemble des scripts permettant de créer les tables de la base de données et d'importer les données du fichier .csv précédemment créé

### /Requetes_LINQPad

- Ensemble des requêtes LINQPad (`.linq`) :
**Exécutez les requêtes LINQPad dans `Requetes_LINQPad`** depuis LINQPad pour récupérer et analyser les données de la base de données.

## Description des Fichiers

### Fichiers Racine

- `nexaworks_db`: Fichier de sauvegarde de la base de données.
- `modele_EA.pdf`: Diagramme du modèle entité-association.

### /Remplissage_BDD/ODTTicketToCSV

- Contient les fichiers du programme pour convertir le fichier texte contenant les tickets en fichier .csv organisé.

### /Remplissage_BDD/Scripts_SQL

- Scripts SQL pour créer les tables de la base de données et importer les données du fichier .csv.

### /Requetes_LINQPad

- `ProblemeProduitVersionPeriodeMotsCles.linq`: Requête LINQPad pour récupérer les problèmes selon les critères spécifiés : Nom du produit (obligatoire), Version (optionnel), Période début (optionnel), Période fin (optionnel), Mots clés (optionnel).
- `ProduitVersionProblemesEnCoursPeriodeMotsCles.linq`: Requête LINQPad pour récupérer les problèmes en cours selon les critères spécifiés : Nom du produit (optionnel), Version (optionnel), Période début (optionnel), Période fin (optionnel), Mots clés (optionnel).
- `ProduitVersionProblemesResoluPeriodeMotsCles.linq`: Requête LINQPad pour récupérer les problèmes résolus selon les critères spécifiés : Nom du produit (optionnel), Version (optionnel), Période début (optionnel), Période fin (optionnel), Mots clés (optionnel).

## Utilisation sans le fichier de sauvegarde de la base de données :

Si vous n'utilisez pas le fichier de sauvegarde de la base données fourni et que vous souhaitez importer manuellement d'autres tickets depuis un fichier .txt, vous pouvez suivre ces étapes pour importer les éléments correctement dans votre base de données :  

1. **Exécutez le programme dans `ODTTicketToCSV`** depuis VS Code pour convertir le fichier texte (il doit respecter la structure du fichier initial) contenant les tickets en fichier .csv.
   
2. **Utilisez les scripts dans `Scripts_SQL`** pour créer les tables de la base de données et importer les données du fichier .csv. Lancer dans l'ordre : `SQLQueryImportCSVTicket.sql` puis `SQLCreateTablesWithKeys.sql` puis `ImportAfterBulk.sql` et enfin `ImportProduitVersionSystemeExploitation.sql` pour importer toutes les combinaisons de Produit-Version-SystemeExploitation existantes.
 
