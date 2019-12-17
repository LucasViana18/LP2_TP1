namespace MovieDatabase
{
    /// <summary>
    /// Calls method for the execution of the program
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Local Instances
            UserInterface userInterface = new UserInterface();
            Query moviesDB = new Query();
            MainControl control = new MainControl(moviesDB, userInterface);

            // Start the program
            byte programResult = control.Execute();

            // End the program
            userInterface = null;
            moviesDB = null;
            System.Environment.Exit(programResult);

        }
    }
}
