using System;
using System.IO;

namespace MovieDatabase
{
    class Program
    {
        private const string folderName = "MyIMDBSearcher";
        private const string fileTitleBasics = "title.basics.tsv";
        private const string fileTitleRatings = "title.ratings.tsv";
        private const string fileTitleEpisodes = "title.episode.tsv";
        private const string fileNameBasics = "name.basics.tsv";

        static void Main(string[] args)
        {
            string folderPath = Path.Combine(
            Environment.GetFolderPath
            (Environment.SpecialFolder.LocalApplicationData), folderName);

            string fileTitleBasicsPath = 
                Path.Combine(folderPath, fileTitleBasics);
            string fileTitleRatingsPath = 
                Path.Combine(folderPath, fileTitleRatings);
            string fileTitleEpisodesPath = 
                Path.Combine(folderPath, fileTitleEpisodes);
            string fileNameBasicsPath = 
                Path.Combine(folderPath, fileNameBasics);

            Console.WriteLine("Selecione uma opção: \n 1 - Títulos\n " +
                "2 - Pessoas\n 3 - Sair");

            string response = Console.ReadLine();
            bool a = true;

            while (a)
            {
                switch (response)
                {
                    case "1":
                        a = false;
                        // Call method from Title class
                        break;
                    case "2":
                        a = false;
                        // Call method from Person class
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Digite uma das opções disponíveis");
                        break;
                }
            }
        }
    }
}
