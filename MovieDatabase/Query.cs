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

        private string folderPath;
        private string fileTitleBasicsPath;
        private string fileTitleRatingsPath;
        private string fileTitleEpisodesPath;
        private string fileNameBasicsPath;

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
        }

        public void SearchTitle(string s)
        {
            FileStream fs = new FileStream(fileTitleBasicsPath, FileMode.Open, FileAccess.Read);

            GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);

            StreamReader sr = new StreamReader(gz);

            List<string[]> l = new List<string[]>();

            for(int i = 0; i < 5; i++)
            {
                l.Add(sr.ReadLine().Split('\t'));
            }

            // Fechar ficheiro
            /*sr.Close();
            IEnumerable<string> titles5060 =
                from title in l
                where title.startYear > 1950
                select title.primaryTitle;
                */
        }
    }
}
