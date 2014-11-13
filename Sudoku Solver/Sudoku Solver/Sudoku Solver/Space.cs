using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sudoku_Solver
{
    /// <summary>
    /// There fro any events that may have to happen
    /// </summary>
    /// <param name="sp">The Space involved in the event. Since everything here is based off spaces anyway.</param>
    public delegate void PlacedNumberEventHandler(Space sp);

    /// <summary>
    /// Represents a space on the board
    /// </summary>
    public class Space
    {
        private bool b_absolute;

        private string s_number;

        /// <summary>
        /// Debugging purposes
        /// </summary>
        public string Name;

        /// <summary>
        /// The single number to be used
        /// </summary>
        public string ChosenNumber
        {
            get
            {
                return s_number;
            }
            set
            {
                string prevNum = s_number;
                s_number = value;

                if ((prevNum == "" && s_number != "") || prevNum != "" && s_number == "")
                {
                    //the value was changed
                    OnPlaced();
                    if (!b_absolute)
                        SpaceAccuracy = SpaceAccuracyStates.Try;
                }
                if (s_number == "")
                {
                    SpaceAccuracy = SpaceAccuracyStates.IDK;
                }
            }
        }

        /// <summary>
        /// Dictates the location of the Space within the table
        /// </summary>
        public Location TableLocation
        {
            get;
            private set;
        }

        /// <summary>
        /// The list of numbers to be drawn
        /// </summary>
        public List<Possibility> Possibilities;

        /// <summary>
        /// The current state of Accuracy for the space
        /// </summary>
        public SpaceAccuracyStates SpaceAccuracy;

        /// <summary>
        /// Will trigger when an absolute number is found.
        /// </summary>
        public event PlacedNumberEventHandler AbsoluteNumberFound;

        /// <summary>
        /// Trigger whenever a number is placed.
        /// </summary>
        public event PlacedNumberEventHandler NumberPlaced;

        //Dictates that this number cannot be changed
        public bool IsAbsolute
        {
            get
            {
                return b_absolute;
            }
            set
            {
                b_absolute = value;
                if (b_absolute)
                {
                    SpaceAccuracy = SpaceAccuracyStates.Right;
                    if (Possibilities != null)
                        Possibilities.Clear();
                }
            }
        }

        #region Constructors
        private Space()
        {
            ChosenNumber = "";
            Possibilities = new List<Possibility>();
            for (int i = 0; i < 9; i++)
            {
                Possibilities.Add(new Possibility((i + 1).ToString(), false));
            }
            SpaceAccuracy = SpaceAccuracyStates.IDK;
            b_absolute = false;
        }

        /// <summary>
        /// A particular Sudoku space holding either one correct, or
        /// possibly correct, number, or a list of possible numbers
        /// </summary>
        public Space(Location location)
            : this()
        {
            TableLocation = location;
        }

        /// <summary>
        /// A particular Sudoku space holding either one correct, 
        /// or possibly correct, number, or a list of possible numbers
        /// </summary>
        /// <param name="location">The location of the Space</param>
        /// <param name="Absolute">Whether or not the Space is absolute</param>
        /// <param name="number">The number placed</param>
        public Space(Location location, bool Absolute, string number)
            : this()
        {
            TableLocation = location;
            IsAbsolute = Absolute;
            s_number = number;
        }

        /// <summary>
        /// A particular Sudoku space holding either one correct, 
        /// or possibly correct, number, or a list of possible 
        /// numbers. THIS VERSION WILL USED ONLY BY THE AGENT.
        /// </summary>
        /// <param name="location">The location of the Space</param>
        /// <param name="possies">Its list of possibilities. These are autormatically copied.</param>
        /// <param name="Absolute">Whether or not the Space is absolute</param>
        /// <param name="number">The number placed</param>
        public Space(Location location, List<Possibility> possies, bool Absolute, string number)
        {
            TableLocation = new Location(location.X, location.Y);
            Possibilities = new List<Possibility>();
            IsAbsolute = Absolute;
            s_number = number;
            foreach (Possibility SpP in possies)
            {
                Possibilities.Add(new Possibility(SpP.Number, SpP.IsUnique));
            }
        }
        #endregion

        /// <summary>
        /// Copies a list of Possibilities to another List and then
        /// returns that list. Each entity the new list is
        /// completely separate from the copied list.
        /// </summary>
        /// <returns>Return the copy of the Possibility list of this space</returns>
        public List<Possibility> CopyPossies()
        {
            List<Possibility> result = new List<Possibility>();

            foreach (Possibility SpP in Possibilities)
            {
                result.Add(new Possibility(SpP.Number, SpP.IsUnique));
            }
            return result;
        }

        /// <summary>
        /// Allows the setting of the number and its absolute property simultaneously.
        /// This is made to prevent forgetting to consider both when placing a number.
        /// </summary>
        /// <param name="number">The number to which the space should be set.</param>
        /// <param name="Absolute">Whether or not this number will be considered absolute.</param>
        public void SetNumber(string number, bool Absolute)
        {
            ChosenNumber = number;
            IsAbsolute = Absolute;
        }

        private void OnPlaced()
        {
            if (NumberPlaced != null)
                NumberPlaced(this);
        }

        /// <summary>
        /// Checks to see if this space has a unique number possibility
        /// </summary>
        /// <returns>Returns true if it has one, else false</returns>
        public bool ShowUnique()
        {
            foreach (Possibility SpP in Possibilities)
            {
                if (SpP.IsUnique)
                    return true;
            }
            return false;
        }

    }
}
