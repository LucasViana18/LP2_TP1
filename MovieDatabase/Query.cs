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
        public IEnumerable<Title> FilteredTitles { get; private set; }
        public IEnumerable<Details> FilteredDetails { get; private set; }
        public IEnumerable<Details> FilteredParent { get; private set; }
        public IEnumerable<Details> FilteredEpisodes { get; private set; }

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
            FilteredTitles = new List<Title>();
            FilteredDetails = new List<Details>(); 
            FilteredEpisodes = new List<Details>();
        }

        public void LoadTitle()
        {
            // Local variables
            string line;
            string[] tempArray;
            string lastLine = null;

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

        public void LoadRating()
        {
            // Local variables
            string line;
            string[] tempArray;
            string lastLine = null;

            // Process of descompress and being able to read the file
            FileStream fs = new FileStream(fileTitleRatingsPath, FileMode.Open, FileAccess.Read);
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
                    ratings.Add(new Rating(tempArray));
                }
                lastLine = line;
            }
            // Close the stream reader
            sr.Close();
        }

        public void LoadEpisode()
        {
            // Local variables
            string line;
            string[] tempArray;
            string lastLine = null;

            // Process of descompress and being able to read the file
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

        public void ProcessTitle(string name)
        {
            int ID = 0;
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

        public void ProcessDetails(string selectedID)
        {
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
                    oE?.EpisodeNumber ?? "", GetParentTitle(oE?.ParentTconst ?? ""), "" })).ToList();
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
    }
}
