using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sudoku_Solver
{
    /// <summary>
    /// Represents the thinker
    /// </summary>
    public class Agent
    {
        #region Variables
        /// <summary>
        /// Holds all of the states made within the agent
        /// </summary>
        List<State> States;

        //Keeps track of the list of tried states once trying occurs.
        List<State> TryStack;

        /// <summary>
        /// A list of the tried spaces that failed to work
        /// </summary>
        List<State> TriedSpaces;

        /// <summary>
        /// Holds the most recent state.
        /// </summary>
        State RecentState;

        /// <summary>
        /// Holds the current state. This may or may not be the
        /// same as the recent state, as backtracking may change
        /// this.
        /// </summary>
        public State CurrentState
        {
            get;
            private set;
        }

        /// <summary>
        /// The board in use
        /// </summary>
        public Board GameBoard;

        /// <summary>
        /// Holds the location of the Space with the smallest amout of possibilities
        /// </summary>
        Space SmallestSpace;

        /// <summary>
        /// Holds the location of the Space with only one possibility
        /// </summary>
        Space SingleSpace;

        /// <summary>
        /// Holds the space in which the number decision will be made
        /// </summary>
        Space ChosenSpace;

        /// <summary>
        /// Holds the location of the Space with a unique possibility
        /// </summary>
        Space UniqueSpace;

        /// <summary>
        /// Holds the Square with the smallest average possibility size
        /// </summary>
        Square SmallestSquare;

        /// <summary>
        /// Dictates whether or not the Agent is trying or not.
        /// Once the Agent enters Try mode, it doesn't leave
        /// until it finds a solution.
        /// </summary>
        public bool IsTrying;

        /// <summary>
        /// Dictates that the Agent is taking a guess
        /// </summary>
        bool IsTakingAGuess;

        /// <summary>
        /// Dictates whether or not the Agent is backtracking
        /// </summary>
        public bool Backtracking;

        /// <summary>
        /// Dictates whether or not the Agent have run out of choices to make
        /// </summary>
        public bool Deadended;

        /// <summary>
        /// Dictates if this is the first action being made
        /// </summary>
        public bool FirstTime;

        /// <summary>
        /// Holds the number decision for ChosenSpace
        /// </summary>
        string ChosenNumber;

        /// <summary>
        /// Keeps track of how many times a guess has been made
        /// </summary>
        int Tries;

        /// <summary>
        /// Tell me what method the Agent is in (for printing purposes)
        /// </summary>
        public string In;
        TimeSpan Timer;

        public int Delay = 0;
        public bool Running = false;

        /// <summary>
        /// Used to show whether or not an error has been found
        /// </summary>
        private bool ErrorFound;

        #endregion
        public Agent(Board BoardInUse)
        {
            GameBoard = BoardInUse;

            States = new List<State>();
            TryStack = new List<State>();
            RecentState = null;
            CurrentState = null;
            FirstTime = true;
            IsTrying = Deadended = Backtracking = IsTakingAGuess = false;
            ChosenNumber = "";
            GameBoard.BoardComplete += new BoardCompletionEventHandler(FinishBoard);
            Timer = TimeSpan.Zero;
            In = "";
        }

        public void Update(GameTime gameTime)
        {
            In = "In Update()...";
            //TODO: Find out why some of guesses end up with green squares instead of blue squares

            Timer += gameTime.ElapsedGameTime;
            if (Timer.TotalMilliseconds >= Delay && Running)
            {
                if (FirstTime)
                {
                    if (CheckForErrors())
                    {
                        Running = false;
                    }
                    IsTakingAGuess = !GlobalSearch();
                    GameBoard.Squares[2, 2].AveragedEvent += new AveragesCalculatedEventHandler(FindLowestAveragePossieSize);
                    //^This is the last Square that's updated. So once this one has finished we can check all of them.
                    FirstTime = false;
                }
                else
                {
                    if (CheckForErrors())
                    {
                        IsTakingAGuess = true;
                        ErrorFound = true;
                    }
                    else
                    {
                        Deadended = !LocalSearch();
                        if (Deadended)
                        {
                            //If you can't find anymore solutions in the table...
                            if (!GlobalSearch())
                            {
                                ///It's a whole 'nother ball game now. Forget dead-ends; we have to start trying.
                                IsTakingAGuess = true;
                                IsTrying = true;
                                Deadended = false;
                            }
                        }
                    }
                }
                if (IsTakingAGuess)
                {
                    if (ErrorFound)
                        Undo();
                    Guess();
                    ErrorFound = false;
                }
                Timer -= Timer;
            }
        }

        /// <summary>
        /// Searches the whole table for singles, uniques, and the
        /// smallest space and squares contaning the smallest
        /// average possibility size. This will only be used for the
        /// initial search to find a starting point. After that, it
        /// only uses the LocalSearch().
        /// </summary>
        private bool GlobalSearch()
        {
            In = "In GlobalSearch()...";
            SmallestSquare = GameBoard.Squares[0, 0];
            Square CurSquare;
            Space CurSpace;
            SmallestSpace = GameBoard.Spaces[0, 0];
            //Searches for singles
            for (int SqRow = 0; SqRow < 3; SqRow++)
            {
                for (int SqCol = 0; SqCol < 3; SqCol++)
                {
                    CurSquare = GameBoard.Squares[SqCol, SqRow];
                    if (CurSquare.IsComplete)//If every space is absolute
                        continue;
                    for (int SpRow = 0; SpRow < 3; SpRow++)
                    {
                        for (int SpCol = 0; SpCol < 3; SpCol++)
                        {
                            CurSpace = CurSquare.Spaces[SpCol, SpRow];

                            if (CurSpace.IsAbsolute)
                                continue;
                            if (SmallestSpace.Possibilities.Count == 1)
                                SmallestSpace = CurSpace;

                            else if (CurSpace.Possibilities.Count == 1)
                                SingleSpace = CurSpace;
                            for (int p_index = 0; p_index < CurSpace.Possibilities.Count; p_index++)
                            {
                                if (CurSpace.Possibilities[p_index].IsUnique)
                                    UniqueSpace = CurSpace;
                                break;
                                ///Thanks to some code in the Board class, a
                                ///UniqueSpace will never be equal to the
                                ///SingleSpace.
                            }
                            //Keep track of the square with the smallest average possibility size
                            if (CurSquare.AveragePossibilitySize < SmallestSquare.AveragePossibilitySize)
                                SmallestSquare = CurSquare;

                            //Keep track of Space with the smallest Possibility Size
                            if (CurSpace.Possibilities.Count > 1 && CurSpace.Possibilities.Count < SmallestSpace.Possibilities.Count)
                                SmallestSpace = CurSpace;
                        }
                    }
                }
            }
            if (SingleSpace != null)
            {
                if (IsTrying)
                {
                    Try(SingleSpace, SingleSpace.Possibilities[0].Number);
                    Act(SingleSpace, true);
                }
                else
                {
                    Learn(SingleSpace, SingleSpace.Possibilities[0].Number);
                    Act(SingleSpace, false);
                }
                Deadended = false;
                SingleSpace = null;
                UniqueSpace = null;
                return true;
            }
            else if (UniqueSpace != null)
            {
                for (int i = 0; i < UniqueSpace.Possibilities.Count; i++)
                {
                    if (UniqueSpace.Possibilities[i].IsUnique)
                    {
                        if (IsTrying)
                        {
                            Try(UniqueSpace, UniqueSpace.Possibilities[0].Number, i);
                            Act(UniqueSpace, true);
                        }
                        else
                        {
                            Learn(UniqueSpace, UniqueSpace.Possibilities[0].Number);
                            Act(UniqueSpace, false);
                        }
                        Deadended = false;
                        SingleSpace = null;
                        UniqueSpace = null;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Does a search on only the current state's affected spaces
        /// and learns and acts on the locally best space while
        /// storing the rest as possible future choices
        /// </summary>
        /// <returns>Returns true if a single or unique is found and learned. Else returns false.</returns>
        private bool LocalSearch()
        {
            In = "In LocalSearch()...";
            ///Using the affected spaces array from the
            ///previous state, since they're going to be
            ///the most relevant, look at those for more
            ///options. However, only use their locations,
            ///since their information belongs to them, and
            ///are not directly linked to the ones within
            ///the gameboard.
            ///
            Space BSp;

            ///Each state has three arrays that need to be checked for
            ///singles and uniques. This blank array allows for easy
            ///switching between them by assigning one of these
            ///particular arrays to the switchboard array and
            ///querying it within a foreach loop. It's better than
            ///constantly copying code. There are three arrays for
            ///the effect of being able to know exactly when one is
            ///one checking square spaces, column spaces, or row
            ///spaces.
            ///

            Space[] SpaceArraySwitch;
            ///Reoresents the first time a space with this possie
            ///property has been found
            bool FirstFind = true;

            int UniqueIndex = 0;
            ///Check to see if there are any Squares on the board
            ///that have the same amount of possibilities. Then
            ///Check among those to see if their possibilities
            ///list are the same
            ///

            ///We'll first accumulate our possible actions/choices
            ///to see what we can do in this state. Then we act on
            ///the best one (i.e. single, then unique, then
            ///backtrack)
            ///

            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        //First search the Square
                        SpaceArraySwitch = CurrentState.AffectedSquareSpaces;
                        break;
                    case 1:
                        //Then Column
                        SpaceArraySwitch = CurrentState.AffectedColumnSpaces;
                        break;
                    case 2:
                        //Then Row
                        SpaceArraySwitch = CurrentState.AffectedRowSpaces;
                        break;
                    default:
                        SpaceArraySwitch = CurrentState.AffectedSquareSpaces;
                        break;
                }

                foreach (Space sp in SpaceArraySwitch)
                {
                    if (sp != null)
                    {
                        BSp = GameBoard.Spaces[sp.TableLocation.X, sp.TableLocation.Y];
                        if (BSp.Possibilities.Count == 1)//If it's a single
                        {
                            SingleSpace = BSp;
                            if (IsTrying)
                            {
                                Try(SingleSpace, SingleSpace.Possibilities[0].Number);
                                Act(SingleSpace, true);
                            }
                            else
                            {
                                Learn(SingleSpace, SingleSpace.Possibilities[0].Number);
                                Act(SingleSpace, false);

                            }
                            //Cleanup
                            SingleSpace = null;
                            UniqueSpace = null;
                            return true;
                        }

                        else if (BSp.Possibilities.Count > 1)
                        {
                            //Check for a unique number
                            for (int p_index = 0; p_index < BSp.Possibilities.Count; p_index++)
                            {
                                if (BSp.Possibilities[p_index].IsUnique && FirstFind)
                                {
                                    UniqueSpace = BSp;
                                    UniqueIndex = p_index;
                                    FirstFind = false;
                                }
                            }
                        }
                    }
                }
                if (UniqueSpace != null)
                {
                    //then you've found a unique number. Act on it.
                    if (IsTrying)
                    {
                        Try(UniqueSpace, UniqueSpace.Possibilities[UniqueIndex].Number);
                        Act(UniqueSpace, true);
                    }
                    else
                    {
                        Learn(UniqueSpace, UniqueSpace.Possibilities[UniqueIndex].Number);
                        Act(UniqueSpace, false);
                    }
                    //Cleanup
                    SingleSpace = null;
                    UniqueSpace = null;
                    return true;
                }
            }
            return false;//something trickier must be done
        }

        /// <summary>
        /// Sets the CurrentState back to the previous state
        /// until it either reaches a state that initiates a
        /// try, or reaches the first state (i.e. No
        /// previous state)
        /// </summary>
        private void Undo()
        {
            In = "In Undo()...";
            ///we backtrack through states to hopefully find a better
            ///solution. While we haven't reached a state where we started
            ///trying or while the current state isn't the very first one...
            ///

            //Look at the previous state and search
            ///While the the current state is not a try state or equal to the first state...
            ///
            int TryI = TryStack.Count - 1;
            if (TryI >= 0)
            {
                do
                {
                    Space CurSpace = TryStack[TryI].UsedSpace;
                    Space[] SpaceArraySwitch;

                    //Undo the space and all its possibilities
                    GameBoard.Spaces[CurSpace.TableLocation.X,
                        CurSpace.TableLocation.Y].SetNumber("", false);

                    GameBoard.Spaces[CurSpace.TableLocation.X,
                        CurSpace.TableLocation.Y].Possibilities.Clear();

                    GameBoard.Spaces[CurSpace.TableLocation.X,
                        CurSpace.TableLocation.Y].Possibilities.AddRange(CurSpace.CopyPossies());

                    for (int i = 0; i < 3; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                SpaceArraySwitch = TryStack[TryI].AffectedSquareSpaces;
                                break;
                            case 1:
                                //Then Column
                                SpaceArraySwitch = TryStack[TryI].AffectedColumnSpaces;
                                break;
                            case 2:
                                //Then Row
                                SpaceArraySwitch = TryStack[TryI].AffectedRowSpaces;
                                break;
                            default:
                                SpaceArraySwitch = TryStack[TryI].AffectedSquareSpaces;
                                break;
                        }

                        foreach (Space sp in SpaceArraySwitch)
                        {
                            if (sp != null)
                            {
                                GameBoard.Spaces[sp.TableLocation.X,
                            sp.TableLocation.Y].SetNumber("", false);

                                GameBoard.Spaces[sp.TableLocation.X,
                                    sp.TableLocation.Y].Possibilities.Clear();

                                GameBoard.Spaces[sp.TableLocation.X,
                                    sp.TableLocation.Y].Possibilities.AddRange(sp.CopyPossies());
                            }
                        }
                    }
                    if (TryI == 0)
                        CurrentState = TryStack[0];
                    else
                        CurrentState = TryStack[TryI - 1];
                    TryStack.RemoveAt(TryI);
                } while (TryI >= 1 && !TryStack[TryI--].TryState);
                //TODO: Find out why/how this while loop is out of range
            }
        }

        /// <summary>
        /// Picks a number from the CurrentState's UsedSpace
        /// </summary>
        private void Guess()
        {
            //TODO: Actually ADD the code that makes it choose another space for a guess
            In = "In Guess()...";
            #region Priority
            ///Priority:
            ///1. Singles
            ///2. Uniques
            ///3. Smallest average Possibility size with the most impact
            ///4. Smallest average possibility size
            ///
            ///Meta-Priority:
            ///Squares
            ///Columns
            ///Rows
            ///
            #endregion

            ///First, check to see if this space has any guesses
            ///left. Since the SmallestSpace has been found in
            ///FindLowestAveragePossieSize(), which happens
            ///right after the last piece was placed, and since
            ///it doesn't get erased until here, it should still
            ///have a value.
            ///
            if (CurrentState.TryState)
            {
                //Undo the space and all its possibilities
                GameBoard.Spaces[CurrentState.UsedSpace.TableLocation.X,
                    CurrentState.UsedSpace.TableLocation.Y].SetNumber("", false);

                GameBoard.Spaces[CurrentState.UsedSpace.TableLocation.X,
                    CurrentState.UsedSpace.TableLocation.Y].Possibilities.Clear();

                GameBoard.Spaces[CurrentState.UsedSpace.TableLocation.X,
                    CurrentState.UsedSpace.TableLocation.Y].Possibilities.AddRange(CurrentState.UsedSpace.CopyPossies());
                int index = CurrentState.GuessIndex + 1;

                //If there are more possibilities...
                if (index < CurrentState.UsedSpace.Possibilities.Count)
                {
                    //Remove this version of the state
                    TryStack.Remove(CurrentState);
                    //Try them immediately
                    Try(CurrentState.UsedSpace, CurrentState.UsedSpace.Possibilities[index].Number, index);
                    Act(GameBoard.Spaces[CurrentState.UsedSpace.TableLocation.X, CurrentState.UsedSpace.TableLocation.Y], true);
                }
            }
            else
            {
                SmallestSpace = SmallestSquare.Spaces[0, 0];
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (SmallestSpace.IsAbsolute)
                        {
                            SmallestSpace = SmallestSquare.Spaces[col, row];
                            continue;
                        }
                        if (SmallestSquare.Spaces[col, row].Possibilities.Count < SmallestSpace.Possibilities.Count &&
                            SmallestSquare.Spaces[col, row].IsAbsolute)
                            SmallestSpace = SmallestSquare.Spaces[col, row];
                    }
                }
                //Give it a shot
                if (SmallestSpace.Possibilities.Count != 0)
                {
                    Try(SmallestSpace, SmallestSpace.Possibilities[0].Number);
                    Act(SmallestSpace, true);
                }

            }
            IsTakingAGuess = false;
            //Cleanup
            UniqueSpace = null;
            SingleSpace = null;
            ChosenNumber = "";
        }

        /// <summary>
        /// Creates a new state based off the current space
        /// information and sets the RecentState and
        /// CurrentState to this new state
        /// </summary>
        /// <param name="sp">the Space in question</param>
        /// <param name="ChosenNumber">The number chosen for the Space</param>
        private void Learn(Space sp, string ChosenNumber)
        {
            States.Add(new State(this, sp, ChosenNumber, false, States.Count, CurrentState));
            CurrentState = States[States.Count - 1];
            RecentState = States[States.Count - 1];
        }

        /// <summary>
        /// Tries the number within the space and adds
        /// it to the try stack Current State is
        /// updated to this state
        /// </summary>
        /// <param name="sp">The space to remember</param>
        /// <param name="ChosenNumber">The chosen number</param>
        /// <param name="GuessIndex">The index that holds the chosen number</param>
        private void Try(Space sp, string ChosenNumber, int GuessIndex = 0)
        {
            States.Add(new State(this, sp, ChosenNumber, true, States.Count, CurrentState, GuessIndex));
            TryStack.Add(States[States.Count - 1]);
            CurrentState = States[States.Count - 1];
            RecentState = States[States.Count - 1];
        }

        /// <summary>
        /// Sets the state's ChosenNumber
        /// </summary>
        /// <param name="ChosenSpace">The Space being acted upon. This 
        /// cannot be CurrentState.UsedSpace. Seemingly nothing will 
        /// happen</param>
        /// <param name="IsATry">Dictates whether or not this action is a try</param>
        private void Act(Space ChosenSpace, bool IsATry)
        {
            if (!IsATry)
                ChosenSpace.SetNumber(CurrentState.ChosenNumber, true);
            else
            {
                ChosenSpace.SetNumber(CurrentState.ChosenNumber, false);
                Tries++;
            }
            ChosenSpace.Possibilities.Clear();
        }

        /// <summary>
        /// Finds the Square with the lowest Average possibility size and smallestSpace
        /// </summary>
        private void FindLowestAveragePossieSize()
        {
            In = "In FindLowestAveragePossieSize()...";
            SmallestSquare = GameBoard.Squares[0, 0];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (GameBoard.Squares[col, row].AveragePossibilitySize < SmallestSquare.AveragePossibilitySize &&
                        !GameBoard.Squares[col, row].IsComplete)
                        SmallestSquare = GameBoard.Squares[col, row];
                }
            }
            SmallestSpace = SmallestSquare.Spaces[0, 0];
            Space CurSpace;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    CurSpace = SmallestSquare.Spaces[col, row];
                    if (SmallestSpace.Possibilities.Count <= 1 && CurSpace.Possibilities.Count > 1)
                    {
                        SmallestSpace = CurSpace;
                    }
                    else if (CurSpace.Possibilities.Count < SmallestSpace.Possibilities.Count)
                    {
                        SmallestSpace = CurSpace;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the board to see if there are any illegal moves present
        /// </summary>
        /// <returns>returns false if there are none, else it returns true.</returns>
        private bool CheckForErrors()
        {
            ///If the space has no possibilities and a number has not been placed...
            ///
            List<Space> Singles = new List<Space>();

            foreach (Square sq in GameBoard.Squares)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (sq.Spaces[col, row].Possibilities.Count == 1)
                        {
                            Singles.Add(sq.Spaces[col, row]);
                        }
                    }
                }

                foreach (Space sp in Singles)
                {
                    for (int i = 0; i < Singles.Count; i++)
                    {
                        //If two singles have the same remaining possiblity...
                        if (Singles[i] != sp &&
                            sp.Possibilities[0].Number == Singles[i].Possibilities[0].Number)
                            return true;//We got a problem.
                    }
                }
                Singles.Clear();
            }

            return false;
        }

        /// <summary>
        /// Uses the TryStack List to finalize all the pieces on the board.
        /// </summary>
        private void FinishBoard()
        {
            In = "In FinishBoard()...";
            for (int i = TryStack.Count - 1; i >= 0; i--)
            {
                Space sp = GameBoard.Spaces[TryStack[i].UsedSpace.TableLocation.X,
                    TryStack[i].UsedSpace.TableLocation.Y];
                Act(sp, false);
                TryStack.RemoveAt(i);
            }
            Running = false;
        }
    }


}
