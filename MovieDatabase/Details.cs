namespace MovieDatabase
{
    /// <summary>
    /// Stores the categories of all files
    /// </summary>
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
        public string NumVotes { get; set; }
        public string SeasonNumber { get; private set; }
        public string EpisodeNumber { get; private set; }
        public string ParentTitle { get; private set; }
        public string ID { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="array"></param>
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
            NumVotes = array[10];
            SeasonNumber = array[11];
            EpisodeNumber = array[12];
            ParentTitle = array[13];
            ID = array[14];
        }
    }
}
