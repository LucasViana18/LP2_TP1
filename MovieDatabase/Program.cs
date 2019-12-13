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

            q.LoadRating();
            q.LoadTitle();
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
                //Console.Write("Digite o que quer pesquisar.\n => ");
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

    }
}
