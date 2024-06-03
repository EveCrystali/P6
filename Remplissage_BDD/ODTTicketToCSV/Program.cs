using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TicketToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Veuillez entrer le chemin de votre fichier d'entrée (.txt) :");
            string inputFilePath = Console.ReadLine(); // Chemin de votre fichier d'entrée

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("Le fichier d'entrée spécifié n'existe pas.");
                return;
            }

            Console.WriteLine("Veuillez entrer le chemin de votre fichier de sortie CSV :");
            string outputFilePath = Console.ReadLine(); // Chemin de votre fichier de sortie CSV

            try
            {
                string fileContent = File.ReadAllText(inputFilePath);
                List<Ticket> tickets = ReadTicketsFromContent(fileContent);
                WriteTicketsToCsv(tickets, outputFilePath);
                Console.WriteLine("Les tickets ont été convertis et enregistrés dans le fichier CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static List<Ticket> ReadTicketsFromContent(string content)
        {
            List<Ticket> tickets = new List<Ticket>();
            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Ticket currentTicket = null;
            List<string> cleanedLines = new List<string>();

            foreach (var line in lines)
            {
                string cleanedLine = PreprocessCsvLine(line);
                cleanedLines.Add(cleanedLine);
            }

            foreach (string line in cleanedLines)
            {
                if (Regex.IsMatch(line, @"^Produit :"))
                {
                    if (currentTicket != null)
                    {
                        tickets.Add(currentTicket);
                    }
                    currentTicket = new Ticket();
                    string produit = line.Substring("Produit :".Length).Trim();
                    currentTicket.Produit = string.IsNullOrWhiteSpace(produit) ? null : produit;
                }
                else if (Regex.IsMatch(line, @"^Version :"))
                {
                    string version = line.Substring("Version :".Length).Trim();
                    currentTicket.Version = string.IsNullOrWhiteSpace(version) ? null : version;
                }
                else if (Regex.IsMatch(line, @"^Système d'exploitation :"))
                {
                    string systemeExploitation = line.Substring("Système d’exploitation :".Length).Trim();
                    currentTicket.SystemeExploitation = string.IsNullOrWhiteSpace(systemeExploitation) ? null : systemeExploitation;
                }
                else if (Regex.IsMatch(line, @"^Date de création :"))
                {
                    string dateCreation = line.Substring("Date de création :".Length).Trim();
                    currentTicket.DateCreation = string.IsNullOrWhiteSpace(dateCreation) ? null : ParseDate(dateCreation);
                }
                else if (Regex.IsMatch(line, @"^Date de résolution :"))
                {
                    string dateResolution = line.Substring("Date de résolution :".Length).Trim();
                    currentTicket.DateResolution = string.IsNullOrWhiteSpace(dateResolution) ? null : ParseDate(dateResolution);
                }
                else if (Regex.IsMatch(line, @"^Statut \(résolu\/en cours\) :"))
                {
                    string statut = line.Substring("Statut (résolu/en cours) :".Length).Trim();
                    currentTicket.Statut = string.IsNullOrWhiteSpace(statut) ? null : statut;
                }
                else if (Regex.IsMatch(line, @"^Problème :"))
                {
                    string probleme = line.Substring("Problème :".Length).Trim();
                    currentTicket.Probleme = string.IsNullOrWhiteSpace(probleme) ? null : probleme;
                }
                else if (Regex.IsMatch(line, @"^Résolution :"))
                {
                    string resolution = line.Substring("Résolution :".Length).Trim();
                    currentTicket.Resolution = string.IsNullOrWhiteSpace(resolution) ? null : resolution;
                }
            }

            if (currentTicket != null)
            {
                tickets.Add(currentTicket);
            }

            return tickets;
        }

        static void WriteTicketsToCsv(List<Ticket> tickets, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Produit;Version;SystemeExploitation;DateCreation;DateResolution;Statut;Probleme;Resolution");

                foreach (Ticket ticket in tickets)
                {
                    writer.WriteLine($"{ticket.Produit};{ticket.Version};{ticket.SystemeExploitation};{ticket.DateCreation};{ticket.DateResolution};{ticket.Statut};\"{ticket.Probleme}\";\"{ticket.Resolution}\"");
                }
            }
        }

        static string PreprocessCsvLine(string line)
        {
            line = line.Replace("\"", "\"\"");
            line = line.Replace("\n", " ").Replace("\r", " ");
            line = line.Replace("’", "'");  
            line = line.Replace("‘", "'");  
            line = line.Replace("`", "'");  
            line = line.Replace("\u00A0", " ");
            line = Regex.Replace(line, @"\s+", " ").Trim();
            return line;
        }

        static DateTime ParseDate(string date)
        {
            Dictionary<string, string> month = new Dictionary<string, string>
            {
                { "janvier", "01" },
                { "février", "02" },
                { "mars", "03" },
                { "avril", "04" },
                { "mai", "05" },
                { "juin", "06" },
                { "juillet", "07" },
                { "août", "08" },
                { "septembre", "09" },
                { "octobre", "10" },
                { "novembre", "11" },
                { "decembre", "12" }
            };

            string pattern = @"^(?<day>\d{1,2})\s+(?<month>\D+)\s+(?<year>\d{4})$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Match match = regex.Match(date);
            if (match.Success)
            {
                // Extraire les groupes de capture
                string day = match.Groups["day"].Value.PadLeft(2, '0');
                string monthName = match.Groups["month"].Value.Trim().ToLower();
                string year = match.Groups["year"].Value;

                // Rechercher le mois dans le dictionnaire
                if (month.TryGetValue(monthName, out string monthNumber))
                {
                    // Assembler la date au format YYYY-MM-DD
                    string formattedDate = $"{year}-{monthNumber}-{day}";
                    return DateTime.Parse(formattedDate);
                }
                else
                {
                    Console.WriteLine("Mois non valide." + monthName);
                    return DateTime.Parse("0000-00-00");
                }

            }
            else
            {
                Console.WriteLine("Format de date non valide.");
                return DateTime.Parse("0000-00-00");
            }

        }
    }

    class Ticket
    {
        public string Produit { get; set; }
        public string Version { get; set; }
        public string SystemeExploitation { get; set; }
        public DateTime? DateCreation { get; set; }
        public DateTime? DateResolution { get; set; }
        public string Statut { get; set; }
        public string Probleme { get; set; }
        public string Resolution { get; set; }
    }
}