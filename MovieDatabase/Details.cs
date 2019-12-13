using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDatabase
{
    public class Details
    {
        // Properties
        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public string IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string RuntimeMinutes { get; set; }
        public string Genres { get; set; }
        public string AverageRating { get; set; }
        public string numVotes { get; set; }
        public string ID { get; private set; }

        // Constructor
        public Details(string[] array)
        {
            // Each category has stored an index of the array
            Tconst = array[0];
            TitleType = array[1];
            PrimaryTitle = array[2];
            OriginalTitle = array[3];
            IsAdult = array[4];
            StartYear = array[5];
            EndYear = array[6];
            RuntimeMinutes = array[7];
            Genres = array[8];
            AverageRating = array[9];
            numVotes = array[10];
            ID = array[11];
        }
    }
}
