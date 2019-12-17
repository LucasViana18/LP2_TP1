namespace MovieDatabase
{
    class Program
    {
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
