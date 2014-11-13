using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace QuizMaker
{
    /// <summary>
    /// Handles typerwriter-like UI (CAUTION: CALL SetupTyper() FIRST!!!)
    /// </summary>
    public static class ConsUI
    {
        //Multiple choice (Lists)...............DONE
        //Yes or No (0 or 1, or true or false)..DONE
        //Info (string input)...................DONE

        //NOTE: Tabs take seven characters
        //NOTE: Escape sequences are characters themselves; they take up one register in a char array.
        static string PrintingString = "";
        static FormattedString CPrintingString;
        static int TypeRunner = 0;
        static Timer Typer;
        static Timer ColorTyper;
        static List<string> StringList;
        static ConsoleKeyInfo CKI;
        static List<FormattedString> ColorStrings;
        static bool TyperSetUp = false, ColorTyperSetUp = false, TyperStarted = false, TyperFinished = true,
            ColorTyperStarted = false, ColorTyperFinished = true;
        static int ConsoleWidth = Console.WindowWidth - 2;

        public struct FormattedString
        {
            private string s_text;
            private ConsoleColor c_text_color;

            public string Text
            {
                get
                {
                    return s_text;
                }
                set
                {
                    s_text = value;
                }
            }

            public ConsoleColor TextColor
            {
                get
                {
                    return c_text_color;
                }
                set
                {
                    c_text_color = value;
                }
            }

            public FormattedString(string text, ConsoleColor text_color)
            {
                s_text = text;
                c_text_color = text_color;
            }
        }

        public static void SetupTyper(int period)
        {
            StringList = new List<string>();
            Typer = new Timer(period);
            Typer.AutoReset = true;
            Typer.Elapsed += new ElapsedEventHandler(TypeText);
            //Typer.Elapsed+=new ElapsedEventHandler(IgnoreTyping);
            TyperSetUp = true;

            ColorStrings = new List<FormattedString>();
            ColorTyper = new Timer(period);
            ColorTyper.AutoReset = true;
            ColorTyper.Elapsed += new ElapsedEventHandler(TypeColorText);
            //ColorTyper.Elapsed+=new ElapsedEventHandler(IgnoreTyping);
            ColorTyperSetUp = true;
        }

        #region Typer Methods
        static void InterruptTyper()
        {
            Typer.Stop();
            TyperStarted = false;
            StringList.Clear();
            TyperFinished = true;
        }

        public static void SetPrintingString()
        {
            if (TyperStarted && !TyperFinished)
                return;
            Typer.Stop();
            TypeRunner = 0;
            PrintingString = StringList[0];
            Typer.AutoReset = true;
            Typer.Start();
            TyperFinished = false;
            TyperStarted = true;
        }

        private static void IgnoreTyping(object sender, ElapsedEventArgs EEA)
        {
            if (!TyperFinished || !ColorTyperFinished)
                CKI = Console.ReadKey(true);
        }

        private static void TypeText(object sender, ElapsedEventArgs EEA)
        {
            if (TypeRunner < PrintingString.Length && !ColorTyperFinished)
            {
                Console.Write(PrintingString[TypeRunner++]);
            }
            else if (TypeRunner == PrintingString.Length && !ColorTyperFinished)
            {
                Console.WriteLine();
                TypeRunner++;
                StringList.RemoveAt(0);

                if (StringList.Count != 0)
                    SetPrintingString();
                else
                {
                    Typer.Stop();
                    TyperFinished = true;
                }
            }
        }
        #endregion
        #region ColorTyper Methods
        private static void InterruptColorTyper()
        {
            ColorTyper.Stop();
            ColorStrings.Clear();
        }

        public static void SetCPrintingString()
        {
            ColorTyper.Stop();
            TypeRunner = 0;
            CPrintingString = ColorStrings[0];
            ColorTyper.AutoReset = true;
            ColorTyper.Start();
            ColorTyperFinished = false;
        }

        private static void TypeColorText(object sender, ElapsedEventArgs EEA)
        {
            if (TypeRunner < CPrintingString.Text.Length && !TyperFinished)
            {
                Console.ForegroundColor = CPrintingString.TextColor;
                Console.Write(CPrintingString.Text[TypeRunner++]);
                Console.ForegroundColor = ConsoleColor.White;//May be removed
            }
            else if (TypeRunner == CPrintingString.Text.Length && !TyperFinished)
            {
                Console.WriteLine();
                TypeRunner++;
                ColorStrings.RemoveAt(0);
                if (ColorStrings.Count != 0)
                    SetCPrintingString();
                else
                {
                    ColorTyper.Stop();
                    ColorTyperFinished = true;
                }
            }
        }
        #endregion

        public static int MultipleChoiceStr(string IntroText, List<string> Decisions, bool ClrScreen)
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return -1;
            }
            string input = "0";
            int i_input = -1;
            int cursor_row = Console.CursorLeft;
            int decisionsize = Decisions.Count;

            do
            {
                do
                {
                    if (ClrScreen)
                    {
                        Console.Clear();
                    }
                    StringList.Add(IntroText);
                    StringList.AddRange(Decisions);
                    SetPrintingString();

                    if (!IsANumber(input))
                        StringList.Add("Input was not a valid answer. Try again.");

                    else if (i_input >= decisionsize)
                        StringList.Add("That was not a possible decision. Please choose from the list above.");

                    input = Console.ReadLine();
                    InterruptTyper();
                } while (!IsANumber(input));

                i_input = int.Parse(input) - 1;//Ususally lists start with 1 as the first choice, but data starts with 0.
            } while (i_input >= Decisions.Count);

            return ++i_input;//For the re-interpreted answer
        }

        /// <summary>
        /// Allows for Mulitple Choice construction
        /// </summary>
        /// <param name="IntroText">The introductory text to go before the choices</param>
        /// <param name="Decisions">The list of decisions, which can also exist within an array</param>
        /// <param name="ClrScreen">Asks if clearing the console is preferred before printing</param>
        /// <param name="Spacing">Asks if spacing between the choices is preferred [definition optional]</param>
        /// <returns></returns>
        public static ConsoleKeyInfo MultipleChoiceCK(string IntroText, List<string> Decisions, bool ClrScreen, bool Spacing = false)
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return CKI;
            }
            int cursor_row = Console.CursorLeft;
            int decisionsize = Decisions.Count;
            int loop_count = 0;
            do
            {
                if (ClrScreen)
                    Console.Clear();

                StringList.Add(WordWrap(IntroText));
                if (Spacing)
                {
                    for (int i = 0; i < Decisions.Count; i++)
                    {
                        StringList.Add(WordWrap(Decisions[i]));
                        if (i != Decisions.Count - 1)
                            StringList.Add("");
                    }
                }
                else
                    StringList.AddRange(WordWrap(Decisions));

                if (!IsANumber(CKI, decisionsize) && loop_count > 0)
                    StringList.Add("Input was not a valid answer. Try again.");

                SetPrintingString();

                CKI = Console.ReadKey(true);
                if (CKI.Key == ConsoleKey.Escape)
                {
                    InterruptTyper();
                    return CKI;
                }
                InterruptTyper();
                ++loop_count;
            } while (!IsANumber(CKI, decisionsize));

            return CKI;
        }
        public static ConsoleKeyInfo MultipleChoiceCK(string IntroText, string[] Decisions, bool ClrScreen)
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return CKI;
            }
            int cursor_row = Console.CursorLeft;
            int decisionsize = Decisions.Length;
            int loop_count = 0;
            do
            {
                if (ClrScreen)
                    Console.Clear();

                StringList.Add(WordWrap(IntroText));
                StringList.AddRange(WordWrap(Decisions));

                if (!IsANumber(CKI, decisionsize) && loop_count > 0)
                    StringList.Add("Input was not a valid answer. Try again.");

                SetPrintingString();

                CKI = Console.ReadKey(true);
                if (CKI.Key == ConsoleKey.Escape)
                {
                    InterruptTyper();
                    return CKI;
                }
                InterruptTyper();
                ++loop_count;
            } while (!IsANumber(CKI, decisionsize));

            return CKI;
        }

        public static ConsoleKeyInfo MultipleChoiceCK(string IntroText, List<string> Decisions, List<ConsoleColor> StringColors, bool ClrScreen)
        {
            if (!ColorTyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return CKI;
            }
            int cursor_row = Console.CursorLeft;
            int decisionsize = Decisions.Count;
            int loop_count = 0;
            do
            {
                if (ClrScreen)
                    Console.Clear();

                StringList.Add(WordWrap(IntroText));
                StringList.AddRange(WordWrap(Decisions));

                if (StringList.Count != StringColors.Count)
                {
                    ColorPrint("COLOR AMOUNT ERROR: ADD OR REMOVE SOME COLORS. REMEMBER THE IntroText COUNTS AS WELL!!",
                        ConsoleColor.White, true, true);
                    return CKI;
                }
                else
                {
                    for (int i = 0; i <= decisionsize; i++)
                    {
                        ColorStrings.Add(new FormattedString(StringList[i], StringColors[i]));
                    }
                    ColorStrings.Clear();
                }

                if (!IsANumber(CKI, decisionsize) && loop_count > 0)
                    ColorStrings.Add(new FormattedString("Input was not a valid answer. Try again.", ConsoleColor.Red));

                SetCPrintingString();
                if (TyperFinished)
                    CKI = Console.ReadKey(true);
                InterruptColorTyper();
                ++loop_count;
            } while (!IsANumber(CKI, decisionsize));

            return CKI;
        }

        public static ConsoleKeyInfo ChooseCK(string IntroText, List<string> Decisions, bool ClrScreen)//TODO: Work on this
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return CKI;
            }
            int loop_count = 0;
            List<int> choices = new List<int>(Decisions.Count);
            int cursor_row = Console.CursorTop;
            do
            {
                StringList.Add(WordWrap(IntroText));
                StringList.AddRange(WordWrap(Decisions));

            } while (CKI.Key != ConsoleKey.Enter);

            return CKI;
        }

        public static bool YesOrNo(string IntroText, bool ClrScreen)
        {
            if (!TyperSetUp)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            bool is_valid = true;

            do
            {
                if (ClrScreen)
                    Console.Clear();

                StringList.Add(WordWrap(IntroText));

                if (!is_valid)
                    StringList.Add("Input was nto a valid answer. Try again.");

                SetPrintingString();

                CKI = Console.ReadKey(true);
                switch (CKI.Key)
                {
                    case ConsoleKey.Y:
                        return true;
                    case ConsoleKey.N:
                        return false;
                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        return false;
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return true;
                    default:
                        is_valid = false;
                        break;
                }
                InterruptTyper();

            } while (!is_valid);
            return false;
        }


        public static string RequestInfo(string IntroText, bool ClrScreen)
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return "";
            }
            string input = "";
            if (ClrScreen)
                Console.Clear();

            StringList.Add(WordWrap(IntroText));
            SetPrintingString();

            input = Console.ReadLine();
            InterruptTyper();


            return input;
        }

        public static bool IsANumber(string input)
        {
            int str_len = input.Length;
            if (str_len == 0)
                return false;
            else
            {
                for (int i = 0; i < str_len; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (input[i] == j.ToString()[0])
                            break;
                        else if (j == 9 && input[i] != j.ToString()[0])
                            return false;


                        //  input = input.Substring(0, input.Length - 1);
                        //}

                    }
                }
            }
            return true;
        }

        //We assume the decision size is of, at least, one.
        private static bool IsANumber(ConsoleKeyInfo CKI, int size)
        {
            switch (CKI.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return true;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    if (2 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    if (3 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    if (4 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    if (5 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    if (6 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    if (7 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    if (8 > size)
                        return false;
                    else
                        return true;
                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    if (9 > size)
                        return false;
                    else
                        return true;
                default:
                    return false;
            }
        }

        public static void Print(string input, bool ClrScreen, bool includeReadline)
        {
            if (!TyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return;
            }
            if (ClrScreen)
                Console.Clear();

            StringList.Add(WordWrap(input));
            SetPrintingString();

            if (includeReadline)
            {
                Console.ReadLine();
                InterruptTyper();
            }
        }

        public static void ColorPrint(string input, ConsoleColor color, bool ClrScreen, bool includeReadline)
        {
            if (!ColorTyperSetUp)
            {
                Console.WriteLine("ERROR: Typer not setup! Use ConsoleUI.SetupTyper() to setup typer.");
                return;
            }
            if (ClrScreen)
                Console.Clear();

            ColorStrings.Add(new FormattedString(input, color));
            SetCPrintingString();

            if (includeReadline)
            {
                Console.ReadLine();
                InterruptColorTyper();
            }
        }

        private static string WordWrap(string input)
        {
            //TODO: Make hyphens split at the end of lines.
            string[] split_str = input.Split(' ');
            string result = "";
            int char_counter = 0;//This will describe the array register
            int size = split_str.Length;
            for (int i = 0; i < size; i++)
            {
                result += split_str[i];
                char_counter += split_str[i].Length;
                if (split_str[i].Contains("\t"))
                    char_counter += 7;
                if (i != size - 1)
                {
                    if ((char_counter >= ConsoleWidth ||
                        char_counter + 1 >= ConsoleWidth) ||
                            char_counter + split_str[i + 1].Length >= ConsoleWidth)
                    {
                        if (split_str[i].Length > ConsoleWidth)
                            char_counter = split_str[i].Length % (ConsoleWidth + 1);
                        else
                        {
                            result += "\n";
                            char_counter = 0;//Start a new line
                        }
                    }
                    if (char_counter != 0)
                    {
                        result += " ";
                        char_counter++;
                    }
                }
            }
            return result;
        }
        private static string[] WordWrap(string[] input)
        {
            int size = input.Length;
            string[] result = new string[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = WordWrap(input[i]);
            }

            return result;
        }
        private static List<string> WordWrap(List<string> input)
        {
            int size = input.Count;
            List<string> result = new List<string>(size);
            for (int i = 0; i < size; i++)
            {
                result.Add(WordWrap(input[i]));
            }

            return result;
        }
    }



}