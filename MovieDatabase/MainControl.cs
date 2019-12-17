using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase
{
    public class MainControl
    {
        private Query moviesDB;
        private UserInterface userInterface;

        private string mainOption = "3";
        private string linkedKey = "";
        private bool linkQueries = false;

        public MainControl(Query setDataBase, UserInterface setInterface)
        {
            moviesDB = setDataBase;
            userInterface = setInterface;
        }

        public byte Execute()
        {
            bool loop = true;
            do
            {
                if (!linkQueries)
                    mainOption = userInterface.StartMenu();
                switch (mainOption)
                {
                    case "1":  // Title
                        QueryByTitle();
                        break;
                    case "2":  // Person
                        QueryByPerson();
                        break;
                    case "3":  // Exit
                        loop = false;
                        break;
                }
            } while (loop);
            return 0;
        }

        private void QueryByTitle()
        {
            string toSearch = "";
            string titleItem = "";
            string moreOption = "";
            string filterCriteria = "";
            FilterType filterType = FilterType.ftTitlesByName;

            if (linkQueries)
            {
                filterType = FilterType.ftTitlesByPerson;
                toSearch = linkedKey;
                filterCriteria = "Titles with " + moviesDB.CurrentPersonName;
            }

            moviesDB.ReleaseFiles();
            moviesDB.LoadFiles(DbType.dtTitles, userInterface);
            moviesDB.LoadFiles(DbType.dtRatings, userInterface);
            moviesDB.LoadFiles(DbType.dtEpisodes, userInterface);
            do
            {
                if (filterType == FilterType.ftTitlesByName)
                {
                    toSearch = userInterface.SearchFor("Titles");
                    if (toSearch == "")
                        break;
                    filterCriteria = "Titles with '" + toSearch + "'";
                }
                moviesDB.SearchForTitles(filterType, toSearch);
                titleItem = userInterface.GetSelectedTitle(moviesDB, filterCriteria);
                if (titleItem != "q")
                {
                    userInterface.Waiting();
                    moviesDB.ProcessDetails(titleItem, DbType.dtTitles);
                    moreOption = userInterface.TitleDetails(moviesDB);
                    userInterface.Waiting();
                    if (moreOption == "e")  // View Episodes
                    {
                        filterType = FilterType.ftEpisodesForSerie;
                        toSearch = moviesDB.CurrentTitleID;
                        filterCriteria = "Episodes of " + moviesDB.CurrentTitleName;
                    }
                    else if (moreOption == "s") // View Serie
                    {
                        filterType = FilterType.ftSerieForEpisode;
                        toSearch = moviesDB.CurrentTitleID;
                        filterCriteria = "Serie of the episode " + moviesDB.CurrentTitleName;
                    } 
                    else if (moreOption == "p") // View people
                    {
                        linkedKey = moviesDB.CurrentTitleID;
                        linkQueries = true;
                        mainOption = "2";  // For Search Persons
                        break;
                    }
                    else
                        filterType = FilterType.ftTitlesByName;
                }
            } while (true);


        }

        private void QueryByPerson()
        {
            string toSearch = "";
            string personItem = "";
            string moreOption = "";
            string filterCriteria = "";
            FilterType filterType = FilterType.ftPersonsByName;

            if (linkQueries)
            {
                filterType = FilterType.ftPersonsByTitle;
                toSearch = linkedKey;
                filterCriteria = "Persons/Crew of " + moviesDB.CurrentTitleName;
            } 
                
            moviesDB.ReleaseFiles();
            moviesDB.LoadFiles(DbType.dtPersons, userInterface);
            do
            {
                if (filterType == FilterType.ftPersonsByName)
                {
                    toSearch = userInterface.SearchFor("Persons");
                    if (toSearch == "")
                        break;
                    filterCriteria = "Person names with " + toSearch;
                }
                moviesDB.SearchForPersons(filterType, toSearch);
                personItem = userInterface.getSelectedPersons(moviesDB,filterCriteria);
                if (personItem != "q")
                {
                    userInterface.Waiting();
                    moviesDB.ProcessDetails(personItem, DbType.dtPersons);
                    moreOption = userInterface.PersonDetails(moviesDB);
                    if (moreOption == "t")
                    {
                        linkedKey = moviesDB.CurrentTitlesforPerson;
                        linkQueries = true;
                        mainOption = "1";  // For Search Titles
                        break;
                    }
                    else
                        linkQueries = false;
                    userInterface.Waiting();
                }
            } while (true);
        }
    }
}
