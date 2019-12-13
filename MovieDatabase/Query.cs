﻿using System;
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

        private string folderPath;
        private string fileTitleBasicsPath;
        private string fileTitleRatingsPath;
        private string fileTitleEpisodesPath;
        private string fileNameBasicsPath;

        private List<Title> titles;
        private List<Rating> ratings;
        public IEnumerable<Title> FilteredTitles { get; private set; }

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
            FilteredTitles = new List<Title>();
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

        public void ProcessTitle (string name)
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
    }
}
