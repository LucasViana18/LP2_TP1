using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace MovieDatabase
{
    public class Query
    {
        private const string folderName = "MyIMDBSearcher";
        private const string fileTitleBasics = "title.basics.tsv.gz";
        private const string fileTitleRatings = "title.ratings.tsv.gz";
        private const string fileTitleEpisodes = "title.episode.tsv.gz";
        private const string fileNameBasics = "name.basics.tsv.gz";

        private const string noRating = "There is no rating for this title.";

        private string fileToImport;
        private string folderPath;
        private string fileTitleBasicsPath;
        private string fileTitleRatingsPath;
        private string fileTitleEpisodesPath;
        private string fileNameBasicsPath;

        private List<Title> titles;
        private List<Rating> ratings;
        private List<Episode> episodes;
        private List<Person> people;

        public IEnumerable<Title> FilteredTitles { get; private set; }
        public IEnumerable<Details> FilteredDetails { get; set; }
        public IEnumerable<Person> FilteredNameDetails { get; private set; }
        public IEnumerable<Person> FilteredNames { get; private set; }

        public string CurrentTitleID { get; set; }
        public string CurrentTitleName { get; set; }
        public string CurrentPersonID { get; set; }
        public string CurrentPersonName { get; set; }
        public string CurrentTitlesforPerson { get; set; }

        public Query()
        {
            folderPath = Path.Combine
                (Environment.GetFolderPath(Environment.SpecialFolder.
                LocalApplicationData), folderName);

            fileTitleBasicsPath =
                Path.Combine(folderPath, fileTitleBasics);
            fileTitleRatingsPath =
                Path.Combine(folderPath, fileTitleRatings);
            fileTitleEpisodesPath =
                Path.Combine(folderPath, fileTitleEpisodes);
            fileNameBasicsPath =
                Path.Combine(folderPath, fileNameBasics);

            titles = new List<Title>();
            ratings = new List<Rating>();
            episodes = new List<Episode>();
            people = new List<Person>();

            FilteredTitles = new List<Title>();
            FilteredDetails = new List<Details>();
            FilteredNames = new List<Person>();
            FilteredNameDetails = new List<Person>();
        }

        // --------------------------------------------------------------------

        // Method to import files from compacted files
        public void LoadFiles(DbType fileType, UserInterface userInt)
        {
            // Local variables
            string line;
            string[] tempArray;
            string lastLine = null;
            string fileName = "";

            bool addColumn = true;
            int processed = 0;
            int imported = 0;

            // Configure the work variables
            switch (fileType)
            {
                case DbType.dtTitles:
                    {
                        fileToImport = fileTitleBasicsPath;
                        fileName = "Titles";
                        break;
                    }
                case DbType.dtRatings:
                    {
                        fileToImport = fileTitleRatingsPath;
                        fileName = "Ratings";
                        addColumn = false;
                        break;
                    }
                case DbType.dtEpisodes:
                    {
                        fileToImport = fileTitleEpisodesPath;
                        fileName = "Episodes";
                        break;
                    }
                case DbType.dtPersons:
                    {
                        fileToImport = fileNameBasicsPath;
                        fileName = "Persons";
                        break;
                    }
            }

            userInt.ShowLoading("Decompressing database file. Please wait.");

            // Process of descompress and being able to read the file
            FileStream fs = new FileStream(fileToImport, FileMode.Open, FileAccess.Read);
            GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
            StreamReader sr = new StreamReader(gz);

            // To ignore the first line(categories)
            sr.ReadLine();

            // Loop through all the lines and store it in the titles list
            while ((line = sr.ReadLine()) != null)
            {
                processed++;
                if (line != lastLine)
                {
                    imported++;
                    lastLine = line;
                    if (addColumn)
                        line += "\t0";

                    tempArray = line.Split('\t');
                    switch (fileType)
                    {
                        case DbType.dtTitles:
                            {
                                titles.Add(new Title(tempArray));
                                break;
                            }
                        case DbType.dtRatings:
                            {
                                ratings.Add(new Rating(tempArray));
                                break;
                            }
                        case DbType.dtEpisodes:
                            {
                                episodes.Add(new Episode(tempArray));
                                break;
                            }
                        case DbType.dtPersons:
                            {
                                people.Add(new Person(tempArray));
                                break;
                            }
                    }

                }
                if ((processed % 85000) == 0)
                    userInt.ShowLoading("Importing " + fileName + "...\nProcessed " + processed + " lines. Imported " + imported + " records.");
            }
            // Close the stream reader
            sr.Close();
        }

        public void ReleaseFiles(DbType whatFile = DbType.dtAll)
        {
            if (whatFile == DbType.dtTitles || whatFile == DbType.dtAll)
                titles.Clear();
            if (whatFile == DbType.dtRatings || whatFile == DbType.dtAll)
                ratings.Clear();
            if (whatFile == DbType.dtEpisodes || whatFile == DbType.dtAll)
                episodes.Clear();
            if (whatFile == DbType.dtPersons || whatFile == DbType.dtAll)
                people.Clear();

            GC.Collect();
        }

        // --------------------------------------------------------------------

        // Search for Titles that contains a string
        public void SearchForTitles(FilterType filterType, string toSearch)
        {
            // Local variable
            int ID = 0;
            // Select the titles with the certain word typed by the user
            switch (filterType)
            {
                case FilterType.ftTitlesByName:
                    {
                        FilteredTitles =
                            (from x in titles

                             where x.PrimaryTitle.ToLower().Contains(toSearch)

                             select new Title(new string[]
                             {x.Tconst, x.TitleType, x.PrimaryTitle, x.OriginalTitle,
                              x.IsAdult.ToString(), x.StartYear.ToString(),
                              x.EndYear.ToString(), x.RuntimeMinutes.ToString(),
                              x.Genres, (ID++).ToString()})).ToList();
                        break;
                    }
                case FilterType.ftSerieForEpisode:
                    {
                        // Search and store into an IEnumerable the details of the parent
                        FilteredTitles =
                            (from x in titles
                             join e in episodes on x.Tconst equals e.ParentTconst

                             where (e.Tconst == toSearch)

                             select new Title
                             (new string[]
                              {x.Tconst, x.TitleType, x.PrimaryTitle, x.OriginalTitle,
                              x.IsAdult.ToString(), x.StartYear.ToString(),
                              x.EndYear.ToString(), x.RuntimeMinutes.ToString(),
                              x.Genres, (ID++).ToString()})).ToList();

                        break;
                    }
                case FilterType.ftEpisodesForSerie:
                    {
                        FilteredTitles =
                            (from e in episodes
                             join x in titles on e.Tconst equals x.Tconst

                             where (e.ParentTconst == toSearch)

                             select new Title
                             (new string[]
                             {x.Tconst, x.TitleType, x.PrimaryTitle, x.OriginalTitle,
                              x.IsAdult.ToString(), x.StartYear.ToString(),
                              x.EndYear.ToString(), x.RuntimeMinutes.ToString(),
                              x.Genres, (ID++).ToString()})).ToList();
                        break;
                    }
                case FilterType.ftTitlesByPerson:
                    {
                        FilteredTitles =
                            (from x in titles

                             where toSearch.Split(',').Any(y => y.Contains(x.Tconst))

                             select new Title
                             (new string[]
                             {x.Tconst, x.TitleType, x.PrimaryTitle, x.OriginalTitle,
                              x.IsAdult.ToString(), x.StartYear.ToString(),
                              x.EndYear.ToString(), x.RuntimeMinutes.ToString(),
                              x.Genres, (ID++).ToString()})).ToList();
                        break;
                    }
            }
        }

        public void SearchForPersons(FilterType filterType, string toSearch)
        {
            // Local variable
            int ID = 0;

            switch (filterType)
            {
                case FilterType.ftPersonsByName:
                    {
                        FilteredNames =
                            (from p in people
                             where p.PrimaryName.ToLower().Contains(toSearch)
                             select new Person(new string[]
                             {p.Nconst, p.PrimaryName, p.BirthYear, p.DeathYear,
                              p.PrimaryProfession, p.KnownForTitles,
                              (ID++).ToString()})).ToList();
                        break;
                    }
                case FilterType.ftPersonsByTitle:
                    {
                        FilteredNames =
                            (from p in people
                             where p.KnownForTitles.Split(',').Any(x => x.Contains(toSearch))
                             select new Person(new string[]
                             {p.Nconst, p.PrimaryName, p.BirthYear, p.DeathYear,
                              p.PrimaryProfession, p.KnownForTitles,
                              (ID++).ToString()})).ToList();
                        break;
                    }
            }
        }

        // Methods that filter the results

        private string GetParentTitle(string episodeParentID)
        {
            if (episodeParentID != "")
            {
                return titles.Find(item => item.Tconst == episodeParentID).PrimaryTitle;
            }
            else
            {
                return "";
            }
        }
        public void ProcessDetails(string selectedID, DbType database)
        {
            if (database == DbType.dtTitles)
            {
                // Select the details, including ratings, of the title, by ID, that the user typed
                FilteredDetails =
                    (from f in FilteredTitles
                     join r in ratings on f.Tconst equals r.Tconst into outerRating
                     join e in episodes on f.Tconst equals e.Tconst into outerEpisodes

                     from oR in outerRating.DefaultIfEmpty()
                     from oE in outerEpisodes.DefaultIfEmpty()

                     where f.ID == selectedID

                     select new Details
                     (new string[]
                     {  f.Tconst, f.TitleType, f.PrimaryTitle, f.OriginalTitle,
                        f.IsAdult.ToString(), f.StartYear.ToString(),
                        f.EndYear.ToString(), f.RuntimeMinutes.ToString(),
                        f.Genres, oR?.AverageRating.ToString() ?? noRating,
                        oR?.NumVotes.ToString() ?? noRating, oE?.SeasonNumber ?? "",
                        oE?.EpisodeNumber ?? "", GetParentTitle(oE?.ParentTconst ?? ""), ""
                     })).ToList();

                // Set up the current title code and nome
                foreach (Details field in FilteredDetails)
                {
                    CurrentTitleID = field.Tconst;
                    CurrentTitleName = field.PrimaryTitle;
                }
            }
            else if (database == DbType.dtPersons)
            {
                FilteredNameDetails =
                    (from f in FilteredNames
                     where f.ID == selectedID
                     select new Person(new string[]
                     {f.Nconst, f.PrimaryName, f.BirthYear, f.DeathYear, f.PrimaryProfession,
                    f.KnownForTitles,""})).ToList();

                // Set up the current person code and nome
                foreach (Person field in FilteredNameDetails)
                {
                    CurrentPersonID = field.Nconst;
                    CurrentPersonName = field.PrimaryName;
                    CurrentTitlesforPerson = field.KnownForTitles;
                }
            }
        }
    }
}
