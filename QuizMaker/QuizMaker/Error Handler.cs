using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizMaker
{
    static class Error_Handler
    {

        public static int MultipleChoice(string IntroText, List<string> Decisions, bool ClrScreen)
        {
            string input = "";
            int i_input = -1;
            int cursor_row = Console.CursorLeft;
            do
            {
                do
                {
                    if (ClrScreen)
                    {
                        Console.Clear();
                    }
                    Console.WriteLine(IntroText);

                    int decisionsize = Decisions.Count;
                    for (int i = 0; i < decisionsize; i++)
                    {
                        Console.WriteLine(i + ". " + Decisions[i]);
                    }
                    if (!IsANumber(input))
                        Console.WriteLine("Input was not a valid answer. Try again.");

                    if (i_input >= Decisions.Count)
                        Console.WriteLine("That was not a possible decision. Please choose from the list above.");

                    input = Console.ReadLine();
                } while (!IsANumber(input));

                i_input = int.Parse(input) - 1;//Ususally lists start with 1 as the first choice, but data starts with 0.
            } while (i_input >= Decisions.Count);

            return i_input + 1;//For the re-interprete answer
        }

        public static bool YesOrNo(string IntroText, bool ClrScreen)
        {
            string input = "";
            bool decision;

            do
            {
                if (ClrScreen)
                    Console.Clear();
                Console.WriteLine(IntroText);
                if (input.ToUpper() != "YES" && input.ToUpper() != "NO")
                    Console.WriteLine("Input was not a valid answer. Try again.");
                input = Console.ReadLine();
            } while (input.ToUpper() != "YES" && input.ToUpper() != "NO");

            if (input.ToUpper() == "YES")
                decision = true;
            else
                decision = false;
            return decision;
        }

        public static string RequestInfo(string IntroText, bool ClrScreen)
        {
            string input = "";
            if (ClrScreen)
                Console.Clear();

            Console.WriteLine(IntroText);
            input = Console.ReadLine();

            return input;
        }

        private static bool IsANumber(string input)
        {
            int str_len = input.Length;
            for (int i = 0; i < str_len; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (input[i] == j.ToString()[0])
                        break;
                    else if (j == 9 && input[i] != j.ToString()[0])
                        return false;
                }
            }
            return true;
        }

    }
}
