namespace MovieDatabase
{
    /// <summary>
    /// Stores the categories of the file Episode
    /// </summary>
    public class Episode
    {
        // Properties
        public string Tconst { get; private set; }
        public string ParentTconst { get; private set; }
        public string SeasonNumber { get; private set; }
        public string EpisodeNumber { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="array">The line that contains the info of a episode</param>
        public Episode(string[] array)
        {
            Tconst = array[0];
            ParentTconst = array[1];
            SeasonNumber = array[2];
            EpisodeNumber = array[3];
        }
    }
}
