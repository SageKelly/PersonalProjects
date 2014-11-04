using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TimeKeeper
{
    /// <summary>
    /// Holds a timespan for a working shift
    /// </summary>
    class TimeEntry
    {
        /// <summary>
        /// Denotes how much time was spent
        /// </summary>
        public TimeSpan timeSpent;

        /// <summary>
        /// The comment for the time spent
        /// </summary>
        public StringBuilder comment;

        /// <summary>
        /// Represents the date this TimeEntry was made
        /// </summary>
        public DateTime started;

        /// <summary>
        /// Represents the date at which this thi TimeEntry was ended
        /// </summary>
        public DateTime ended;

        /// <summary>
        /// Determines whether or not this Time entry is count toward the Total Time
        /// </summary>
        public bool marked;

        /// <summary>
        /// Marker for if it must be combined with another TimeEntry
        /// </summary>
        public bool combine;

        /// <summary>
        /// Create a TimeEntry object
        /// </summary>
        private TimeEntry()
        {
            timeSpent = TimeSpan.Zero;
            comment = new StringBuilder(20);
            combine = false;
            marked = true;
        }

        /// <summary>
        /// Creates a TimeEntry object
        /// </summary>
        /// <param name="timeSpent">How much time was spent during time span</param>
        public TimeEntry(DateTime startingTime)
            : this()
        {
            started = startingTime;
        }
    }


    class Program
    {
        #region VARIABLES
        static Thread InputWatcher, DayWatcher;
        static ConsoleKeyInfo CKI;
        static Stopwatch Timer;
        static List<TimeEntry> Times;
        static int listIndex = 0;
        static TimeSpan curTime = TimeSpan.Zero, prevTime = curTime;
        /// <summary>
        /// Represents if the Timer's minutes or hours changed
        /// </summary>
        static bool TimerChanged;
        static bool newDayOccurred = false;
        /// <summary>
        /// Represents the currently selected in the Times list.
        /// In this program, it assumes the newest index before it exists.
        /// Therefore, if Times has 3 entries, ListIndex, at that point would equal 3.
        /// </summary>
        static int ListIndex
        {
            get
            {
                return listIndex;
            }
            set
            {
                listIndex = value;
                Console.CursorTop = listIndex + TEXT_COUNT;
            }
        }

        #region consts
        /// <summary>
        /// Represents the last line the Console's cursor before
        /// a method manipulated it.
        /// </summary>
        static int LastLine = 0;

        /// <summary>
        /// The Left location for the comments column for a TimeInfo
        /// </summary>
        const int COMMENT_LEFT = 16;

        static int COMMENT_PAD = Console.BufferWidth - COMMENT_LEFT;

        /// <summary>
        /// The Top location for the program's status. Also used for Total Time
        /// </summary>
        const int STATUS_TOP = 6;

        /// <summary>
        /// The Left location for the program's status.
        /// </summary>
        const int STATUS_LEFT = 8;

        const int STATUS_PAD = 7;

        /// <summary>
        /// How many lines before TimeEntries are written.
        /// </summary>
        const int TEXT_COUNT = 8;

        /// <summary>
        /// The Left location for the Total Time
        /// </summary>
        const int TOTAL_LEFT = 16;

        /// <summary>
        /// The Left locations of the cursor for a TimeEntry mark
        /// </summary>
        const int MARK_LEFT = 0;

        /// <summary>
        /// The Left location of the cursor for a TimeEntry
        /// </summary>
        const int TIME_LEFT = 1;

        /// <summary>
        /// Holds the tab escape sequences for printing
        /// </summary>
        const int PRINT_TABS = 11/*two tabs*/;

        /// <summary>
        /// Represents the cursor's top location for the instructions
        /// </summary>
        const int INSTRUCTIONS_TOP = 1;//No left needed

        /// <summary>
        /// Represents how much console padding should happen
        /// on certain printing instructions
        /// </summary>
        const int PAD_AMOUNT = 39;
        #endregion

        #region Printing Booleans
        private static bool printCombineMark = false;
        private static bool printInstructions = false;
        private static bool printMark = false;
        private static bool printTime = false;
        private static bool printScreen = false;
        private static bool printStatus = false;
        private static bool printAllTimes = false;
        private static bool printTotalTime = false;
        private static bool saveTimes = false;
        #endregion

        static TimeSpan TotalTimeSpan;

        static ConsoleColor CombineOnColor;
        static ConsoleColor CombineOffColor;
        static ConsoleColor MarkOnColor;
        static ConsoleColor MarkOffColor;

        static string MarkChar, CombineChar;

        enum States
        {
            Off,
            Watch,
            Edit,
            Combine,
            Mark,
            Comment,
            Save,
            Exit
        }
        private static States programState;
        private static States ProgramState
        {
            get
            {
                return programState;
            }
            set
            {
                programState = value;
                printStatus = true;
                printInstructions = true;
            }
        }
        private static States PrevState;

        /// <summary>
        /// Represents the day this program was initialized
        /// </summary>
        private static DateTime StartingDate;

        /// <summary>
        /// Represents the first-most possible index for current TimeEntry combining
        /// </summary>
        static int CombineIndexFirst;

        /// <summary>
        /// Represents the last-most possible index for current TimeEntry combining
        /// </summary>
        static int CombineIndexLast;
        #endregion

        static void Main(string[] args)
        {
            Console.Title = "Time Keeper";
            ///Start the threads
            InputWatcher = new Thread(ReadKeys);
            DayWatcher = new Thread(ClockAndPrintWatcher);

            ///Set special colors
            CombineOnColor = ConsoleColor.Cyan;
            CombineOffColor = ConsoleColor.DarkGray;
            MarkOnColor = ConsoleColor.Green;
            MarkOffColor = ConsoleColor.Red;

            ///Set speical marking characters
            MarkChar = "*";
            CombineChar = "+";

            CKI = new ConsoleKeyInfo();

            Times = new List<TimeEntry>();
            ListIndex = Times.Count - 1;
            ResetCombineIndices();

            ///Recoard the day this program started
            StartingDate = DateTime.Now;

            Timer = new Stopwatch();
            TotalTimeSpan = new TimeSpan();

            PrintScreen();
            ProgramState = States.Off;
            DayWatcher.Start();
            InputWatcher.Start();

        }

        /// <summary>
        /// Adds a new TimeInfo instance to the list of TimeInfo objects.
        /// </summary>
        public static void AddNewTime()
        {
            if (Times.Count > 0)
            {
                Timer.Stop();
                Times[Times.Count - 1].timeSpent = Timer.Elapsed;
                Times[Times.Count - 1].ended = DateTime.Now;
                TotalTimeSpan += Timer.Elapsed;
                Times.Add(new TimeEntry(DateTime.Now));
                Times[Times.Count - 1].started = DateTime.Now;
                Timer.Restart();
            }
            else
            {
                Times.Add(new TimeEntry(DateTime.Now));
                Times[Times.Count - 1].started = DateTime.Now;
                Timer.Start();
            }
        }

        /// <summary>
        /// Combines all TimeEntries marked for combining and keeps the first-most's comment
        /// </summary>
        public static void CombineMarked()
        {
            TimeEntry mainEntry;
            int CombineIndex;
            if (CombineIndexFirst == 0 && Times[CombineIndexFirst].combine)
            {
                mainEntry = Times[CombineIndexFirst];
            }
            else
            {
                mainEntry = Times[CombineIndexFirst + 1];
            }

            if (CombineIndexLast == Times.Count - 1 && Times[CombineIndexLast].combine)
            {
                CombineIndex = CombineIndexLast;
            }
            else
            {
                CombineIndex = CombineIndexLast - 1;
            }
            while (Times[CombineIndex].combine && CombineIndex != Times.IndexOf(mainEntry))
            {
                mainEntry.timeSpent += Times[CombineIndex].timeSpent;
                Times.RemoveAt(CombineIndex);
                CombineIndex--;
            }

        }

        /// <summary>
        /// Checks to see if any TimeEntries are marked for combining
        /// </summary>
        /// <returns>returns true if there are, else false</returns>
        private static bool NoCombineMarks()
        {
            #region Logic behind method
            /*
             * Since the CombineIndexFirst and ""Last variables are going
             * to keep ListIndex within their bounds, and grow/shrink each
             * time the TimeEntry at ListIndex is toggled on/off, that means
             * that those variables, if there's only one marked, will be
             * directly next to, or equalling, ListIndex. Therefore, you only
             * need to check, at most, three indices.
             */
            #endregion
            if (!Times[ListIndex].combine)
            {
                //If not at 0, and above location not combine
                if (ListIndex - 1 > -1 && !Times[ListIndex - 1].combine)
                {
                    //If not at end and location below not combine
                    if (ListIndex + 1 < Times.Count - 1 && !Times[ListIndex + 1].combine)
                    {
                        return true;
                    }
                }
                //If you made it here, then there was only one entry
                else if (Times.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deccrements both ListIndex and Console.CursorTop
        /// </summary>
        public static void DecListAndTop()
        {
            ListIndex--;
        }

        /// <summary>
        /// Deletes the TimeEntry instance at ListIndex
        /// </summary>
        private static void DeleteTime()
        {
            if (ListIndex < Times.Count && Times.Count != 0)
            {
                //Clear the last line off the buffer
                int temp = ListIndex;
                SetToBottom();
                DecListAndTop();
                Console.Write("".PadRight(Console.BufferWidth - 1));
                ListIndex = temp;
                Console.CursorLeft = 0;
                Timer.Stop();
                Times.RemoveAt(ListIndex);
                Timer.Restart();

                if (ListIndex == Times.Count && ListIndex > 0)//If you deleted the last TimeEntry
                    DecListAndTop();
            }
        }

        /// <summary>
        /// Find the first-most and last-most possible TimeEntries can be current combined.
        /// </summary>
        public static void FindCombineEdges()
        {
            if (ListIndex != 0 && !Times[ListIndex - 1].combine)
            {
                CombineIndexFirst = ListIndex - 1;
            }
            else
            {
                while (!Times[CombineIndexFirst].combine)
                {
                    CombineIndexFirst++;
                }
            }
            if (ListIndex < Times.Count - 1 && !Times[ListIndex + 1].combine)
            {
                CombineIndexLast = ListIndex + 1;
            }
            else
            {
                while (!Times[CombineIndexLast].combine)
                {
                    CombineIndexLast--;
                }
            }
        }

        /// <summary>
        /// Increments both ListIndex and Console.CursorTop
        /// </summary>
        public static void IncListAndTop()
        {
            ListIndex++;
        }

        /// <summary>
        /// Uses ListIndex to print the Combine mark for the TimeEntry
        /// </summary>
        public static void PrintCombineMark()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = MARK_LEFT;
            Console.ForegroundColor = Times[ListIndex].combine ? CombineOnColor : CombineOffColor;
            Console.Write(CombineChar);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Prints case-related instructions
        /// </summary>
        private static void PrintInstructions()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            string l1p1 = "", l1p2 = "", l2p1 = "", l2p2 = "", l3p1 = "", l3p2 = "";
            Console.CursorTop = INSTRUCTIONS_TOP;
            Console.WriteLine("INSTRUCTIONS:");
            switch (ProgramState)
            {
                //If necessary apply '\t' only to part 1
                //Space, Escape, Delete, E, R, S, P, M, C, Up/Down, Enter
                #region Instruction comments
                /*
                 * C: C to enter Combine Mode
                 * C: C to mark the end or range, then the start
                 * C: C to mark a combine a range of contiguous entries
                 * Delete: Delete to delete an entry
                 * E: E to enter Edit Mode
                 * E: E to exit Edit Mode
                 * Enter: Enter to Create/Edit Comment
                 * Enter: Enter to Confirm Changes
                 * Enter: Enter to confirm Combine range
                 * Enter: Enter to leave Mark Mode
                 * Escape: Escape to Save/Exit
                 * Escape: Escape to leave Combine Mode without combining
                 * M: M to enter Mark Mode
                 * M: M to mark/unmark an entry to count toward total time
                 * P: P to pause current timer
                 * P: P to start/resume current timer
                 * R: R to refresh
                 * S: S to save
                 * Space: Space to add and start new timer
                 * Up/Down Arrows: Up/Down Arrows to move
                 */
                #endregion
                case States.Combine:
                    l1p1 = "Up/Down Arrows to move,";
                    l2p1 = "C to mark the range: end then start";
                    l2p2 = "Enter to confirm Combine range,";
                    l3p1 = "Escape to leave without combining";
                    break;
                case States.Comment:
                    l1p1 = "Enter to Confirm Changes";
                    l2p1 = "";
                    l3p1 = "";
                    break;
                case States.Edit:
                    l1p1 = "C to enter Combine Mode (nope),";
                    l1p2 = "Delete to delete an entry";
                    l2p1 = "E to exit Edit Mode,";
                    l2p2 = "Enter to Create/Edit Comment,";
                    l3p1 = "M to enter Mark Mode,";
                    l3p2 = "Up/Down Arrows to move, S to Save";
                    break;
                case States.Mark:
                    l1p1 = "Up/Down Arrows to move,";
                    l1p2 = "M to mark/unmark an entry";
                    l2p1 = "to count toward total time,";
                    l2p2 = "R to refresh,";
                    l3p1 = "Enter to leave Mark Mode";
                    break;
                case States.Off:
                    l1p1 = "P to Pause/Unpause,";
                    l1p2 = "R to refresh,";
                    l2p1 = "E to enter Edit Mode,";
                    l2p2 = "S to save,";
                    l3p1 = "Escape to Save/Exit";
                    break;
                case States.Save:
                    l1p1 = "Follow the";
                    l2p1 = "on-screen";
                    l3p1 = "instructions";
                    break;
                case States.Watch:
                    l1p1 = "Space to Add a record time,";
                    l1p2 = "P to Pause/Unpause,";
                    l2p1 = "R to refresh,";
                    l2p2 = "E to enter Edit Mode,";
                    l3p1 = "S to save,";
                    l3p2 = "Escape to Save/Exit";
                    break;
            }
            /*
             * To explain the seemingly random numbers in the PadRight methods:
             * 51 is the longest possible line so far
             * 14 is the extra spaces in the \t that aren't counted...for some reason
             */
            Console.WriteLine(
                l1p1.PadRight(PAD_AMOUNT) + l1p2.PadRight(PAD_AMOUNT) + "\n" +
                l2p1.PadRight(PAD_AMOUNT) + l2p2.PadRight(PAD_AMOUNT) + "\n" +
                l3p1.PadRight(PAD_AMOUNT) + l3p2.PadRight(PAD_AMOUNT) + "\n" +
                "------------------------------------".PadRight(Console.BufferWidth));
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Uses ListIndex to print a TimeEntry's mark pip.
        /// </summary>
        public static void PrintMark()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorLeft = MARK_LEFT;
            Console.ForegroundColor = Times[ListIndex].marked ? MarkOnColor : MarkOffColor;
            Console.Write(MarkChar);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Uses ListIndex to print a TimeEntry to the screen
        /// </summary>
        public static void PrintTime()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            PrintMark();
            Console.CursorLeft = TIME_LEFT;
#if DEBUG
            Console.WriteLine("{0}:{1}{2}",
                Times[ListIndex].timeSpent.Minutes,
                Times[ListIndex].timeSpent.Seconds.ToString().PadRight(PRINT_TABS),
                Times[ListIndex].comment.ToString().PadRight(COMMENT_PAD));
#else
            Console.WriteLine("{0}:{1}{2}",
                Times[ListIndex].timeSpent.Hours,
                Times[ListIndex].timeSpent.Minutes.ToString().PadRight(PRINT_TABS),
                Times[ListIndex].comment.ToString().PadRight(COMMENT_PAD));
#endif
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Clears and prints the entire screen
        /// </summary>
        public static void PrintScreen()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("TIME KEEPER");
            Console.ForegroundColor = ConsoleColor.White;
            PrintInstructions();
            PrintStatus();
            PrintTotalTime();
            Console.WriteLine();//Just to add a new line

            PrintAllTimes();
            SetToBottom();
        }

        /// <summary>
        /// Updates the program status section of the screen
        /// </summary>
        public static void PrintStatus()
        {
            //Cursor: 2,8   
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorTop = STATUS_TOP;
            Console.CursorLeft = 0;
            Console.Write("Status: ");
            switch (ProgramState)
            {
                case States.Combine:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case States.Watch:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case States.Edit:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case States.Comment:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case States.Save:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case States.Off:
                case States.Exit:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.Write(ProgramState.ToString().PadRight(7));
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Prints all Time Entries
        /// </summary>
        public static void PrintAllTimes()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.SetCursorPosition(0, 7);
            Console.WriteLine("Time Spent\tComment");

            //Print all the times and their associated marker
            foreach (TimeEntry TI in Times)
            {
                if (programState == States.Combine)
                {
                    Console.ForegroundColor = TI.combine ? CombineOnColor : CombineOffColor;
                    Console.Write(CombineChar);
                }
                else
                {
                    Console.ForegroundColor = TI.marked ? MarkOnColor : MarkOffColor;
                    Console.Write(MarkChar);
                }
                Console.ForegroundColor = ConsoleColor.White;
#if DEBUG
                Console.WriteLine("{0}:{1}{2}",
                    TI.timeSpent.Minutes.ToString().PadRight(2,'0'), TI.timeSpent.Seconds.ToString().PadRight(PRINT_TABS), TI.comment.ToString().PadRight(COMMENT_PAD));
#else
                Console.WriteLine("{0}:{1}{2}",
                    TI.timeSpent.Hours, TI.timeSpent.Minutes.ToString().PadRight(PRINT_TABS), TI.comment.ToString().PadRight(COMMENT_PAD));
#endif
            }
            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Updates the Total Time section of the screen
        /// </summary>
        public static void PrintTotalTime()
        {
            TallyTime();
            //Cursor: 2,16
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.CursorTop = STATUS_TOP;
            Console.CursorLeft = TOTAL_LEFT;

            //Console.Write("Total Time: {0}:{1} ", TotalTimeSpan.Hours, TotalTimeSpan.Minutes);
            Console.Write("Total Time: {0}".PadRight(PAD_AMOUNT), TotalTimeSpan.Duration());

            Console.SetCursorPosition(left, top);
        }

        /// <summary>
        /// Reads particular key inputs for UI controls
        /// </summary>
        public static void ReadKeys()
        {
            while (ProgramState != States.Exit)
            {
                switch (ProgramState)
                {
                    #region CKI Program States
                    case States.Off:
                    case States.Watch:
                    case States.Edit:
                    case States.Combine:
                    case States.Mark:
                        CKI = Console.ReadKey(true);
                        break;
                    case States.Comment:
                        CKI = Console.ReadKey();
                        switch (CKI.Key)
                        {
                            case ConsoleKey.Backspace:
                                if (Times[ListIndex].comment.Length > 0)
                                {
                                    Console.Write(" ");
                                    Console.CursorLeft--;
                                    Times[ListIndex].comment.Remove(Times[ListIndex].comment.Length - 1, 1);
                                }
                                break;
                            default:
                                Times[ListIndex].comment.Append(CKI.KeyChar);
                                break;
                        }
                        break;
                    #endregion
                }

                switch (CKI.Key)
                {
                    case ConsoleKey.Backspace:

                        break;
                    #region Delete
                    case ConsoleKey.Delete://Delete a TimeInfo
                        /*    
                        switch (ProgramState)
                            {
                                case States.Edit:
                                    LastLine = Console.CursorTop;
                                    DeleteTime();
                                    TallyTime();
                                    printTotalTime = true;
                                    printAllTimes = true;
                                    break;
                            }
                            */
                        break;
                    #endregion
                    #region Enter
                    case ConsoleKey.Enter:
                        switch (ProgramState)
                        {
                            case States.Combine:
                                //TODO: Get combine marking working
                                CombineMarked();
                                ResetCombineIndices();
                                printScreen = true;
                                ProgramState = PrevState;
                                break;
                            case States.Comment:
                                //finished writing/editing
                                ProgramState = States.Edit;
                                break;
                            case States.Edit:
                                if (ListIndex < Times.Count && Times.Count != 0)
                                {
                                    Console.CursorLeft = COMMENT_LEFT;
                                    //Remove the comment from the screen buffer
                                    int spaces = Times[ListIndex].comment.Length;
                                    for (int i = 0; i < spaces; i++)
                                        Console.Write(" ");
                                    Console.CursorLeft = COMMENT_LEFT;

                                    Times[ListIndex].comment.Clear();

                                    //begin editing
                                    ProgramState = States.Comment;
                                }
                                break;
                            case States.Mark:
                                ProgramState = States.Edit;
                                break;
                            case States.Save:
                                ProgramState = States.Exit;
                                break;
                        }
                        break;
                    #endregion
                    #region Spacebar
                    case ConsoleKey.Spacebar:
                        switch (ProgramState)
                        {
                            case States.Watch:
                                AddNewTime();
                                ListIndex++;
                                printTime = true;
                                printTotalTime = true;
                                break;
                        }
                        break;
                    #endregion
                    #region Escape
                    case ConsoleKey.Escape://Close program
                        switch (ProgramState)
                        {
                            case States.Off:
                            case States.Watch:
                                //write out times/comments to file
                                if (Times.Count > 0)
                                {
                                    ProgramState = States.Save;
                                    SetToBottom();
                                    LastLine = Console.CursorTop;
                                    saveTimes = true;
                                }
                                ProgramState = States.Exit;
                                break;
                            case States.Combine:
                                ProgramState = States.Edit;
                                break;
                        }
                        break;
                    #endregion
                    #region C: Combine Times
                    /*
                    case ConsoleKey.C:
                        switch (ProgramState)
                        {
                            case States.Edit:
                                if (Times.Count > 0)
                                {
                                    ProgramState = States.Combine;
                                    ResetCombineIndices();
                                    PrintScreen();
                                    SetToBottom();
                                }
                                break;
                            case States.Combine:
                                if (ListIndex < Times.Count && Times.Count != 0)
                                {
                                    if (Times[ListIndex].Combine ||
                                        (!Times[CombineIndexFirst].Combine || !Times[CombineIndexLast].Combine))
                                    {
                                        int Last = Times.Count - 1;
                                        Times[ListIndex].Combine = !Times[ListIndex].Combine;
                                        if (Times[ListIndex].Combine)
                                        {
                                            if (CombineIndexLast ==Last)
                                            {
                                                CombineIndexLast = ListIndex;
                                            }
                                            else if (CombineIndexFirst == Last)
                                            {
                                                CombineIndexFirst = ListIndex;
                                            }

                                        }
                                        else
                                        {
                                            if (!Times[CombineIndexFirst].Combine)
                                            {
                                                CombineIndexFirst = Times.Count - 1;
                                            }
                                            else if (!Times[CombineIndexLast].Combine)
                                            {
                                                CombineIndexLast = Times.Count - 1;
                                            }
                                        }

                                        //FindCombineEdges();
                                        PrintCombineMark();
                                        Console.CursorLeft = MARK_LEFT;
                                    }
                                }
                                break;
                        }
                        break;
                        */
                    #endregion
                    #region E: Edit
                    case ConsoleKey.E://Pause/edit 
                        switch (ProgramState)
                        {
                            case States.Off:
                                ProgramState = States.Edit;
                                PrevState = States.Off;
                                break;
                            case States.Watch:
                                ProgramState = States.Edit;
                                PrevState = States.Watch;
                                break;
                            case States.Edit:
                                ProgramState = PrevState;
                                SetToBottom();
                                break;
                        }
                        break;
                    #endregion
                    #region M: Marking
                    case ConsoleKey.M:
                        switch (ProgramState)
                        {
                            case States.Edit:
                                ProgramState = States.Mark;
                                break;
                            case States.Mark:
                                if (ListIndex < Times.Count && Times.Count != 0)
                                {
                                    Times[ListIndex].marked = !Times[ListIndex].marked;
                                    printMark = true;
                                    Console.CursorLeft = MARK_LEFT;
                                    TallyTime();
                                    printTotalTime = true;
                                }
                                break;
                        }
                        break;
                    #endregion
                    #region P: Pause/Unpause
                    case ConsoleKey.P:
                        switch (ProgramState)
                        {
                            case States.Off:
                                if (Times.Count != 0)
                                    Timer.Start();
                                ProgramState = States.Watch;
                                break;
                            case States.Watch:
                                if (Times.Count != 0)
                                    Timer.Stop();
                                ProgramState = States.Off;
                                break;
                        }
                        break;
                    #endregion
                    #region R: Refresh
                    case ConsoleKey.R:
                        switch (ProgramState)
                        {
                            case States.Off:
                            case States.Watch:
                            case States.Mark:
                            case States.Edit:
                                Console.Clear();
                                int left = Console.CursorLeft;
                                LastLine = Console.CursorTop;
                                int index = ListIndex;
                                printScreen = true;
                                Console.SetCursorPosition(left, LastLine);
                                ListIndex = index;
                                break;
                        }
                        break;
                    #endregion
                    #region S: Save
                    case ConsoleKey.S:
                        switch (ProgramState)
                        {
                            case States.Off:
                            case States.Watch:
                                //write out times/comments to file
                                if (Times.Count > 0)
                                {
                                    PrevState = ProgramState;
                                    ProgramState = States.Save;
                                    SetToBottom();
                                    LastLine = Console.CursorTop;
                                    saveTimes = true;
                                    SetToBottom();
                                    ProgramState = PrevState;
                                }
                                break;
                        }
                        break;
                    #endregion
                    #region Arrows
                    #region Up/Down
                    case ConsoleKey.UpArrow:
                        switch (ProgramState)
                        {
                            case States.Combine:
                            /*
                            if (ListIndex > CombineIndexFirst)
                            {
                                DecListAndTop();
                            }
                            break;
                            */
                            case States.Mark:
                            case States.Edit:
                                if (ListIndex > 0)
                                    DecListAndTop();
                                break;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        switch (ProgramState)
                        {
                            case States.Combine:
                            /*
                            if (ListIndex < CombineIndexLast)
                            {
                                IncListAndTop();
                            }
                            break;
                            */
                            case States.Mark:
                            case States.Edit:
                                if (ListIndex < Times.Count - 1)
                                    IncListAndTop();
                                break;
                        }
                        break;
                    #endregion
                    #region Left/Right: unnecessary
                    case ConsoleKey.LeftArrow:
                        switch (ProgramState)
                        {
                            case States.Combine:
                            case States.Edit:
                                //Set Console.CursorLeft to specific values for the console
                                break;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        switch (ProgramState)
                        {
                            case States.Combine:
                            case States.Edit:
                                //Set Console.CursorLeft to specific values for the console
                                break;
                        }
                        break;
                    #endregion
                    #endregion
                }
            }
        }

        /// <summary>
        /// Sets the outer bounds of the the combine variables
        /// to each end of the list of TimeEntries
        /// </summary>
        private static void ResetCombineIndices()
        {
            CombineIndexFirst = 0;
            CombineIndexLast = Times.Count - 1;
        }

        /// <summary>
        /// Stops the Timer and adds new TimeEntry
        /// </summary>
        private static void SaveTimes()
        {
            //Stop the timer
            Timer.Stop();
            bool ValidInput = true;

            #region Do you want to save your latest time?
            do
            {
                Console.CursorTop = LastLine + 1;
                Console.WriteLine("Do you want to save your latest time? (y/n)".PadRight(Console.BufferWidth - 1));
                if (!ValidInput)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nInvalid Input: try again.".PadRight(Console.BufferWidth - 1));
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.CursorTop = LastLine + 2;
                }

                CKI = Console.ReadKey(true);
                //to clean the screen
                Console.CursorTop = LastLine + 1;
                if (ValidInput)
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }
                else
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }

                switch (CKI.Key)
                {
                    case ConsoleKey.Y:
                        Times[Times.Count - 1].ended = DateTime.Now;
                        Times[Times.Count - 1].timeSpent = Timer.Elapsed;
                        ListIndex++;
                        ValidInput = true;
                        break;
                    case ConsoleKey.N:
                        Times.RemoveAt(Times.Count - 1);
                        ValidInput = true;
                        break;
                    default:
                        ValidInput = false;
                        break;
                }
            } while (!ValidInput);
            #endregion
            printScreen = true;
            #region Do you want save the time sheet?
            do
            {
                Console.CursorTop = LastLine + 1;
                Console.WriteLine("Do you want to save? (y/n)".PadRight(Console.BufferWidth - 1));
                if (!ValidInput)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nInvalid Input: try again.".PadRight(Console.BufferWidth - 1));
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.CursorTop = LastLine + 2;
                }
                CKI = Console.ReadKey(true);

                //to clean the screen
                Console.CursorTop = LastLine + 1;
                if (ValidInput)
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }
                else
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }


                switch (CKI.Key)
                {
                    case ConsoleKey.Y:
                        ValidInput = true;
                        break;
                    case ConsoleKey.N:
                        ValidInput = true;
                        return;
                    default:
                        ValidInput = false;
                        break;
                }
            } while (!ValidInput);
            #endregion
            #region Input Path
            do
            {
                Console.CursorTop = LastLine + 1;
                Console.WriteLine("Where do you want to save? (without the file name)".PadRight(Console.BufferWidth - 1));
                if (!ValidInput)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nInvalid Input: try again.".PadRight(Console.BufferWidth - 1));
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.CursorTop = LastLine + 2;
                }
                Console.Write("".PadRight(Console.BufferWidth - 1));
                Console.CursorLeft = 0;
                string input = Console.ReadLine();
                //Check the directory to see if it's valid

                //to clean the screen
                Console.CursorTop = LastLine + 1;
                if (ValidInput)
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }
                else
                {
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                    Console.WriteLine("".PadRight(Console.BufferWidth));
                }

                if (Directory.Exists(input))//if so...
                {
                    //...make the file, and write it there.
                    WriteFile(input);

                    ValidInput = true;
                }
                else
                {
                    ValidInput = false;
                }

            } while (!ValidInput);
            #endregion
        }

        /// <summary>
        /// Sets the Cursor's Top location to the last line in the console.
        /// </summary>
        public static void SetToBottom()
        {
            ListIndex = Times.Count - 1;
        }

        /// <summary>
        /// Takes quick inventory of the total time spent so far.
        /// Useful when deleting TimeEntries.
        /// </summary>
        public static void TallyTime()
        {
            if (Times.Count != 0)
            {
                TotalTimeSpan = TimeSpan.Zero;
                foreach (TimeEntry TI in Times)
                {
                    if (TI.marked)
                        TotalTimeSpan += TI.timeSpent;
                }
                if (Times[Times.Count - 1].marked)
                    TotalTimeSpan += Timer.Elapsed;
            }
        }

        /// <summary>
        /// Watches the day and the Timer StopWatch
        /// </summary>
        public static void ClockAndPrintWatcher()
        {
            while (ProgramState != States.Exit)
            {
                WatchForNewDay();
                WatchTimer();
                WatchForPrintTasks();
            }
        }

        /// <summary>
        /// Keeps track of the change of day, and adds a new TimeEntry when it happens
        /// </summary>
        public static void WatchForNewDay()
        {
            DateTime Previous = DateTime.Now, Now = DateTime.Now;
            string newDay = "New Day!";
            Now = DateTime.Now;
            if (Now.Day != Previous.Day)
            {
                newDayOccurred = true;
                AddNewTime();
                PrintTotalTime();
                int length = Times.Count - 1;//Last TimeEntry
                if (length > 0)
                {
                    //Set a char array equal to the length of the second-to-last time entry's comment +
                    //the length of "new Day!" + 1 for a space
                    char[] temp = new char[Times[length - 1].comment.Length + newDay.Length + 1];
                    //Copy New Day! to the beginning registers of temp
                    newDay.CopyTo(0, temp, 0, newDay.Length);
                    //The register after "New Day!" should be a space: ' '
                    temp[newDay.Length] = ' ';
                    //Then, copy the full length of the second-to-last index
                    Times[length - 1].comment.CopyTo(0, temp, newDay.Length + 1, Times[length - 1].comment.Length);
                    //Finally, add the entire comment to the newest time entry
                    Times[length].comment.Append(temp);
                }
                SetToBottom();
            }
            if (newDayOccurred && ProgramState == States.Watch)
                printTime = true;
            Previous = Now;
        }

        /// <summary>
        /// Checks printing booleans set to true and does
        /// those tasks
        /// </summary>
        public static void WatchForPrintTasks()
        {
            bool printed = false;
            if (printScreen)
            {
                PrintScreen();
                printScreen = false;
                printed = true;
            }
            if (printInstructions)
            {
                PrintInstructions();
                printInstructions = false;
                printed = true;
            }
            if (printStatus)
            {
                PrintStatus();
                printStatus = false;
                printed = true;
            }
            if (printTotalTime)
            {
                PrintTotalTime();
                printTotalTime = false;
                printed = true;
            }
            if (printAllTimes)
            {
                PrintAllTimes();
                printAllTimes = false;
                printed = true;
            }
            if (printMark)
            {
                PrintMark();
                printMark = false;
                printed = true;
            }
            if (printCombineMark)
            {
                PrintCombineMark();
                printCombineMark = false;
                printed = true;
            }
            if (printTime)
            {
                PrintTime();
                printTime = false;
                printed = true;
            }
            if (saveTimes)
            {
                SaveTimes();
                saveTimes = false;
                printed = true;
            }
            if (printed)
                SetToBottom();
        }

        /// <summary>
        /// Checks for time changes on the Timer StopWatch,
        /// and prints the time accordingly
        /// </summary>
        public static void WatchTimer()
        {
            curTime = Timer.Elapsed;
#if DEBUG
            if (prevTime.Seconds != curTime.Seconds)
                TimerChanged = true;
#else
            if (prevTime.Minutes != curTime.Minutes)
                TimerChanged = true;
#endif
            switch (programState)
            {
                case States.Watch:
                    if (TimerChanged)
                    {
                        listIndex = Times.Count - 1;
                        Times[Times.Count - 1].timeSpent = Timer.Elapsed;
                        SetToBottom();
                        printTime = true;
                        TimerChanged = false;
                    }
                    break;
            }
            prevTime = curTime;
        }

        /// <summary>
        /// Writes times made to a text file at the chosen path
        /// </summary>
        /// <param name="path">The path at which to save the file</param>
        public static void WriteFile(string path)
        {
            FileStream fs;

            string temp = @path + @"/Timesheet for " +
                StartingDate.Date.Month +
                "_" + StartingDate.Day +
                "_" + StartingDate.Date.Year + ".txt";

            if (File.Exists(temp))
                fs = new FileStream(temp, FileMode.Append);
            else
                fs = new FileStream(temp, FileMode.CreateNew);

            StreamWriter sw = new StreamWriter(fs);

            if (File.Exists(temp))
                sw.WriteLine("---------------------------");

            sw.WriteLine("Program Started on {0}/{1}/{2} at {3}",
                StartingDate.Month, StartingDate.Day, StartingDate.Year, StartingDate.TimeOfDay);
            sw.WriteLine("Total Time: {0}", TotalTimeSpan.Duration());
            sw.WriteLine(" Time In\t\tTime Out\t\tTime Spent\t\t\tComment");

            foreach (TimeEntry TI in Times)
            {
                sw.WriteLine("{0}{1}\t\t\t{2}\t\t\t{3}:{4}\t\t\t\t\t{5}", TI.marked ? "M" : " ",
                    TI.started.TimeOfDay.Duration().ToString(@"hh\:mm"), TI.ended.TimeOfDay.Duration().ToString(@"hh\:mm"), TI.timeSpent.Hours, TI.timeSpent.Minutes, TI.comment);
            }
            DateTime tempEnding = DateTime.Now;
            sw.Write("Program ended on {0}/{1}/{2} at ", tempEnding.Month, tempEnding.Day, tempEnding.Year, tempEnding.TimeOfDay);
            sw.Close();

        }
    }
}
