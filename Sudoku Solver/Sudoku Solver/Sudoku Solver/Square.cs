using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Sudoku_Solver
{
    /// <summary>
    /// Handles whenever the averages are updated;
    /// </summary>
    public delegate void AveragesCalculatedEventHandler(Square sender);

    /// <summary>
    /// Represents a section/sector of the Sudoku puzzle board
    /// </summary>
    public class Square
    {
        /// <summary>
        /// The collection of Spaces
        /// </summary>
        public Space[,] Spaces;

        /// <summary>
        /// Dictates the Location of Square on the table
        /// </summary>
        public Location TableLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// Dictates if all of the Spaces within this square is absolute.
        /// </summary>
        public bool IsComplete;

        /// <summary>
        /// Holds the Average Possibility size of the Square 
        /// based on how many spaces have possibilities
        /// </summary>
        public float AveragePossibilitySize;

        public event AveragesCalculatedEventHandler AveragedEvent;

        /// <summary>
        /// Denotes when all possible guesses
        /// have been tried for this square
        /// </summary>
        public bool OutOfGuesses;

        /// <summary>
        /// A collection of spaces
        /// </summary>
        public Square(Location location)
        {
            Spaces = new Space[3, 3];
            TableLocation = location;
            IsComplete = false;
            AveragePossibilitySize = 0;
            OutOfGuesses = false;
        }

        /// <summary>
        /// Calculates the Average Possibility Size based on how many 
        /// Spaces in the Square have possibilities (i.e. Aren't given or absolute)
        /// </summary>
        /// <param name="sp">This is useless, but necessary</param>
        private void CalculateAveragePossieSize(Space sp)
        {
            CalculateAveragePossieSize();
        }


        /// <summary>
        /// Calculates the Average Possibility Size based on how many 
        /// Spaces in the Square have possibilities (i.e. Aren't given or absolute)
        /// </summary>
        public void CalculateAveragePossieSize()
        {
            int SpaceCount = 0;//Holds how many space have possibilities (i.e. Aren't given or absolute)
            AveragePossibilitySize = 0;
            if (!IsComplete)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        ///For Trying purposes (see Agent) this MUST
                        ///check the possie size, for an absent possie
                        ///count does not signify an absolute space.
                        if (Spaces[col, row].Possibilities.Count > 1 && !Spaces[col, row].HasUnique())
                        {
                            SpaceCount++;
                            AveragePossibilitySize += Spaces[col, row].Possibilities.Count;
                        }
                    }
                }
                if (SpaceCount != 0)
                    AveragePossibilitySize /= SpaceCount;
                OnAveraged();
            }
        }

        private void OnAveraged()
        {
            if (AveragedEvent != null)
                AveragedEvent(this);
        }
    }
}
