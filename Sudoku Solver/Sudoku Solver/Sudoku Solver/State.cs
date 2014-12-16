using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    /// <summary>
    /// Keeps track of what happened before the action of 
    /// this state, and the action that took place.
    /// </summary>
    public class State
    {
        /// <summary>
        /// Holds the value of the State ID. It's dictated
        /// via its order of creation
        /// </summary>
        public int ID;

        /// <summary>
        /// Dictates the index of the last possibility used
        /// on this State's space. This can be cross-
        /// referenced with the size of the possibility
        /// list to determine if there are more guesses
        /// for this state.
        /// </summary>
        public int GuessIndex;

        /// <summary>
        /// Holds the State's Parent
        /// </summary>
        public State PreviousState;

        /// <summary>
        /// Holds the child State. This will be used only
        /// if you have no dispatches.
        /// </summary>
        public State NextState;

        /// <summary>
        /// The agent using the State
        /// </summary>
        public Agent Thinker;

        /// <summary>
        /// The Space that was used
        /// </summary>
        public Space UsedSpace;

        /// <summary>
        /// The Spaces that were affected, along with their previous possibilities lists
        /// </summary>
        public Space[] AffectedSquareSpaces, AffectedColumnSpaces, AffectedRowSpaces;

        /// <summary>
        /// Keeps track of the Square in which the ChosenSpace is located
        /// </summary>
        public Location SquareLocation;

        /// <summary>
        /// The number that was placed
        /// </summary>
        public string ChosenNumber;

        /// <summary>
        /// Whether or not a guess was made within this state.
        /// This does not count for singles or uniques. If it
        /// were either of those, InitialTry would be false.
        /// 
        /// This will be used when trying out numbers after
        /// all singles and uniques have been found. Once the
        /// end of the try phase has ended, and with failure,
        /// this will allow the States to revert back to their
        /// original state, with the IntialTry being a flag,
        /// indicating where the try phase started.
        /// </summary>
        public bool TryState;

        /// <summary>
        /// Creates a state for the particular Agent
        /// </summary>
        /// <param name="thinker">The Agent</param>
        /// <param name="CurrentSpace">Space to which the chosen number belongs</param>
        /// <param name="ChosenNumber">The number placed in the Space</param>
        /// <param name="Try">Whether or not the Agent was trying out a number or declaring an absolute</param>
        ///<param name="id">The State's ID</param>
        ///<param name="CurrentState">The State's previous state</param>
        ///<param name="guessIndex">If guessing, this will hold the index of the possibility being used on this State.</param>
        public State(Agent thinker, Space CurrentSpace, string ChosenNumber, bool Try, int id, State CurrentState, int guessIndex = 0)
        {
            Thinker = thinker;
            UsedSpace = new Space(CurrentSpace.TableLocation, CurrentSpace.Possibilities,
                CurrentSpace.IsAbsolute, CurrentSpace.ChosenNumber);
            this.ChosenNumber = ChosenNumber;
            TryState = Try;
            ID = id;
            PreviousState = CurrentState;
            GuessIndex = guessIndex;

            //Makes the Square's location using the space
            SquareLocation = new Location(CurrentSpace.TableLocation.X / 3, CurrentSpace.TableLocation.Y / 3);
            ///6 within the adjacent Row-Squares +
            ///6 within the adjacent Column-Squares +
            ///8 within the Square in which the Space resides =
            ///20
            ///I will not use all of them, but I will never need more than this.
            AffectedSquareSpaces = new Space[8];
            AffectedColumnSpaces = new Space[6];
            AffectedRowSpaces = new Space[6];
            CalcuateAffectedSpaces();
            NextState = null;
        }

        /// <summary>
        /// Finds out and records each of the spaces affected by the current space.
        /// </summary>
        public void CalcuateAffectedSpaces()
        {
            int SCRindex = 0;
            Square CurSquare = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, UsedSpace.TableLocation.Y / 3];
            //Get all the spaces in the Square
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Space sp = CurSquare.Spaces[col, row];
                    if (!sp.IsAbsolute && sp != Thinker.GameBoard.Spaces[
                        UsedSpace.TableLocation.X, UsedSpace.TableLocation.Y])
                    {
                        AffectedSquareSpaces[SCRindex] = new Space(sp.TableLocation, sp.Possibilities, sp.IsAbsolute, sp.ChosenNumber);
                        SCRindex++;
                    }
                }
            }
            SCRindex = 0;
            //Get all the spaces in a column
            //Find the two squares for space acquisition
            Square Sq1, Sq2;
            switch (UsedSpace.TableLocation.Y / 3)
            {
                case 0:
                    Sq1 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 1];
                    Sq2 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 2];
                    break;
                case 1:
                    Sq1 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 0];
                    Sq2 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 2];
                    break;
                case 2:
                    Sq1 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 0];
                    Sq2 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 1];
                    break;
                default:
                    Sq1 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 1];
                    Sq2 = Thinker.GameBoard.Squares[UsedSpace.TableLocation.X / 3, 2];
                    break;
            }
            for (int i = 0; i < 3; i++)
            {
                Space sp = Sq1.Spaces[UsedSpace.TableLocation.X / 3, i];
                if (!sp.IsAbsolute)
                {
                    AffectedColumnSpaces[SCRindex] = new Space(sp.TableLocation, sp.Possibilities, sp.IsAbsolute, sp.ChosenNumber);
                    SCRindex++;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Space sp = Sq2.Spaces[UsedSpace.TableLocation.X / 3, i];
                if (!sp.IsAbsolute)
                {
                    AffectedColumnSpaces[SCRindex] = new Space(sp.TableLocation, sp.Possibilities, sp.IsAbsolute, sp.ChosenNumber);
                    SCRindex++;
                }
            }
            SCRindex = 0;
            //Get all the spaces in a row

            switch (UsedSpace.TableLocation.X / 3)
            {
                case 0:
                    Sq1 = Thinker.GameBoard.Squares[1, UsedSpace.TableLocation.Y / 3];
                    Sq2 = Thinker.GameBoard.Squares[2, UsedSpace.TableLocation.Y / 3];
                    break;
                case 1:
                    Sq1 = Thinker.GameBoard.Squares[0, UsedSpace.TableLocation.Y / 3];
                    Sq2 = Thinker.GameBoard.Squares[2, UsedSpace.TableLocation.Y / 3];
                    break;
                case 2:
                    Sq1 = Thinker.GameBoard.Squares[0, UsedSpace.TableLocation.Y / 3];
                    Sq2 = Thinker.GameBoard.Squares[1, UsedSpace.TableLocation.Y / 3];
                    break;
                default:
                    Sq1 = Thinker.GameBoard.Squares[1, UsedSpace.TableLocation.Y / 3];
                    Sq2 = Thinker.GameBoard.Squares[2, UsedSpace.TableLocation.Y / 3];
                    break;
            }
            for (int i = 0; i < 3; i++)
            {
                Space sp = Sq1.Spaces[i, UsedSpace.TableLocation.Y / 3];
                if (!sp.IsAbsolute)
                {
                    AffectedRowSpaces[SCRindex] = new Space(sp.TableLocation, sp.Possibilities, sp.IsAbsolute, sp.ChosenNumber);
                    SCRindex++;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Space sp = Sq2.Spaces[i, UsedSpace.TableLocation.Y / 3];
                if (!sp.IsAbsolute)
                {
                    AffectedRowSpaces[SCRindex] = new Space(sp.TableLocation, sp.Possibilities, sp.IsAbsolute, sp.ChosenNumber);
                    SCRindex++;
                }
            }
        }
    }
}
