using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Sudoku_Solver
{
    public delegate void AgentActedHandler(Agent sender, Space space);
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
        /// Holds the most recent state.
        /// </summary>
        public State RecentState { get; private set; }

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
        /// Used to make the AI start thinking and acting
        /// as opposed to waiting for the time interval
        /// to pass.
        /// </summary>
        public bool ForceAct;

        TimeSpan Timer;
        /// <summary>
        /// Keeps a list of the squares organized
        /// in the lowest average possibility size
        /// to the highest
        /// </summary>
        List<Square> PossieSizeSorted;

        public event AgentActedHandler AgentActed;

        public int Delay = 0;
        public bool Running = false;

        #endregion
        public Agent(Board BoardInUse, List<Square> PossieList)
        {
            GameBoard = BoardInUse;

            States = new List<State>();
            TryStack = new List<State>();
            PossieSizeSorted = PossieList;
            RecentState = null;
            CurrentState = null;
            FirstTime = true;
            IsTrying = Deadended = Backtracking = IsTakingAGuess = ForceAct = false;
            GameBoard.BoardComplete += new BoardCompletionEventHandler(FinishBoard);
            Timer = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            //TODO: Find out why some of guesses end up with green squares instead of blue squares

            Timer += gameTime.ElapsedGameTime;
            if ((Timer.TotalMilliseconds >= Delay && Running) || ForceAct)
            {
                if (FirstTime)
                {
                    if (CheckForErrors())
                    {
                        //Something's wrong with the initial setup.
                        Running = false;
                        return;
                    }
                    FindLowestAveragePossieSize(null);
                    IsTakingAGuess = !GlobalSearch();
                    foreach (Square s in GameBoard.Squares)
                    {
                        s.AveragedEvent += new AveragesCalculatedEventHandler(FindLowestAveragePossieSize);
                    }
                    //^This is the last Square that's updated. So once this one has finished we can check all of them.
                    FirstTime = false;
                }
                else
                {
                    if (CheckForErrors())
                    {
                        Undo();
                        IsTakingAGuess = true;
                        //shouldn't it take a guess before an error is found?
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
                    Guess();
                }
                Timer -= Timer;
                ForceAct = false;
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
            /*
             * Run through each square sorted in descending
             * order of average possie size and find a
             * single or unique space....
             */
            foreach (Square s in PossieSizeSorted)
            {
                foreach (Space Ss in s.Spaces)
                {
                    if (Ss.Possibilities.Count == 1)
                    {
                        SingleSpace = Ss;
                        break;
                    }
                    else if (Ss.HasUnique())
                    {
                        UniqueSpace = Ss;
                        break;
                    }
                }
                if (SingleSpace != null || UniqueSpace != null)
                    break;
            }
            //---------------------------------------------------------------------------------------
            //Then, act on it.
            if (SingleSpace != null)
            {
                Learn(SingleSpace, SingleSpace.Possibilities[0].Number, false, IsTrying);
                Act(SingleSpace, IsTrying);

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
                        Learn(UniqueSpace, UniqueSpace.Possibilities[i].Number, false, IsTrying);
                        Act(UniqueSpace, IsTrying);

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
            /*
             * Use the locations of the affected spaces
             * found in the previous state, to look for
             * singles and uniques. We're only using the
             * locations because the data is not linked to
             * the actual board.
             */
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

                            Learn(SingleSpace, SingleSpace.Possibilities[0].Number, false, IsTrying);
                            Act(SingleSpace, IsTrying);

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
                                if (BSp.Possibilities[p_index].IsUnique)
                                {
                                    UniqueSpace = BSp;
                                    UniqueIndex = p_index;

                                    //then you've found a unique number. Act on it.
                                    Learn(UniqueSpace, UniqueSpace.Possibilities[UniqueIndex].Number, false, IsTrying);
                                    Act(UniqueSpace, IsTrying);

                                    //Cleanup
                                    SingleSpace = null;
                                    UniqueSpace = null;
                                    return true;
                                }
                            }
                        }
                    }
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
                    CurrentState = TryStack[TryI];
                } while (TryI >= 1 && !TryStack[TryI--].GuessState);
                //By this point the tryState is undone.
                //Now, erase all of the states that were undone.

                /*
                 * Because of short-circuiting, TryI, as 0, will
                 * exit the loop being 0. Being checked as x >= 1,
                 * however, will exit being x - 1.
                 * 
                 * In other words, if TryI is anything other than
                 * 0 when it's compared, it will leave referring
                 * to the index BEFORE the trystate.
                 */
                int deleteIndex = TryStack.IndexOf(CurrentState);
                TryStack.RemoveRange(deleteIndex + 1, TryStack.Count - (deleteIndex + 1));
            }
        }

        /// <summary>
        /// Picks a number from the CurrentState's UsedSpace
        /// </summary>
        private void Guess()
        {
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
            if (CurrentState.GuessState)//i.e. If you came back because your last guess didn't work...
            {
                int index = CurrentState.GuessIndex + 1;

                //If there are more possibilities...
                if (index < CurrentState.UsedSpace.Possibilities.Count)
                {
                    //Remove this version of the state
                    TryStack.Remove(CurrentState);
                    //Try them immediately
                    Learn(CurrentState.UsedSpace, CurrentState.UsedSpace.Possibilities[index].Number, true, true, true, index);
                    Act(GameBoard.Spaces[CurrentState.UsedSpace.TableLocation.X, CurrentState.UsedSpace.TableLocation.Y], true);
                }
                else
                {
                    /*
                     * This is for when mutiple tries have been
                     * made, and the most recent one has no
                     * good choices. You must back-track to the
                     * next-made choice.
                     * 
                     * Because the most recent TryState is
                     * still on the stack, it must be removed,
                     * or else Undo() won't work.
                     */
                    GameBoard.Squares[CurrentState.SquareLocation.X, CurrentState.SquareLocation.Y].OutOfGuesses = true;
                    TryStack.Remove(CurrentState);
                    Undo();
                    FindAvailableGuess();
                }
            }
            else//i.e. This is your first time guessing
            {
                if (SmallestSquare.OutOfGuesses)
                {
                    FindAvailableGuess();
                }
                Learn(SmallestSpace, SmallestSpace.Possibilities[0].Number, true, true);
                Act(SmallestSpace, true);
                //TODO: Find a wway to keep smallest space from choosing the same square constantly
            }
            IsTakingAGuess = false;
            //Cleanup
            UniqueSpace = null;
            SingleSpace = null;
        }

        /// <summary>
        /// Runs through the PossieSizeSorted list to find
        /// a square that still has untried guesses
        /// </summary>
        private void FindAvailableGuess()
        {
            int index = 0/*PossieSizeSorted.IndexOf(SmallestSquare)*/;
            while (index < PossieSizeSorted.Count - 1 &&
               (PossieSizeSorted[index].AveragePossibilitySize == 0.0 ||
                PossieSizeSorted[index].OutOfGuesses ||
                PossieSizeSorted[index] == SmallestSquare))
            { index++; }
            SmallestSquare = PossieSizeSorted[index];

            foreach (Space s in SmallestSquare.Spaces)
            {
                if (s.Possibilities.Count < SmallestSpace.Possibilities.Count && s.Possibilities.Count > 0)
                {
                    SmallestSpace = s;
                }
                else if (s.Possibilities.Count > SmallestSpace.Possibilities.Count && s.HasUnique())
                {
                    SmallestSpace = s;
                }
                else if (SmallestSpace.Possibilities.Count == 0)
                {
                    SmallestSpace = s;
                }
            }
        }

        /// <summary>
        /// Creates a new state based off the current space
        /// information and sets the RecentState and
        /// CurrentState to this new state
        /// </summary>
        /// <param name="sp">the Space in question</param>
        /// <param name="ChosenNumber">The number chosen for the Space</param>
        /// <param name="isGuess">Is this a state wherein the AI is guessing?</param>
        /// <param name="trying">Is this AI in the trying state?</param>
        /// <param name="addToStack">Will the AI add a new state to the TryStack?</param>
        /// <param name="GuessIndex">If guessing, what is the guessing number's possibility index?</param>
        private void Learn(Space sp, string ChosenNumber, bool isGuess, bool trying = false, bool addToStack = true, int GuessIndex = 0)
        {
            if (addToStack)
            {
                States.Add(new State(this, sp, ChosenNumber, isGuess, States.Count, CurrentState, GuessIndex));
                if (trying)
                    TryStack.Add(States[States.Count - 1]);
            }
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
            ChosenSpace.SetNumber(CurrentState.ChosenNumber, !IsATry);
            ChosenSpace.Possibilities.Clear();
            if (AgentActed != null)
                AgentActed(this, ChosenSpace);
        }

        /// <summary>
        /// Finds the Square with the lowest Average possibility size and smallestSpace
        /// </summary>
        private void FindLowestAveragePossieSize(Square sender)
        {
            int iswitch = 0;
            if (PossieSizeSorted.Count > 0)
            {
                while (iswitch < PossieSizeSorted.Count - 1 &&
                    (PossieSizeSorted[iswitch] == SmallestSquare || PossieSizeSorted[iswitch].AveragePossibilitySize == 0.0))
                { iswitch++; }
                /*
                 * In some cases, this will make it intentionally
                 * choose a worse square, but will prevent it
                 * from constantly choosing the same square.
                */
                SmallestSquare = PossieSizeSorted[iswitch];
                SmallestSpace = SmallestSquare.Spaces[0, 0];
                foreach (Space s in SmallestSquare.Spaces)
                {
                    if (s.Possibilities.Count < SmallestSpace.Possibilities.Count && s.Possibilities.Count > 0)
                    {
                        SmallestSpace = s;
                    }
                    else if (s.Possibilities.Count > SmallestSpace.Possibilities.Count && s.HasUnique())
                    {
                        SmallestSpace = s;
                    }
                    else if (SmallestSpace.Possibilities.Count == 0)
                    {
                        SmallestSpace = s;
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
            //If the space has no possibilities and a number has not been placed...

            foreach (Square sq in GameBoard.Squares)
            {
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        if (sq.Spaces[col, row].Possibilities.Count == 0 && sq.Spaces[col, row].ChosenNumber == "")
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Uses the TryStack List to finalize all the pieces on the board.
        /// </summary>
        private void FinishBoard()
        {
            for (int i = TryStack.Count - 1; i >= 0; i--)
            {
                Space sp = GameBoard.Spaces[TryStack[i].UsedSpace.TableLocation.X,
                    TryStack[i].UsedSpace.TableLocation.Y];
                sp.IsAbsolute = true;
                TryStack.RemoveAt(i);
            }
            Running = false;
        }
    }


}