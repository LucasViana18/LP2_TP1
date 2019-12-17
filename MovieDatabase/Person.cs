namespace MovieDatabase
{
    /// <summary>
    /// Stores the categories of the file Person
    /// </summary>
    public class Person
    {
        // Properties
        public string Nconst { get; private set; }
        public string PrimaryName { get; private set; }
        public string BirthYear { get; private set; }
        public string DeathYear { get; private set; }
        public string PrimaryProfession { get; private set; }
        public string KnownForTitles { get; private set; }
        public string ID { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="array">The line that contains the info of a person</param>
        public Person(string[] array)
        {
            Nconst = array[0];
            PrimaryName = array[1];
            BirthYear = array[2];
            DeathYear = array[3];
            PrimaryProfession = array[4];
            KnownForTitles = array[5];
            ID = array[6];
        }
    }
}
