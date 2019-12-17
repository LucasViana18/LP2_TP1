using System;

namespace MovieDatabase
{
    /// <summary>
    /// Displays to the user the information that he/she needs
    /// </summary>
    public class UserInterface
    {
        // Instance variables
        private string sectionHeader;

        // Interface Properties
        public string ProgramName { get; set; }
        public string ProgramHeader1 { get; set; }
        public string ProgramHeader2 { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserInterface()
        {
            ProgramName = "IMDB Movie Database";
            sectionHeader = "Main Menu";
            ProgramHeader1 = "This program provide you further information on thousands of movies.";
            ProgramHeader2 = "Data souce: Public database available on IMDb website (https://www.imdb.com/interfaces/)";
        }

        /// <summary>
        /// The main header of the interface
        /// </summary>
        public void Header()
        {
            string localHeader = ProgramName + " - " + sectionHeader;
            Console.Clear();
            Console.WriteLine(localHeader);
            Console.WriteLine(RepeatChar('=', localHeader.Length));
            Console.WriteLine(ProgramHeader1);
            Console.WriteLine(ProgramHeader2);
            Console.WriteLine(RepeatChar('-', 60));
        }

        /// <summary>
        /// Display the waiting message
        /// </summary>
        public void Waiting()
        {
            Header();
            Console.WriteLine("Please wait...");
        }

        /// <summary>
        /// Auxiliar method that creates a line
        /// </summary>
        /// <param name="character">The char</param>
        /// <param name="number">The number of characters</param>
        /// <returns>The line</returns>
        private string RepeatChar(char character, int number)
        {
            string result = "";
            for (int i = 0; i < number; i++)
                result += character;
            return result;
        }

        /// <summary>
        /// User input along with restrictions that may lead to error
        /// </summary>
        /// <param name="validLetters">Valid letters</param>
        /// <param name="allowNumbers">Case allow numbers or not</param>
        /// <param name="minNumber">Minimum number that can type</param>
        /// <param name="maxNumber">Maximum number that can type</param>
        /// <returns>The response of the user</returns>
        private string GetUserChoice(string validLetters = "", bool allowNumbers = false, Int16 minNumber = 0, Int16 maxNumber = 999)
        {
            ConsoleKeyInfo response;
            string result = "";
            do
            {
                response = Console.ReadKey(true);
                if (Char.IsLetterOrDigit(response.KeyChar))
                {
                    if (Char.IsDigit(response.KeyChar) && allowNumbers)
                    {
                        if (Int16.TryParse(result + response.KeyChar, out Int16 numberResponse))
                        {
                            if ((numberResponse >= minNumber || numberResponse.ToString().Length < minNumber.ToString().Length)
                                && numberResponse <= maxNumber)
                            {
                                result += response.KeyChar;
                                Console.Write(response.KeyChar);
                            }
                        }
                    }
                    else
                    {
                        if (validLetters == "" || validLetters.ToLower().Contains(Char.ToLower(response.KeyChar)))
                        {
                            result += Char.ToLower(response.KeyChar);
                            Console.Write(response.KeyChar);
                            break;
                        }
                    }
                }
                else
                {
                    if (response.Key == ConsoleKey.Backspace)
                    {
                        if (result.Length > 0)
                        {
                            result = result.Substring(0, result.Length - 1);
                            Console.Write("\b \b");
                        }
                    }
                    else if (response.Key == ConsoleKey.Enter && result != "")
                    {
                        Console.Write(response.KeyChar);
                        break;
                    }
                }

            } while (true);

            return result;
        }

        /// <summary>
        /// Displays the start menu
        /// </summary>
        /// <returns>User choice</returns>
        public string StartMenu()
        {
            // Draw Header
            sectionHeader = "Main Menu";
            Header();

            // Ask the user for the search of titles or persons
            Console.Write("Please choose your Search Criteria: \n 1 - Titles\n " +
                    "2 - Persons/Crew\n 3 - Exit\n --> ");
            return GetUserChoice("", true, 1, 3);
        }

        /// <summary>
        /// Displays the loading process of files
        /// </summary>
        /// <param name="progressMessage">The message</param>
        public void ShowLoading(string progressMessage)
        {
            sectionHeader = "Import File";
            Header();
            Console.Clear();
            Console.WriteLine(progressMessage);
        }

        /// <summary>
        /// Display the question for title or person search
        /// </summary>
        /// <param name="sectionTitle">Title or Person</param>
        /// <returns>The option of the user</returns>
        public string SearchFor(string sectionTitle)
        {
            string option;
            sectionHeader = sectionTitle;
            do
            {
                Header();
                Console.WriteLine();
                Console.WriteLine("     Please enter with the " + sectionTitle + " name for searching");
                Console.WriteLine("or press ENTER to return. You can type only a part of the name:");
                option = Console.ReadLine();
                if (option == "")
                {
                    Console.WriteLine("Do you confirm return to main menu? (Y/N):");
                    if (GetUserChoice("YN") == "y")
                    {
                        break;
                    }

                }
                else
                    break;
            } while (true);

            return option;
        }

        /// <summary>
        /// Displays the list of titles searched by the user
        /// </summary>
        /// <param name="moviesDB">Query reference</param>
        /// <param name="criteria">The previous response</param>
        /// <returns>Returns the selected title</returns>
        public string GetSelectedTitle(Query moviesDB, string criteria = "")
        {
            // Local variable
            string option = "";
            byte i = 0;
            bool morePages = false;
            bool restart = false;
            int currentPage = 1;
            int selectedPage = 1;
            byte minItem = 255;
            byte maxItem = 1;

            do
            {
                sectionHeader = "List of Titles";
                Header();
                Console.WriteLine("Filter Criteria: " + criteria);
                Console.Write("ITEMS |   TYPES   |");
                Console.Write("             PRIMARY TITLES             |");
                Console.WriteLine("YEAR |  GENRE  ");
                Console.WriteLine(RepeatChar('-', 74));
                restart = false;
                // Loop that goes through the list of titles that contains the word/s that the user typed
                foreach (Title field in moviesDB.FilteredTitles)
                {
                    if (currentPage == selectedPage)
                    {
                        Console.Write("{0,-6}|", "(" + field.ID + ")");
                        Console.Write("{0,-11}|", field.TitleType);
                        Console.Write("{0,-40}|", field.PrimaryTitle);
                        Console.Write("{0,-5}|", field.StartYear);
                        Console.WriteLine(field.Genres);

                        minItem = (minItem == 255) ? Byte.Parse(field.ID) : minItem;
                        maxItem = Byte.Parse(field.ID);
                        i++;
                        if (i > 19) // end of page
                        {
                            morePages = true;
                            if (currentPage == 1)
                            {
                                Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [N] to next page");
                                option = GetUserChoice("qn", true, minItem, maxItem);
                            }
                            else
                            {
                                Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [N] to next page, [P] to previous page:");
                                option = GetUserChoice("qnp", true, minItem, maxItem);
                            }

                            // User choose or quit
                            if ((Byte.TryParse(option, out byte bOp)) || (option == "q"))
                                break;

                            i = 0;

                            if (option == "n")
                            {
                                currentPage++;
                                selectedPage = currentPage;
                                sectionHeader = "List of Titles";
                                Header();
                                Console.WriteLine("Filter Criteria: " + criteria);
                                Console.Write("ITEMS |   TYPES   |");
                                Console.Write("             PRIMARY TITLES             |");
                                Console.WriteLine("YEAR |  GENRE  ");
                                Console.WriteLine(RepeatChar('-', 74));
                            }
                            else
                            {
                                restart = true;
                                selectedPage = currentPage - 1;
                                currentPage = 1;
                                break;
                            }
                        }
                        else
                            morePages = false;
                    }
                    else
                    {
                        i++;
                        if (i > 19)
                        {
                            i = 0;
                            currentPage++;
                        }
                    }
                }

                if (!morePages)
                {
                    Console.WriteLine("-----------------------------END------------------------------------");
                    Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [P] to previus page");
                    option = GetUserChoice("qp", true, minItem, maxItem);

                    if (option == "p")
                    {
                        restart = true;
                        selectedPage = currentPage - 1;
                        currentPage = 1;
                    }
                    else
                        break;
                }
            } while (restart);

            return option;

        }

        /// <summary>
        /// Displays the list of people searched by the user
        /// </summary>
        /// <param name="moviesDB">Query reference</param>
        /// <param name="criteria">The previous response</param>
        /// <returns>Returns the selected person</returns>
        public string GetSelectedPersons(Query moviesDB, string criteria = "")
        {
            // Local variable
            string option = "";
            byte i = 0;
            bool morePages = false;
            bool restart = false;
            int currentPage = 1;
            int selectedPage = 1;
            byte minItem = 255;
            byte maxItem = 1;

            do
            {
                sectionHeader = "List of Persons";
                Header();
                Console.WriteLine("Filter Criteria: " + criteria);
                Console.WriteLine("ITEMS\t| Primary Name\t| Primary Professions\n");
                restart = false;
                // Loop that goes through the list of persons that contains the word/s that the user typed
                foreach (Person field in moviesDB.FilteredNames)
                {
                    if (currentPage == selectedPage)
                    {
                        Console.WriteLine
                            ("(" + field.ID + ")\t|" + field.PrimaryName + "\t| " + field.PrimaryProfession);
                        minItem = (minItem == 255) ? Byte.Parse(field.ID) : minItem;
                        maxItem = Byte.Parse(field.ID);
                        i++;
                        if (i > 19) // end of page
                        {
                            morePages = true;
                            if (currentPage == 1)
                            {
                                Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [N] to next page");
                                option = GetUserChoice("qn", true, minItem, maxItem);
                            }
                            else
                            {
                                Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [N] to next page, [P] to previous page:");
                                option = GetUserChoice("qnp", true, minItem, maxItem);
                            }

                            // User choose or quit
                            if ((Byte.TryParse(option, out byte bOp)) || (option == "q"))
                                break;

                            i = 0;

                            if (option == "n")
                            {
                                currentPage++;
                                selectedPage = currentPage;
                                sectionHeader = "List of Persons";
                                Header();
                                Console.WriteLine("Filter Criteria: " + criteria);
                                Console.WriteLine("ITEMS\t| Primary Name\t| Primary Professions\n");
                            }
                            else
                            {
                                restart = true;
                                selectedPage = currentPage - 1;
                                currentPage = 1;
                                break;
                            }
                        }
                        else
                            morePages = false;
                    }
                    else
                    {
                        i++;
                        if (i > 19)
                        {
                            i = 0;
                            currentPage++;
                        }
                    }
                }

                if (!morePages)
                {
                    Console.WriteLine("-----------------------------END------------------------------------");
                    Console.WriteLine("Please enter the selected ITEM or [Q] to quit, [P] to previus page");
                    option = GetUserChoice("qp", true, minItem, maxItem);

                    if (option == "p")
                    {
                        restart = true;
                        selectedPage = currentPage - 1;
                        currentPage = 1;
                    }
                    else
                        break;
                }
            } while (restart);

            return option;
        }

        /// <summary>
        /// Displays the details of the selected title
        /// </summary>
        /// <param name="moviesDB">Query reference</param>
        /// <returns>Returns the option chosen by the user</returns>
        public string TitleDetails(Query moviesDB)
        {
            bool hasEpisodes = false;
            bool hasParent = false;
            string validOptions = "";
            sectionHeader = "Detail of Title";
            Header();
            foreach (Details field in moviesDB.FilteredDetails)
            {
                Console.WriteLine("{0,17}: {1}", "Code", field.Tconst);
                Console.WriteLine("{0,17}: {1}", "Name", field.PrimaryTitle);
                Console.WriteLine("{0,17}: {1}", "Original Name", field.OriginalTitle);
                Console.WriteLine("{0,17}: {1}", "Title Type", field.TitleType);
                Console.WriteLine("{0,17}: {1}", "Genre", field.Genres);
                Console.WriteLine("{0,17}: {1}", "Average Rating", field.AverageRating);
                Console.WriteLine("{0,17}: {1}", "Number of Votes", field.NumVotes);
                Console.WriteLine("{0,17}: {1}", "Start Year", field.StartYear);
                Console.WriteLine("{0,17}: {1}", "End Year", field.EndYear);
                Console.WriteLine("{0,17}: {1}", "Duration", field.RuntimeMinutes);
                Console.WriteLine("{0,17}: {1}", "Adults Only", field.IsAdult);
                if (field.TitleType == "tvEpisode")
                {
                    Console.WriteLine("{0,17}: {1}", "From the series", field.ParentTitle);
                    Console.WriteLine("{0,17}: {1}", "Season", field.SeasonNumber);
                    Console.WriteLine("{0,17}: {1}", "Episode", field.EpisodeNumber);
                    hasParent = true;
                }
                hasEpisodes = (field.TitleType == "tvSeries");
            }
            Console.WriteLine(RepeatChar('-', 15));
            Console.WriteLine("You have more options:");
            Console.WriteLine("[T] to search another Title");
            Console.WriteLine("[P] to list the names/crew (time-consuming)");
            validOptions = "tp";
            if (hasEpisodes)
            {
                Console.WriteLine("[E] to list the Episodes");
                validOptions += "e";
            }
            if (hasParent)
            {
                Console.WriteLine("[S] for list the Series");
                validOptions += "s";
            }
            return GetUserChoice(validOptions);
        }

        /// <summary>
        /// Displays the details of the selected person
        /// </summary>
        /// <param name="moviesDB">QUery reference</param>
        /// <returns>Returns the option chosen by the user</returns>
        public string PersonDetails(Query moviesDB)
        {
            sectionHeader = "Detail of Person";
            Header();
            foreach (Person field in moviesDB.FilteredNameDetails)
            {
                Console.WriteLine("{0,17}: {1}", "Code", field.Nconst);
                Console.WriteLine("{0,17}: {1}", "Name", field.PrimaryName);
                Console.WriteLine("{0,17}: {1}", "Birth Year", field.BirthYear);
                Console.WriteLine("{0,17}: {1}", "Death Year", field.DeathYear);
                Console.WriteLine("{0,17}: {1}", "Primary professions", field.PrimaryProfession);
            }

            Console.WriteLine(RepeatChar('-', 15));
            Console.WriteLine("You have more options:");
            Console.WriteLine("[P] to search another Person");
            Console.WriteLine("[T] to list the Titles (time-consuming)");
            return GetUserChoice("pt");
        }
    }
}

