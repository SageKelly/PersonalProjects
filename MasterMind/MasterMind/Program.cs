using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    class Program
    {
        #region  Game Elements
        #region Classes
        #region Player
        public class Player
        {
            static bool hasWon;
            static int turns_taken;
            static string name;
            #region Constructor(s)
            public Player()
            {
                hasWon = false;
                turns_taken = 0;
                name = "";
            }
            public Player(string n)
            {
                name = n;
                turns_taken = 0;
                hasWon = false;
            }
            #endregion
            #region Properties
            #region HasWon
            public bool HasWon
            {
                get
                {
                    return hasWon;
                }
                set
                {
                    hasWon = value;
                }
            }
            #endregion
            #region TurnsTaken
            public int TurnsTaken
            {
                get
                {
                    return turns_taken;
                }
                set
                {
                    turns_taken = value;
                }
            }
            #endregion
            #region Name
            public string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }
            #endregion
            #endregion
        }
        #endregion
        #region Piece
        public class Piece
        {
            string symbol;
            bool isReferenced;
            bool isOccupied;
            #region Constructors
            public Piece()
            {
                symbol = " ";
                isOccupied = false;
                isReferenced = false;
            }
            public Piece(string sym)
            {
                symbol = sym;
                isOccupied = false;
                isReferenced = false;
            }
            #endregion

            #region Properties
            #region Symbol
            public string Symbol
            {
                get
                {
                    return symbol;
                }
                set
                {
                    symbol = value;
                }
            }
            #endregion
            #region IsOccupied
            public bool IsOccupied
            {
                get
                {
                    return isOccupied;
                }
                set
                {
                    isOccupied = value;
                }
            }
            #endregion
            #region IsReferenced
            public bool IsReferenced
            {
                get
                {
                    return isReferenced;
                }
                set
                {
                    isReferenced = value;
                }
            }
            #endregion
            #endregion
        }
        #endregion
        #endregion
        #region DECLARED VARIABLES
        static Player p1;
        static Piece[,] GamePlayArray = new Piece[11, 9];
        static GameState GS = new GameState();
        static Random rand = new Random();
        static string[] Symbols = new string[10] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")" };
        static int row_num;
        static int desired_symbol_amount;
        static string player_input;
        static bool wrongInput;
        static int random_column;
        #region GameState
        public enum GameState
        {
            SetUp,
            Instructions,
            Turn,
            GameOver,
            EndGame,
            Rematch,
            Unreachable
        }
        #endregion
        #endregion
        #region Methods
        #region SetUpGame()
        private static void SetUpGame()
        {
            Console.Title = "Master Mind";
            Console.SetBufferSize(80, 30);
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Master Mind!");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("1: Start Game");
                Console.WriteLine("2: Instructions");
                Console.WriteLine("3: Exit");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("Choose and option by pressing the corresponding number.");
                if (wrongInput)
                {
                    Console.WriteLine("Incorrect Input.");
                    wrongInput = false;
                }
                player_input = Console.ReadLine();
                if (player_input == "1")
                {
                    GS = GameState.SetUp;
                }
                else if (player_input == "2")
                {
                    GS = GameState.Instructions;
                }
                else if (player_input == "3")
                {
                    GS = GameState.EndGame;
                }
                else
                    wrongInput = true;
            } while (player_input != "1" && player_input != "2" && player_input != "3");
        }
        #endregion
        #region SetUpBoard()
        private static void SetUpBoard()
        {
            for (int row = 0; row < 11; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    GamePlayArray[row, column] = new Piece();
                }
            }
        }
        #endregion
        #region CreateComputerCombo()
        private static void CreateComputerCombo()
        {
            int num_register;
            for (int i = 0; i < 4; i++)
            {
                num_register = rand.Next(0, desired_symbol_amount);
                GamePlayArray[0, i].Symbol = Symbols[num_register];
            }
            //GamePlayArray[0,0].Symbol="%";
            //GamePlayArray[0,1].Symbol="%";
            //GamePlayArray[0,2].Symbol="$";
            //GamePlayArray[0,3].Symbol="#";
        }
        #endregion
        #region PrintBoard()
        public static void PrintBoard()
        {
            #region Prints the Computer combo and the available symbols
            Console.Clear();
            #region Commented out text
            //Console.Write("    ");
            //for (int column = 0; column < 4; column++)
            //{
            //    if (column < 3)
            //        Console.Write("- ");
            //    else
            //        Console.Write("-");
            //}
            //Console.WriteLine();
            //Console.Write("   |");
            //for (int column = 0; column < 4; column++)
            //{
            //    Console.Write(GamePlayArray[0, column].Symbol + "|");
            //}
            #endregion
            Console.Write("   Your available symbols: ");
            for (int sym = 0; sym < desired_symbol_amount; sym++)
            {
                if (sym != desired_symbol_amount - 1)
                    Console.Write(Symbols[sym] + ", ");
                else
                    Console.Write(Symbols[sym]);
            }
            Console.WriteLine();
            #region COT
            //Console.Write("    ");
            //for (int i = 0; i < 4; i++)
            //{
            //    if (i < 3)
            //        Console.Write("- ");
            //    else
            //        Console.Write("-");
            //}
            #endregion
            #endregion
            Console.WriteLine();
            #region Prints all of the previous Player combos
            Console.Write("    ");
            for (int dash = 0; dash < 9; dash++)
            {
                if (dash == 4)
                    Console.Write("  ");
                else if (dash < 8)
                    Console.Write("- ");
                else
                    Console.Write("-");
            }
            Console.WriteLine();
            for (int row = 1; row < 11; row++)
            {
                Console.Write(row + ".");
                if (row < 10)
                    Console.Write(" ");
                for (int column = 0; column < 9; column++)
                {
                    Console.Write("|" + GamePlayArray[row, column].Symbol);
                }
                Console.Write("|");
                Console.WriteLine();
                Console.Write("    ");
                for (int dash = 0; dash < 9; dash++)
                {
                    if (dash == 4)
                        Console.Write("  ");
                    else if (dash < 8)
                        Console.Write("- ");
                    else
                        Console.Write("-");
                }
                Console.WriteLine();
            }
            #endregion
        }
        #endregion
        #region ParseCombo
        public static void ParseCombo(string input)
        {
            for (int player_char = 0; player_char < 4; player_char++)
            {
                GamePlayArray[row_num, player_char].Symbol = input.Substring(player_char, 1);
            }
        }
        #endregion
        #region CheckInput()
        public static void CheckInput(string input)
        {
            switch (GS)
            {
                case GameState.SetUp:
                    #region Text
                    int not_equal = 0;
                    for (int num = 5; num <= 10; num++)
                    {
                        if (player_input == num.ToString())
                        {
                            wrongInput = false;
                            return;
                        }
                        else
                            not_equal++;
                    }
                    if (not_equal == 6)
                    {
                        wrongInput = true;
                        return;
                    }
                    break;
                    #endregion
                case GameState.Turn:
                    #region Text
                    if (input.ToUpper().Contains("GIVE UP"))
                    {
                        GS = GameState.GameOver;
                        return;
                    }
                    else if (input.ToUpper().Contains("QUIT"))
                    {
                        GS = GameState.EndGame;
                        return;
                    }
                    if (input.Length != 4)
                    {
                        wrongInput = true;
                        return;
                    }
                    int similarities = 0;
                    for (int symbol = 0; symbol < 4; symbol++)
                    {
                        for (int comp_column = 0; comp_column < desired_symbol_amount; comp_column++)
                        {
                            if (input.Substring(symbol, 1) == Symbols[comp_column])
                            {
                                similarities++;
                            }
                        }
                    }
                    if (similarities != 4)
                    {
                        wrongInput = true;
                        GS = GameState.Turn;
                        return;
                    }
                    else
                        wrongInput = false;
                    #endregion
                    break;
                case GameState.Rematch:
                    #region Text
                    if (input.ToUpper() == "YES")
                    {
                        GS = GameState.SetUp;
                        return;
                    }
                    else if (input.ToUpper() == "NO")
                    {
                        GS = GameState.EndGame;
                    }
                    else
                    {
                        wrongInput = true;
                        GS = GameState.Rematch;
                    }
                    #endregion
                    break;
            }
        }
        #endregion
        #region CheckComboAccuracy()
        public static void CheckComboAccuracy(string input)
        {
            for (int player_char = 0; player_char < 4; player_char++)
            {
                for (int comp_char = 0; comp_char < 4; comp_char++)
                {
                    if (!GamePlayArray[row_num, player_char].IsReferenced &&
                        !GamePlayArray[0, comp_char].IsReferenced &&
                        GamePlayArray[row_num, player_char].Symbol == GamePlayArray[0, comp_char].Symbol)
                    {
                        #region Both Symbols in the right spot
                        if (player_char == comp_char)
                        {
                            do
                            {
                                random_column = rand.Next(5, 9);

                                if (!GamePlayArray[row_num, random_column].IsOccupied)
                                {
                                    GamePlayArray[row_num, random_column].Symbol = "2";
                                    GamePlayArray[0, comp_char].IsReferenced = true;
                                    GamePlayArray[row_num, player_char].IsReferenced = true;
                                }
                            } while (GamePlayArray[row_num, random_column].IsOccupied);
                            GamePlayArray[row_num, random_column].IsOccupied = true;
                        }
                        #endregion
                        #region Computer checks for a similar symbol in the same spot as the current symbol
                        else if (GamePlayArray[0, comp_char].Symbol == GamePlayArray[row_num, comp_char].Symbol)
                        {
                            do
                            {
                                random_column = rand.Next(5, 9);

                                if (!GamePlayArray[row_num, random_column].IsOccupied)
                                {
                                    GamePlayArray[row_num, random_column].Symbol = "2";
                                    GamePlayArray[0, comp_char].IsReferenced = true;
                                    GamePlayArray[row_num, comp_char].IsReferenced = true;
                                }
                            } while (GamePlayArray[row_num, random_column].IsOccupied);
                            GamePlayArray[row_num, random_column].IsOccupied = true;
                        }
                        #endregion
                        #region Player checks for a similar symbol in the same spot as the current symbol
                        else if (GamePlayArray[0, player_char].Symbol == GamePlayArray[row_num, player_char].Symbol)
                        {
                            do
                            {
                                random_column = rand.Next(5, 9);

                                if (!GamePlayArray[row_num, random_column].IsOccupied)
                                {
                                    GamePlayArray[row_num, random_column].Symbol = "2";
                                    GamePlayArray[0, player_char].IsReferenced = true;
                                    GamePlayArray[row_num, player_char].IsReferenced = true;
                                }
                            } while (GamePlayArray[row_num, random_column].IsOccupied);
                            GamePlayArray[row_num, random_column].IsOccupied = true;
                        }
                        #endregion
                        #region Marks a 1 for any other conditions
                        else
                        {

                            do
                            {
                                random_column = rand.Next(5, 9);

                                if (!GamePlayArray[row_num, random_column].IsOccupied)
                                {
                                    GamePlayArray[row_num, random_column].Symbol = "1";
                                    GamePlayArray[0, comp_char].IsReferenced = true;
                                    GamePlayArray[row_num, player_char].IsReferenced = true;
                                }
                            } while (GamePlayArray[row_num, random_column].IsOccupied);
                            GamePlayArray[row_num, random_column].IsOccupied = true;

                        }
                        #endregion
                    }
                }
            }
            #region Cleanup: Places zeros in remaining spots and un-references computer symbols
            for (int column = 5; column < 9; column++)
            {
                if (GamePlayArray[row_num, column].Symbol == " ")
                    GamePlayArray[row_num, column].Symbol = "0";
            }
            for (int register = 0; register < 4; register++)
            {
                GamePlayArray[0, register].IsReferenced = false;
            }
            #endregion
            #region Checks for Exact Answer
            int two_spots = 0;
            for (int register = 5; register < 9; register++)
            {
                if (GamePlayArray[row_num, register].Symbol == "2")
                    two_spots++;
            }
            if (two_spots == 4)
            {
                p1.HasWon = true;
            }
            #endregion
        }
        #endregion
        #region GameHandler()
        public static void GameHandler()
        {
            do
            {
                do
                {
                    switch (GS)
                    {
                        case GameState.SetUp:
                            #region Text
                            #region Name Registration
                            p1 = new Player();
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("What is your name?");
                                player_input = Console.ReadLine();
                                p1.Name = player_input;
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("Are you sure that's what you want? \"{0}\"", p1.Name);

                                    if (wrongInput)
                                    {
                                        Console.WriteLine("Incorrect Input.");
                                        wrongInput = false;
                                    }
                                    player_input = Console.ReadLine();
                                    if (player_input.ToUpper() != "YES" && player_input.ToUpper() != "NO")
                                        wrongInput = true;
                                } while (player_input.ToUpper() != "YES" && player_input.ToUpper() != "NO");

                            } while (player_input.ToUpper() != "YES");
                            #endregion
                            #region Piece Registration
                            do
                            {
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("How many pieces do you want to use? (Pick between five and ten)");
                                    if (wrongInput)
                                    {
                                        Console.WriteLine("Incorrect Input.");
                                        wrongInput = false;
                                    }
                                    player_input = Console.ReadLine();
                                    CheckInput(player_input);
                                } while (wrongInput);

                                desired_symbol_amount = int.Parse(player_input);

                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("Are you ok with this number? \"{0}\"", desired_symbol_amount);
                                    if (wrongInput)
                                    {
                                        Console.WriteLine("Incorrect input.");
                                        wrongInput = false;
                                    }
                                    player_input = Console.ReadLine();
                                    if (player_input.ToUpper() != "YES" && player_input.ToUpper() != "NO")
                                    {
                                        wrongInput = true;
                                    }
                                } while (player_input.ToUpper() != "YES" && player_input.ToUpper() != "NO");
                            } while (player_input.ToUpper() == "NO");
                            #endregion
                            row_num = 1;
                            SetUpBoard();
                            CreateComputerCombo();
                            GS = GameState.Turn;
                            #endregion
                            break;
                        case GameState.Instructions:
                            #region Text
                            Console.Clear();
                            Console.Write("\n\n\n\n\n\n\n\n\n" + 
@"The objective of the game is to figure out the code that the computer has 
                            
randomly created. When you first reach the game screen, it will show the 

available symbols you can use to figure out the computer's code. Type in a 

combination of symbols of length 4 and press " + "\"Enter\"" + @". It will then appear on 

the screen in the first available slot. The computer will then let you know how

close your were to its code by using the last four slots on the screen and 

placing either 0, 1, or 2 in the slots.");
                            Console.ReadLine();
                            Console.Clear();
                            Console.Write("\n\n\n\n\n\n\n\n\n\"0\"" + 
@" means that that one of the symbols are wrong, " + "\"1\"" + @" means that one of the 

symbols is right, but in the wrong slot, and " + "\"2\"" + @" means that one of the symbols 

is right AND in the right slot. They are placed randomly, so there is no 

correspondence between your code and the placement numbers (i.e. even if the 

first slot is " + "\"0\"" + @", that does not that the first symbol you put in was wrong).");
                            Console.ReadLine();
                            Console.Clear();
                            Console.Write("\n\n\n\n\n\n\n\n\n" + @"For example, I may have put in the code "+"\"!@#$\""+@" and got this in return:

    - - - -   - - - -      |---------------------------------------------------|1. |!|@|#|$| |0|1|2|0| <---|Here, it's saying that two are wrong (the zeros),  |    - - - -   - - - -      |one is right, but not in the right place (the one),|2. | | | | | | | | | |     |and one is both right AND in the right place (the  |    - - - -   - - - -      |two).                                              |                           |---------------------------------------------------|

");
                            Console.ReadLine();
                            Console.Clear();
                            Console.Write("\n\n\n\n\n\n\n\n\n"+
@"You have ten turns to figure it out. If you figure out before then, or on the 

tenth turn, you win. However, if you can't, you lose, at which point, you will 

be sent to a screen that will tell you what the computer's code was. 

Afterwards, you can then choose whether or not you want to play again. The game

will start over, and you will have to choose your name and your piece amount 

again.

Press "+"\"Enter\""+" to continue.");
                            Console.WriteLine();
                            Console.ReadLine();
                            SetUpGame();
                            #endregion
                            break;
                        case GameState.Turn:
                            #region Text
                            if (row_num != 11)
                            {
                                PrintBoard();
                                Console.WriteLine("Input your combo.");
                                if (wrongInput)
                                {
                                    Console.WriteLine("Incorrect input.");
                                    wrongInput = false;
                                }
                                player_input = Console.ReadLine();
                                CheckInput(player_input);
                                if (!wrongInput)
                                {
                                    ParseCombo(player_input);
                                    CheckComboAccuracy(player_input);
                                    row_num++;
                                    p1.TurnsTaken++;
                                    if (p1.HasWon)
                                    {
                                        GS = GameState.GameOver;
                                    }
                                }
                            }
                            else
                                GS = GameState.GameOver;
                            #endregion
                            break;
                    }
                } while (GS != GameState.GameOver && GS != GameState.EndGame);
                do
                {
                    switch (GS)
                    {
                        case GameState.GameOver:
                            #region Text
                            Console.Clear();
                            if (p1.HasWon)
                            {
                                Console.WriteLine("We regret to inform you that......................... YOU WON!!!!!\nNumber of turns taken: {0}", p1.TurnsTaken);
                                Console.WriteLine("\nPress \"Enter\" to continue.");
                                Console.ReadLine();
                                GS = GameState.Rematch;
                            }
                            else
                            {
                                Console.WriteLine("Congratulations, YOU LOST!!!!!\n Number of turns taken: {0}", p1.TurnsTaken);
                                Console.WriteLine("Actual Code: ");
                                #region Prints the Computer Code
                                Console.Write("    ");
                                for (int column = 0; column < 4; column++)
                                {
                                    if (column < 3)
                                        Console.Write("- ");
                                    else
                                        Console.Write("-");
                                }
                                Console.WriteLine();
                                Console.Write("   |");
                                for (int column = 0; column < 4; column++)
                                {
                                    Console.Write(GamePlayArray[0, column].Symbol + "|");
                                }
                                Console.WriteLine();
                                Console.Write("    ");
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i < 3)
                                        Console.Write("- ");
                                    else
                                        Console.Write("-");
                                }
                                #endregion
                                Console.WriteLine("\n Press \"Enter\" to continue.");
                                Console.ReadLine();
                                GS = GameState.Rematch;
                            }
                            #endregion
                            break;
                        case GameState.Rematch:
                            #region Text
                            Console.Clear();
                            if (wrongInput)
                            {
                                Console.WriteLine("Incorrect input.");
                                wrongInput = false;
                            }
                            Console.WriteLine("Would you like to play again?");
                            player_input = Console.ReadLine();
                            CheckInput(player_input);
                            #endregion
                            break;
                        case GameState.EndGame:
                            #region Text
                            Console.Clear();
                            Console.WriteLine("Thanks for playing and I hope you come back to play again. \n\n\n-\"Kasha\"\n\nPress \"Enter\" to exit.");
                            Console.ReadLine();
                            return;
                            #endregion
                    }
                } while (GS != GameState.SetUp && GS != GameState.EndGame);
            } while (GS != GameState.Unreachable);
            return;
        }
        #endregion
        #endregion
        #endregion
        static void Main(string[] args)
        {
            SetUpGame();
            GameHandler();
        }
    }
}
