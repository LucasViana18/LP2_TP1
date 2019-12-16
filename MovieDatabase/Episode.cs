namespace MovieDatabase
{
    public class Episode
    {
        public string Tconst { get; private set; }
        public string ParentTconst { get; private set; }
        public string SeasonNumber { get; private set; }
        public string EpisodeNumber { get; private set; }

        public Episode(string[] array)
        {
            Tconst = array[0];
            ParentTconst = array[1];
            SeasonNumber = array[2];
            EpisodeNumber = array[3];
        }
    }
}
