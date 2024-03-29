﻿using System;

namespace MovieDatabase
{
    /// <summary>
    /// Stores the categories of the file Rating
    /// </summary>
    public class Rating
    {
        // Properties
        public string Tconst { get; private set; }
        public float AverageRating { get; private set; }
        public int NumVotes { get; private set; }

        private float floatNum = 3f / 2f;
        private bool useComma;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="array">The line that contains the info 
        /// of a rating</param>
        public Rating(string[] array)
        {
            // Each category has stored an index of the array
            useComma = (floatNum.ToString().IndexOf(",") != -1);

            Tconst = array[0];

            if (useComma)
                AverageRating = (array[1] == @"\N") ? -1 : 
                    Convert.ToSingle(array[1].Replace(".", ","));
            else
                AverageRating = (array[1] == @"\N") ? -1 : 
                    Convert.ToSingle(array[1]);
            NumVotes = (array[2] == @"\N") ? -1 : Convert.ToInt32(array[2]);
        }
    }
}
