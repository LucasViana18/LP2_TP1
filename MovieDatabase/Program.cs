using System;
using System.IO;

namespace MovieDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            Query q = new Query();

            Console.WriteLine("Loading. Please wait...");
            q.LoadRating();
            q.LoadTitle();
            q.LoadEpisode();
            Console.Clear();
            Console.Write("Selecione uma opção: \n 1 - Títulos\n " +
                "2 - Pessoas\n 3 - Sair\n => ");

            p.StartMenu(q);
        }

        private void StartMenu(Query q)
        {
            string response = Console.ReadLine();

            switch (response)
            {
                case "1":
                    AskTitle(q);
                    break;
                case "2":

                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Digite uma das opções disponíveis");
                    break;
            }
        }

        public void AskTitle(Query q)
        {
            string option;

            Console.Write("1 - Pesquisar\n2 - Voltar\n => ");
            option = Console.ReadLine();

            if (option == "1")
            {
                // Call method with all the titles with the certain word
                string s;
                Console.Write("Digite o que quer pesquisar.\n => ");
                s = Console.ReadLine();
                ShowTitles(s,q);
            }
            else if (option == "2")
                // Call method of start menu
                StartMenu(q);
            else
                Console.WriteLine("Digite uma das opções disponíveis");
        }

        public void ShowTitles(string response, Query q)
        {
            q.ProcessTitle(response);

            foreach (Title field in q.FilteredTitles)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine
                    ("(" + field.ID + ") " + field.TitleType + " | " + field.PrimaryTitle + " | " + field.IsAdult + " | " +
                    field.StartYear + " | " + field.EndYear + " | " + field.RuntimeMinutes + " | " +
                    field.Genres + "  - " + field.Tconst + "\n");
            }
        }

        public void AskDetails(Query q)
        {
            string option;
            Console.WriteLine("Selecione uma das opções acima ou [V] para voltar a pesquisar ou [C] para Continuar.");

            option = Console.ReadLine();

            if (option == "v")
                AskTitle(q);
            else
                ShowDetails(option, q);
        }

        public void ShowDetails(string ID, Query q)
        {
            Console.Clear();
            Console.WriteLine("Please check the details:\n");

            // Call function for the details of the selected title
            q.ProcessDetails(ID);

            foreach (Details field in q.FilteredDetails)
            {
                Console.WriteLine("Code: \t" + field.Tconst + "\n");
                Console.WriteLine("Name: \t" + field.PrimaryTitle + "\n");
                Console.WriteLine("Original Name: \t" + field.OriginalTitle + "\n");
                Console.WriteLine("Title Type: \t" + field.TitleType + "\n");
                Console.WriteLine("Genre: \t" + field.Genres + "\n");
                Console.WriteLine("Average Rating: \t" + field.AverageRating + "\n");
                Console.WriteLine("Number of Votes: \t" + field.numVotes + "\n");
                Console.WriteLine("Start Year: \t" + field.StartYear + "\n");
                Console.WriteLine("End Year: \t" + field.EndYear + "\n");
                Console.WriteLine("Duration: \t" + field.RuntimeMinutes + "\n");
                Console.WriteLine("Adults Only: \t" + field.IsAdult + "\n");

                if (field.TitleType == "tvEpisode")
                {
                    Console.WriteLine("From the series: \t" + field.ParentTitle + "\n");
                    Console.WriteLine("Season: \t" + field.SeasonNumber + "\n");
                    Console.WriteLine("Episode: \t" + field.EpisodeNumber + "\n");
                }
                AskGeneric(q);
            }
        }

        public void AskGeneric(Query q)
        {
            string option;

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("\n1 - See the series (if the title selected is an episode)\n");
            Console.WriteLine("2 - See all episodes (if the title selected is a series)\n");
            Console.WriteLine("3 - See all people that got into this title\n");
            Console.Write("4 - Go back to main menu\n --> ");

            option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowParent(q);
                    break;
                case "2":
                    ShowChildren(q);
                    break;
                case "3":
                    //ShowPeopleInTitle();
                    break;
                case "4":
                    StartMenu(q);
                    break;
            }
        }

        public void ShowParent(Query q)
        {
            Console.Clear();

            q.ProcessParent();
            Console.WriteLine("Here is the tv series of that episode. It might take some time.");
            foreach (Details field in q.FilteredParent)
            {
                Console.WriteLine("Code: \t" + field.Tconst + "\n");
                Console.WriteLine("Name: \t" + field.PrimaryTitle + "\n");
                Console.WriteLine("Original Name: \t" + field.OriginalTitle + "\n");
                Console.WriteLine("Title Type: \t" + field.TitleType + "\n");
                Console.WriteLine("Genre: \t" + field.Genres + "\n");
                Console.WriteLine("Average Rating: \t" + field.AverageRating + "\n");
                Console.WriteLine("Number of Votes: \t" + field.numVotes + "\n");
                Console.WriteLine("Start Year: \t" + field.StartYear + "\n");
                Console.WriteLine("End Year: \t" + field.EndYear + "\n");
                Console.WriteLine("Duration: \t" + field.RuntimeMinutes + "\n");
                Console.WriteLine("Adults Only: \t" + field.IsAdult + "\n");
            }
            Console.WriteLine("-----------------------------END------------------------------------\n");
            AskGeneric(q);
        }

        public void ShowChildren(Query q)
        {

        }
    }
}
