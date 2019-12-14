using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase
{
    public class UserInterface
    {
        private Query q;

        public void StartMenu()
        {
            // Local variables
            char response;
            do
            {
                Console.Clear();

                // Ask the user for the search of titles or persons
                Console.Write("Selecione uma opção: \n 1 - Títulos\n " +
                    "2 - Pessoas\n 3 - Sair\n --> ");
                response = Console.ReadLine()[0];

                q = new Query();

                // Option choosing
                switch (response)
                {
                    case '1':
                        SearchFor('T');
                        break;
                    case '2':
                        SearchFor('P');
                        break;
                    case '3':
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Digite uma das opções disponíveis");
                        break;
                }
                q = null;
                GC.Collect();
            } while (response != '3');
        }

        private void SearchFor(char searchType)
        {
            Console.Clear();
            // Local variables
            string option;

            // Ask the user for the title search
            if (searchType == 'T')
            {
                q.LoadFiles("titles");
                Console.Clear();
                Console.Write("Pesquise o título ou digite 'v' para voltar.\n -->");

            }
            else
            {
                q.LoadFiles("names");
                Console.Clear();
                Console.Write("Pesquise o nome da pessoa ou digite 'v' para voltar.\n -->");
            }

            option = Console.ReadLine();

            if (option.ToLower() != "v")
            {
                if (searchType == 'T')
                    // Call method for the list of title results
                    ShowListOfResults(option, true);
                else
                    // Call method for the list of name results
                    ShowListOfResults(option, false);
            }
        }

        private void ShowListOfResults(string response, bool isTitle)
        {
            // Local variable
            string option = "";
            string userChoice;
            byte i = 0;

            Console.Clear();

            if (isTitle) // Results for titles
            {
                // Call the function to filter the titles
                q.ProcessListOfResults(response, true);

                Console.WriteLine("--------------------------CATEGORIES----" +
                    "----------------------------");
                Console.WriteLine("ID | Type | Primary Title | For adults? | " +
                        "Start Year | End Year | Minutes | Genre\n");

                // Loop that goes through the list of titles that contains the word/s that the user typed
                foreach (Title field in q.FilteredTitles)
                {
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine
                        ("(" + field.ID + ") " + field.TitleType + " | " + field.PrimaryTitle + " | " + field.IsAdult + " | " +
                        field.StartYear + " | " + field.EndYear + " | " + field.RuntimeMinutes + " | " +
                        field.Genres + "  - " + field.Tconst + "\n");
                    i++;
                    if (i > 19)
                    {
                        option = AskForID(true);
                        if (option != "c")
                        {
                            break;
                        }
                        else
                        {
                            i = 0;
                            option = "";
                        }

                    }
                }
                if (option == "")
                {
                    Console.WriteLine("-----------------------------END------------------------------------");

                    option = AskForID(false);
                }
                if (option != "v")
                {
                    ShowDetails(option, true);
                    userChoice = AskGeneric(true);
                    switch (userChoice)
                    {
                        case "1":
                            ShowParent();
                            break;
                        case "2":
                            ShowChildren();
                            break;
                        case "3":
                            ShowPeopleInTitle();
                            break;
                    }
                }
            }
            else // Results for people
            {
                q.ProcessListOfResults(response, false);

                Console.WriteLine("--------------------------CATEGORIES----" +
                    "----------------------------");
                Console.WriteLine("ID | Primary Name | Primary Professions | IMDB ID\n");

                // Loop that goes through the list of titles that contains the word/s that the user typed
                foreach (Person field in q.FilteredNames)
                {

                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine
                        ("(" + field.ID + ") " + field.PrimaryName + " - " + field.PrimaryProfession + " - " +
                        field.Nconst + "\n");
                    i++;
                    if (i > 19)
                    {
                        option = AskForID(true);
                        if (option != "c")
                        {
                            break;
                        }
                        else
                        {
                            i = 0;
                            option = "";
                        }
                    }
                }
                if (option == "")
                {
                    Console.WriteLine("-----------------------------END------------------------------------");

                    option = AskForID(false);
                }
                if (option != "v")
                {
                    ShowDetails(option, false);
                    userChoice = AskGeneric(false);
                    if (userChoice == "1")
                        ShowTitlesWithPerson();
                }
            }
        }

        private string AskForID(bool nextPage)
        {
            // Local variables
            string option;
            string extraOption;

            // Ask user the title ID for further detail
            if (nextPage)
            {
                Console.WriteLine("Selecione uma das opções acima ou [V] para voltar para menu principal ou [C] para Continuar.");
                extraOption = "c";

            }
            else
            {
                Console.WriteLine("Selecione uma das opções acima ou [V] para voltar para menu principal.");
                extraOption = "v";
            }
            do
            {
                option = Console.ReadLine();
                option = option.ToLower();
            } while (option != "v" && option != extraOption && !int.TryParse(option, out int _));

            return option;
        }

        private void ShowDetails(string ID, bool isTitle)
        {
            Console.Clear();
            Console.WriteLine("Please check the details:\n");

            if (isTitle)
            {
                q.LoadFiles("ratings");
                q.LoadFiles("episodes");
                // Call function for the details of the selected title
                q.ProcessDetails(ID, true);

                // Goes through the list of details of the selected title
                foreach (Details field in q.FilteredDetails)
                {
                    q.CurrentTitleID = field.Tconst;
                    q.CurrentTitleName = field.PrimaryTitle;
                    Console.WriteLine("Code: \t" + field.Tconst + "\n");
                    Console.WriteLine("Name: \t" + field.PrimaryTitle + "\n");
                    Console.WriteLine("Original Name: \t" + field.OriginalTitle + "\n");
                    Console.WriteLine("Title Type: \t" + field.TitleType + "\n");
                    Console.WriteLine("Genre: \t" + field.Genres + "\n");
                    Console.WriteLine("Average Rating: \t" + field.AverageRating + "\n");
                    Console.WriteLine("Number of Votes: \t" + field.NumVotes + "\n");
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
                }
            }
            else
            {
                // Call function for the details of the selected name
                q.ProcessDetails(ID, false);

                // Goes through the list of details of the selected name
                foreach (Person field in q.FilteredNameDetails)
                {
                    Console.WriteLine("Code: \t" + field.Nconst + "\n");
                    Console.WriteLine("Name: \t" + field.PrimaryName + "\n");
                    Console.WriteLine("Birth Year: \t" + field.BirthYear + "\n");
                    Console.WriteLine("Death Year: \t" + field.DeathYear + "\n");
                    Console.WriteLine("Primary professions: \t" + field.PrimaryProfession + "\n");
                }

            }
            Console.WriteLine("-----------------------------END------------------------------------\n");
        }

        private string AskGeneric(bool isTitle)
        {
            // Local variables
            string option = "";

            if (isTitle)
            {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("\n1 - See the series (if the title selected is an episode)\n");
                Console.WriteLine("2 - See all episodes (if the title selected is a series)\n");
                Console.WriteLine("3 - See all people that got into this title\n");
                Console.Write("4 - Go back to main menu\n --> ");

                do
                {
                    option = Console.ReadLine();
                } while (option != "1" && option != "2" && option != "3" && option != "4");

            }
            else
            {
                Console.WriteLine("1 - See the titles that the person is part of\n");
                Console.Write("2 - Go back to main menu\n --> ");
                do
                {
                    option = Console.ReadLine();
                } while (option != "1" && option != "2");

            }
            return option;
        }

        private void ShowParent()
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
                Console.WriteLine("Number of Votes: \t" + field.NumVotes + "\n");
                Console.WriteLine("Start Year: \t" + field.StartYear + "\n");
                Console.WriteLine("End Year: \t" + field.EndYear + "\n");
                Console.WriteLine("Duration: \t" + field.RuntimeMinutes + "\n");
                Console.WriteLine("Adults Only: \t" + field.IsAdult + "\n");
            }
            Console.WriteLine("-----------------------------END------------------------------------\n");
            AskGeneric(true);
        }

        private void ShowChildren()
        {
            // Local variable
            byte i = 0;
            string option = "";

            Console.Clear();

            // Call the function to filter the titles
            q.ProcessEpisodes();

            Console.WriteLine("Here are the episodes of the series.\n");
            Console.WriteLine("--------------------------CATEGORIES--------------------------------");
            Console.WriteLine("Type | Primary Title | For adults? | " +
                    "Start Year | End Year | Minutes | Genre\n");
            // Loop that goes through the list of titles that contains the word/s that the user typed
            foreach (Title field in q.FilteredEpisodes)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine
                    ("(" + field.ID + ") " + field.TitleType + "| " + field.PrimaryTitle + " | " + field.IsAdult + " | " +
                    field.StartYear + " | " + field.EndYear + " | " + field.RuntimeMinutes + " |" +
                    field.Genres + "  - " + " - " + field.Tconst + "\n");
                i++;
                if (i > 20)
                {
                    option = AskForID(true);
                    if (option != "c")
                    {
                        break;
                    }
                    else
                    {
                        i = 0;
                        option = "";
                    }
                }
            }
            if (option == "")
            {
                Console.WriteLine("-----------------------------END------------------------------------");

                option = AskForID(false);
            }
            if (option != "v")
                ShowDetails(option, true);
        }

        private void ShowTitlesWithPerson()
        {
            byte i = 0;
            string option = "";

            q.ReleaseFiles();
            q.LoadFiles("titles");

            Console.Clear();

            q.ProcessTitlesWithPerson();

            foreach (Title field in q.FilteredTitlesWithPerson)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine
                    ("(" + field.ID + ") " + field.TitleType + " | " + field.PrimaryTitle + " | " + field.IsAdult + " | " +
                    field.StartYear + " | " + field.EndYear + " | " + field.RuntimeMinutes + " | " +
                    field.Genres + "  - " + field.Tconst + "\n");
                i++;
                if (i > 20)
                {
                    option = AskForID(true);
                    if (option != "c")
                    {
                        break;
                    }
                    else
                    {
                        i = 0;
                        option = "";
                    }
                }
            }
            if (option == "")
            {
                Console.WriteLine("-----------------------------END------------------------------------");

                option = AskForID(false);
            }
            if (option != "v")
                ShowDetails(option, true);
        }

        private void ShowPeopleInTitle()
        {
            byte i = 0;
            string option = "";

            q.ReleaseFiles();
            q.LoadFiles("names");

            Console.Clear();

            q.ProcessPeopleInTitle();
            Console.WriteLine("Check below the list of people of " + q.CurrentTitleName);

            foreach (Person field in q.FilteredPeopleInTitle)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine
                    ("(" + field.ID + ") " + field.PrimaryName + " - " +
                    field.PrimaryProfession + " - " + field.Nconst + "\n");
                i++;
                if (i > 20)
                {
                    option = AskForID(true);
                    if (option != "c")
                    {
                        break;
                    }
                    else
                    {
                        i = 0;
                        option = "";
                    }
                }
            }
            if (option == "")
            {
                Console.WriteLine("-----------------------------END------------------------------------");

                option = AskForID(false);
            }
            if (option == "v")
                ShowDetails(option, false);
        }
    }
}

