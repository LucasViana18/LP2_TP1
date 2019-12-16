using System;

namespace MovieDatabase
{
    public class Title
    {
        // Properties
        public string ID { get; private set; }
        public string Tconst { get; private set; }
        public string TitleType { get; private set; }
        public string PrimaryTitle { get; private set; }
        public string OriginalTitle { get; private set; }
        public bool IsAdult { get; private set; }
        public int StartYear { get; private set; }
        public int EndYear { get; private set; }
        public int RuntimeMinutes { get; private set; }
        public string Genres { get; private set; }

        // Constructor
        public Title(string[] array)
        {
            // Each category has stored an index of the array
            Tconst = array[0];
            TitleType = array[1];
            PrimaryTitle = array[2];
            OriginalTitle = array[3];
            IsAdult = (array[4] == "1") ? true : false;
            StartYear = (array[5] == @"\N") ? -1 : Convert.ToInt32(array[5]);
            EndYear = (array[6] == @"\N") ? -1 : Convert.ToInt32(array[6]);
            RuntimeMinutes = (array[7] == @"\N") ? -1 : Convert.ToInt32(array[7]);
            Genres = array[8];
            ID = array[9];
        }
    }
}
