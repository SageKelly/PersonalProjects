using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AI_XML_Reader
{
    class Program
    {
        enum States
        {
            Menu,
            Setup,
            Load,
            View,
            Instructions,
            Main,
            Exit
        }
        static XMLDoc Doc;
        static States AIState;
        static ConsoleKeyInfo CSI;
        static bool DirDeclared;
        static List<string> dirs, FileNames;
        static string DesktopDir = "";

        static void Main(string[] args)
        {
            AIState = States.Menu;
            Console.WindowWidth = 100;
            Console.Title = "The Tree (Sage Kelly)";
            DirDeclared = false;
            dirs = new List<string>();
            FileNames = new List<string>();
            Console.ForegroundColor = ConsoleColor.Gray;
            while (AIState != States.Exit)
            {
                HandleKeyInfo();
            }
        }


        /// <summary>
        /// Handles key input per each state in the program
        /// </summary>
        private static void HandleKeyInfo()
        {
            switch (AIState)
            {
                case States.Menu:
                    PrintMenu();
                    switch (CSI.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1://Read Engraving
                            AIState = States.Instructions;
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2://Plant Seed
                            AIState = States.Setup;
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3://Visit Tree
                            if (DirDeclared)
                                AIState = States.View;
                            else
                                AIState = States.Menu;
                            break;
                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4://Pick Leaf
                            if (DirDeclared)
                                AIState = States.Main;
                            else
                                AIState = States.Menu;
                            break;
                        case ConsoleKey.D5:
                        case ConsoleKey.NumPad5://Go Home
                            AIState = States.Exit;
                            break;
                    }
                    break;
                case States.Setup:
                    PrintSetup();
                    AIState = States.Menu;
                    break;
                case States.View:
                    Console.Clear();
                    Doc.PrintSubtree();
                    Console.WriteLine("Press Enter to leave the tree.");
                    Console.ReadKey(true);
                    AIState = States.Menu;
                    break;
                case States.Instructions:
                    PrintInstructions();
                    AIState = States.Menu;
                    break;
                case States.Main:
                    PrintMain();
                    AIState = States.Menu;
                    break;
            }
        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.Write("Welcome to ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("the Tree");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Read the Engraving");
            Console.WriteLine("2. Plant a Seed");
            if (!DirDeclared)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("3. Visit my tree");
            Console.WriteLine("4. Pick a leaf.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("5. Go Home");
            CSI = Console.ReadKey(true);
        }

        private static void PrintInstructions()
        {
            Console.Clear();
            Console.Write(@"Welcome to the Tree. If you like you can plant a tree here. They grow very fast,
so you can visit them almost immediately. However, you must first tell me where
your seeds are. Surely you must know of a");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(@" directory");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(@" of where you keep your
seeds... Well, once you have found that, I can show you all the seeds you can plant.
You may plant as many as you want, but only one at a time. Once it's grown, you may
either visit your new tree, or find a leaf to pick from it. Everyone loves
souvenirs, and I'm sure your tree won't mind if you take some. To pick a leaf,
describe to me how the leaf ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(@"behaves ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(@"and I will ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(@"respond ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(@"with one that
matches. After that, you may decide to either look for more leaves or leave
your tree.");
            Console.WriteLine("Press Enter to stop reading.");
            Console.ReadKey(true);
        }

        private static void PrintSetup()
        {
            if (!DirDeclared)//If you don't have a directory defined...
            {
                bool valid = true;
                do
                {
                    Console.Clear();
                    if (!valid)
                    {
                        Console.WriteLine("Don't lie to me; I won't give you any fruit.");
                    }
                    Console.WriteLine("Tell me where your seeds are.");
                    DesktopDir = Console.ReadLine();

                    if (Directory.Exists(DesktopDir))
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                } while (!valid);//This keeps running until you give it a proper answer.

                DirDeclared = true;
            }

            FileNames = Directory.EnumerateFiles(DesktopDir, "*.xml", SearchOption.TopDirectoryOnly).ToList();

            int offset = 0;
            string ChosenPath = "";

            ///This loop prints out 9 of the choices of XML files you
            ///have currently in your chosen directory. The switch
            ///block allows you to choose from the ones printed on
            ///the screen while the up/down arrows allow you to
            ///scroll. It will automatically adjust for any offset
            ///in choice you make.
            while (ChosenPath == "")
            {
                Console.Clear();
                Console.WriteLine("Here are your seeds: (Press Up/Down to scroll)");
                if (offset != 0)
                {
                    Console.WriteLine("↑");
                }
                else
                    Console.WriteLine();
                for (int i = 0; i < (FileNames.Count >= 9 ? 9 : FileNames.Count); i++)
                {
                    Console.Write((i + 1).ToString().PadRight(2));
                    string name = Path.GetFileName(FileNames[i + offset]);
                    Console.WriteLine(": {0}", name);
                }
                if (FileNames.Count > 10 && offset < FileNames.Count - 9)
                {
                    Console.WriteLine("↓");
                }
                else
                    Console.WriteLine();

                CSI = Console.ReadKey();
                switch (CSI.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        ChosenPath = FileNames[0 + offset];
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        ChosenPath = FileNames[1 + offset];
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        ChosenPath = FileNames[2 + offset];
                        break;
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        ChosenPath = FileNames[3 + offset];
                        break;
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        ChosenPath = FileNames[4 + offset];
                        break;
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        ChosenPath = FileNames[5 + offset];
                        break;
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        ChosenPath = FileNames[6 + offset];
                        break;
                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        ChosenPath = FileNames[7 + offset];
                        break;
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        ChosenPath = FileNames[8 + offset];
                        break;
                    case ConsoleKey.UpArrow:
                        if (offset != 0)
                            offset--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (offset + 9 < FileNames.Count)
                            offset++;
                        break;
                    default:
                        break;
                }
            }
            Doc = new XMLDoc(@ChosenPath);
        }

        private static void PrintMain()
        {
            Console.Clear();
            string input = "";
            Random gen = new Random();
            int Top = Console.CursorTop;
            int Bottom = Top;
            Stopwatch stpwtch = new Stopwatch();


            while (CSI.Key != ConsoleKey.Escape)
            {
                Console.CursorTop = Top;
                for (int Y = Top; Y < Bottom; Y++)
                {
                    for (int X = 0; X < Console.WindowWidth; X++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Console.CursorTop = Top;

                Console.WriteLine("What type of leaves would you like to find?");
                input = Console.ReadLine();
                Console.WriteLine("I'll search two different ways.");

                stpwtch.Start();
                List<Node> results = Doc.BreadthFirstSearch(Doc.Root, input);
                stpwtch.Stop();

                if (results != null)
                {
                    if (results.Count > 0)
                    {
                        Console.WriteLine("Here's what I've found using Breadth-First:");

                        int index = gen.Next(results.Count);
                        Console.WriteLine(results[index].Attributes["response"]);

                        /*
                        foreach (Node n in results)
                        {
                            Console.WriteLine(n.Attributes["response"]);
                        }
                        */
                        Console.WriteLine("\nIt only tok me {0}min:{1}sec:{2}ms to find it.",
                            stpwtch.Elapsed.Minutes, stpwtch.Elapsed.Seconds, stpwtch.Elapsed.Milliseconds);
                    }
                    else
                    {
                        Console.WriteLine("I could not find any leaves for you using Breadth-First.");
                    }
                }



                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("--------------------------------------------------------------\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                stpwtch.Reset();

                stpwtch.Start();
                results = Doc.DepthFirstSearch(Doc.Root, input);
                stpwtch.Stop();

                if (results != null)
                {
                    if (results.Count > 0)
                    {
                        Console.WriteLine("Here's what I've found using Depth-First:");

                        int index = gen.Next(results.Count);
                        Console.WriteLine(results[index].Attributes["response"]);

                        /*
                        foreach (Node n in results)
                        {
                            Console.WriteLine(n.Attributes["response"]);
                        }
                        */
                        Console.WriteLine("\nIt only took me {0}min:{1}sec:{2}ms to find it.",
                            stpwtch.Elapsed.Minutes, stpwtch.Elapsed.Seconds, stpwtch.Elapsed.Milliseconds);
                    }
                    else
                    {
                        Console.WriteLine("I could not find any leaves for you using Depth-First.");
                    }
                }


                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("--------------------------------------------------------------\n");
                Console.ForegroundColor = ConsoleColor.Gray;

                stpwtch.Reset();

                Console.WriteLine("That is all. Press Enter if you want me to find more. Press Escape to leave your tree.");
                Bottom = Console.CursorTop;
                CSI = Console.ReadKey(true);
            }
        }
    }
}
