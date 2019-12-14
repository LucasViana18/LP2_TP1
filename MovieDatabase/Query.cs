using System;
using System.Collections.Generic;
using System.Text;
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
        public IEnumerable<Details> FilteredDetails { get; private set; }
        public IEnumerable<Details> FilteredParent { get; private set; }
        public IEnumerable<Details> FilteredEpisodes { get; private set; }
        public IEnumerable<Person> FilteredNameDetails { get; private set; }
        public IEnumerable<Person> FilteredNames { get; private set; }
        public IEnumerable<Title> FilteredTitlesWithPerson { get; private set; }
        public IEnumerable<Person> FilteredPeopleInTitle { get; private set; }

        public string CurrentTitleID { get; set; }

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
            FilteredEpisodes = new List<Details>();
        }

        public void LoadFiles(string fileType)
        {
            // Local variables
            string line;
            string[] tempArray;
            string lastLine = null;

            Console.Clear();
            Console.WriteLine("Reading the " + fileType + " database. Please wait...");
            // Load the file title.basics
            if (fileType == "titles")
            {
                // Process of descompress and being able to read the file
                FileStream fs = new FileStream(fileTitleBasicsPath, FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gz);

                // To ignore the first line(categories)
                sr.ReadLine();

                // Loop through all the lines and store it in the titles list
                while ((line = sr.ReadLine()) != null)
                {
                    line += "\t0";
                    if (line != lastLine)
                    {
                        tempArray = line.Split('\t');
                        titles.Add(new Title(tempArray));
                    }
                    lastLine = line;
                }
                // Close the stream reader
                sr.Close();
            }
            // Load the file title.ratings
            if (fileType == "ratings")
            {
                // Process of descompress and being able to read the file
                FileStream fs = new FileStream(fileTitleRatingsPath, FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gz);

                // To ignore the first line(categories)
                sr.ReadLine();

                // Loop through all the lines and store it in the ratings list
                while ((line = sr.ReadLine()) != null)
                {
                    if (line != lastLine)
                    {
                        tempArray = line.Split('\t');
                        ratings.Add(new Rating(tempArray));
                    }
                    lastLine = line;
                }
                // Close the stream reader
                sr.Close();
            }
            // Load the file title.episode
            if (fileType == "episodes")
            {
                FileStream fs = new FileStream(fileTitleEpisodesPath, FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gz);

                // To ignore the first line(categories)
                sr.ReadLine();

                // Loop through all the lines and store it in the titles list
                while ((line = sr.ReadLine()) != null)
                {
                    line += "\t0";
                    if (line != lastLine)
                    {
                        tempArray = line.Split('\t');
                        episodes.Add(new Episode(tempArray));
                    }
                    lastLine = line;
                }

                // Close the stream reader
                sr.Close();
            }
            // Load the file name.basics
            if (fileType == "names")
            {
                FileStream fs = new FileStream(fileNameBasicsPath, FileMode.Open, FileAccess.Read);
                GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
                StreamReader sr = new StreamReader(gz);

                // To ignore the first line(categories)
                sr.ReadLine();

                // Loop through all the lines and store it in the titles list
                while ((line = sr.ReadLine()) != null)
                {
                    line += "\t0";
                    if (line != lastLine)
                    {
                        tempArray = line.Split('\t');
                        people.Add(new Person(tempArray));
                    }
                    lastLine = line;
                }

                // Close the stream reader
                sr.Close();
            }
        }

        public void ProcessListOfResults(string name, bool isTitle)
        {
            // Local variable
            int ID = 0;
            if (isTitle)
            {
                // Select the titles with the certain word typed by the user
                FilteredTitles =
                    (from x in titles

                     where x.PrimaryTitle.ToLower().Contains(name)

                     select new Title(new string[]
                     {x.Tconst, x.TitleType, x.PrimaryTitle, x.OriginalTitle,
                    x.IsAdult.ToString(), x.StartYear.ToString(),
                    x.EndYear.ToString(), x.RuntimeMinutes.ToString(),
                    x.Genres, (ID++).ToString()})).ToList();
            }
            else
            {
                FilteredNames =
                    (from y in people
                     where y.PrimaryName.ToLower().Contains(name)
                     select new Person(new string[]
                     {y.Nconst, y.PrimaryName, y.BirthYear, y.DeathYear,
                     y.PrimaryProfession, y.KnownForTitles,
                     (ID++).ToString()})).ToList();
            }
        }

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

        public void ProcessDetails(string selectedID, bool isTitle)
        {
            if (isTitle)
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
                     {   f.Tconst, f.TitleType, f.PrimaryTitle, f.OriginalTitle,
                    f.IsAdult.ToString(), f.StartYear.ToString(),
                    f.EndYear.ToString(), f.RuntimeMinutes.ToString(),
                    f.Genres, oR?.AverageRating.ToString() ?? noRating,
                    oR?.NumVotes.ToString() ?? noRating, oE?.SeasonNumber ?? "",
                    oE?.EpisodeNumber ?? "", GetParentTitle(oE?.ParentTconst ?? ""), ""
                     })).ToList();
            }
            else
            {
                FilteredNameDetails =
                    (from f in FilteredNames
                     where f.ID == selectedID
                     select new Person(new string[]
                     {f.Nconst, f.PrimaryName, f.BirthYear, f.DeathYear, f.PrimaryProfession,
                    f.KnownForTitles,""})).ToList();
            }
        }

        public void ProcessParent()
        {
            FilteredParent =
                from d in FilteredDetails
                join e in episodes on d.Tconst equals e.Tconst
                join t in titles on e.ParentTconst equals t.Tconst
                join r in ratings on t.Tconst equals r.Tconst into gj

                from l in gj.DefaultIfEmpty()

                select new Details
                (new string[]
                {   t.Tconst, t.TitleType, t.PrimaryTitle, t.OriginalTitle,
                    t.IsAdult.ToString(), t.StartYear.ToString(),
                    t.EndYear.ToString(), t.RuntimeMinutes.ToString(),
                    t.Genres, l?.AverageRating.ToString() ?? noRating,
                    l?.NumVotes.ToString() ?? noRating, e.SeasonNumber, e.EpisodeNumber,
                    "", ""
                });
        }

        public void ProcessEpisode()
        {
            short ID = 0;
            // Search and store into an IEnumerable the details of the episodes
            FilteredEpisodes =
                (from d in FilteredDetails
                 join e in episodes on d.Tconst equals e.ParentTconst
                 join t in titles on e.Tconst equals t.Tconst
                 join r in ratings on t.Tconst equals r.Tconst into gj

                 from l in gj.DefaultIfEmpty()

                 select new Details
                 (new string[]
                 {
                    t.Tconst, t.TitleType, t.PrimaryTitle, t.OriginalTitle,
                    t.IsAdult.ToString(), t.StartYear.ToString(),
                    t.EndYear.ToString(), t.RuntimeMinutes.ToString(),
                    t.Genres, l?.AverageRating.ToString() ?? noRating,
                    l?.NumVotes.ToString() ?? noRating, e.SeasonNumber, e.EpisodeNumber,
                    "", (ID++).ToString()
                 })).ToList();
        }

        public void ProcessTitlesWithPerson()
        {
            short ID = 0;

            FilteredTitlesWithPerson =
                (from f in FilteredNameDetails
                 from t in titles

                 where f.KnownForTitles.Split(',').Any(x => x.Contains(t.Tconst))

                 select new Title(new string[]
                 {t.Tconst, t.TitleType, t.PrimaryTitle, t.OriginalTitle,
                t.IsAdult.ToString(), t.StartYear.ToString(),
                t.EndYear.ToString(), t.RuntimeMinutes.ToString(),
                t.Genres, (ID++).ToString()})).ToList();

        }

        public void ProcessPeopleInTitle()
        {
            short ID = 0;

            FilteredPeopleInTitle =
                from p in people

                where p.KnownForTitles.Split(',').Any(x => x.Contains(CurrentTitleID))

                select new Person(new string[]
                {p.Nconst, p.PrimaryName, p.BirthYear, p.DeathYear,
                p.PrimaryProfession, p.KnownForTitles,
                (ID++).ToString()});
        }
    }
}
