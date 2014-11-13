using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoTheGame
{
    #region Piece Class
    class Piece
    {
        #region Declared Variables
        Location register;
        Piece left;
        Piece top;
        Piece right;
        Piece bottom;
        int turn_created;
        string symbol;
        bool onEdge;
        bool captured;
        bool inCorner;
        bool topSame;
        bool leftSame;
        bool rightSame;
        bool isOccupied;
        bool bottomSame;
        #endregion
        #region Constructors
        public Piece()
        {
            register = new Location();
            symbol = "null";
            left = null;
            top = null;
            right = null;
            bottom = null;
            onEdge = false;
            captured = false;
            topSame = false;
            inCorner = false;
            leftSame = false;
            rightSame = false;
            isOccupied = false;
            bottomSame = false;
        }
        public Piece(int r, int c)
        {
            register = new Location(c, r);
            symbol = "null";
            left = null;
            top = null;
            right = null;
            bottom = null;
            onEdge = false;
            captured = false;
            topSame = false;
            inCorner = false;
            leftSame = false;
            rightSame = false;
            isOccupied = false;
            bottomSame = false;
        }
        public Piece(int r, int c, string sym, int tc)
        {
            register = new Location(r, c);
            turn_created = tc;
            symbol = sym;
            left = null;
            top = null;
            right = null;
            bottom = null;
            onEdge = false;
            captured = false;
            topSame = false;
            inCorner = false;
            leftSame = false;
            rightSame = false;
            isOccupied = false;
            bottomSame = false;
        }
        #endregion
        #region Properties
        #region Top
        public Piece Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }
        #endregion
        #region Left
        public Piece Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }
        #endregion
        #region Right
        public Piece Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
        #endregion
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
        #region Bottom
        public Piece Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
        }
        #endregion
        #region OnEdge
        public bool OnEdge
        {
            get
            {
                return onEdge;
            }
            set
            {
                onEdge = value;
            }
        }
        #endregion
        #region Register
        public Location Register
        {
            get
            {
                return register;
            }
            set
            {
                register = value;
            }
        }
        #endregion
        #region Captured
        public bool Captured
        {
            get
            {
                return captured;
            }
            set
            {
                captured = value;
            }
        }
        #endregion
        #region TopSame
        public bool TopSame
        {
            get
            {
                return topSame;
            }
            set
            {
                topSame = value;
            }
        }
        #endregion
        #region InCorner
        public bool InCorner
        {
            get
            {
                return inCorner;
            }
            set
            {
                inCorner = value;
            }
        }
        #endregion
        #region LeftSame
        public bool LeftSame
        {
            get
            {
                return leftSame;
            }
            set
            {
                leftSame = value;
            }
        }
        #endregion
        #region RightSame
        public bool RightSame
        {
            get
            {
                return rightSame;
            }
            set
            {
                rightSame = value;
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
        #region BottomSame
        public bool BottomSame
        {
            get
            {
                return bottomSame;
            }
            set
            {
                bottomSame = value;
            }
        }
        #endregion
        #region Turn_Created
        public int Turn_Created
        {
            get
            {
                return turn_created;
            }
            set
            {
                turn_created = value;
            }
        }
        #endregion
        #endregion
    }
    #endregion
    #region Player Class
    class Player
    {
        #region Declared Variables
        int score;
        bool win;
        bool myTurn;
        string input = "";
        string name = "";
        string symbol = "";
        #endregion
        #region Constructor
        public Player()
        {
            score = 0;
            win = false;
            myTurn = false;
        }
        #endregion
        #region Properties
        #region Win
        public bool Win
        {
            get
            {
                return win;
            }
            set
            {
                win = value;
            }
        }
        #endregion
        #region Input
        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }
        #endregion
        #region Score
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
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
        #region MyTurn
        public bool MyTurn
        {
            get
            {
                return myTurn;
            }
            set
            {
                myTurn = value;
            }
        }
        #endregion
        #endregion
        #region Methods
        public void CalculateScore()
        {
        }
        #endregion
    }
    #endregion
    #region Location Class
    class Location
    {
        #region Declared Variables
        int column;
        int row;
        #endregion
        #region Constructors
        public Location()
        {
            column = 0;
            row = 0;
        }
        public Location(int r, int c)
        {
            column = c;
            row = r;
        }
        #endregion
        #region Properties
        #region Column
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }
        #endregion
        #region Row
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }
        #endregion
        #endregion
    }
    #endregion
    //------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------
    class Program
    {
        #region Declared Variables
        #region Variables
        #region Oject Variables
        static Player player1;
        static Player player2;
        static GameStates GS = new GameStates();
        static ColumnLetters CL = new ColumnLetters();
        static Instructions Ins = new Instructions();
        static List<Piece> OrderedPieces = new List<Piece> { };
        static List<Piece> ConnectedPieces = new List<Piece> { };
        #endregion
        #region Primitive Variables
        static int turn_number = 1;
        static int turn_pass_count = 0;
        static int board_size_input = 0;
        static Piece[,] Gameboard;
        static Piece[,] SnapShotOfTurn1;
        static Piece[,] SnapShotOfTurn2;
        static Piece[,] TestBoardForKo;
        static string[] ComboArray;
        static string playerInput = "";
        static string reply = "";
        static string player1Name = "";
        static string player2Name = "";
        static string predecessor = "";
        static bool KoViolationFound;
        static bool ComboIsValid;
        static bool matchOver = false;
        static bool gameOver = false;
        #endregion
        #endregion
        #region enums
        #region ColumnLetters
        public enum ColumnLetters
        {
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S
        }
        #endregion
        #region GamesStates
        public enum GameStates
        {
            GameSetUp,
            Instructions,
            Rematch,
            FirstTimeIn,
            Pp1,
            Pp2,
            Tp1,
            Tp2,
            EndGame
        }
        #endregion
        #region Instructions
        public enum Instructions
        {
            Page1,
            Page2,
            Page3,
            Page4,
            Page5
        }
        #endregion
        #endregion
        #endregion
        #region Methods
        #region Set-up/Process Methods
        #region StartGame()
        static public GameStates StartGame()
        {
            #region *EXPLANATION*
            /*This method literally STARTS the game. It gives you 
             * the menu that allows you to move to the corresponding 
             * Game state. Then, the method ends.*/
            #endregion
            Console.Clear();
            Console.Title = "Go: the Chinese Game";
            Console.WriteLine("WELCOME TO THE GAME OF GO!\n----------------------------------------------------------------------------");
            Console.WriteLine("Press 1 to Play");
            Console.WriteLine("Press 2 for Instructions");
            Console.WriteLine("Press 3 to Exit.");
            reply = Console.ReadLine();
            #region Check for incorrect input
            while (reply.ToUpper() != "1" && reply.ToUpper() != "2" && reply.ToUpper() != "3")
            {
                Console.Clear();
                Console.Title = "Go: the Chinese Game";
                Console.WriteLine("WELCOME TO THE GAME OF GO!\n----------------------------------------------------------------------------");
                Console.WriteLine("Press 1 to Play");
                Console.WriteLine("Press 2 for Instructions");
                Console.WriteLine("Press 3 to Exit.");
                Console.WriteLine("Invalid answer");
                reply = Console.ReadLine();
            }
            #endregion
            /*Then move to the corresponding state*/
            if (reply == "1")
                return GS;
            else if (reply == "2")
            {
                GS = GameStates.Instructions;
                matchOver = false;
                return GS;
            }
            else if (reply == "3")
            {
                matchOver = true;
                gameOver = true;
                return GS;
            }
            return GS;
        }
        #endregion
        #region CreateBoard()
        private static void CreateBoard(int boardInput)
        {
            #region *EXPLANATION*
            /*This method just creates the 2D array that will eventually hold
             * all of the pieces. All new pieces created will go into this method.
             * Since there are none yet, the board starts off with all of the
             * registers equalling null.*/
            #endregion
            Gameboard = new Piece[boardInput, boardInput];
            SnapShotOfTurn1 = new Piece[boardInput, boardInput];
            SnapShotOfTurn2 = new Piece[boardInput, boardInput];
            TestBoardForKo = new Piece[boardInput, boardInput];
            for (int row = 0; row < boardInput; row++)
            {
                for (int column = 0; column < boardInput; column++)
                {
                    Gameboard[row, column] = null;
                    SnapShotOfTurn1[row, column] = null;
                    SnapShotOfTurn2[row, column] = null;
                    TestBoardForKo[row, column] = null;
                }
            }
            #region Debugging Region TBDL
            switch (board_size_input)
            {
                case 4:
                    Gameboard[0, 3] = new Piece(0, 3, player1.Symbol, 0);
                    Gameboard[1, 3] = new Piece(1, 3, player1.Symbol, 0);
                    Gameboard[2, 3] = new Piece(2, 3, player1.Symbol, 0);
                    Gameboard[3, 3] = new Piece(3, 3, player1.Symbol, 0);
                    Gameboard[3, 2] = new Piece(3, 2, player1.Symbol, 0);
                    Gameboard[0, 2] = new Piece(0, 2, player2.Symbol, 0);
                    Gameboard[1, 2] = new Piece(1, 2, player2.Symbol, 0);
                    Gameboard[2, 2] = new Piece(2, 2, player2.Symbol, 0);
                    break;
            }
            #endregion
        }
        #endregion
        #region PrintBoard()
        static public void PrintBoard()
        {
            /*Each region has it's own explanation. Open them
             * to see what they say.*/
            #region Screen Sizing
            #region *EXPLANATION*
            /* This is just for show, or display. It adjusts the size of
             * the Console so that it fits the game almost perfectly. 
             * There's still about 6 or more spaces left underneath 
             * the actual game board for placing your piece's 
             * coordinates.*/
            #endregion
            switch (board_size_input)
            {
                case 9:
                    Console.SetWindowSize(90, 30);
                    break;
                case 13:
                    Console.SetWindowSize(90, 38);
                    break;
                case 19:
                    Console.SetWindowSize(90, 50);
                    break;
            }
            #endregion
            Console.Clear();
            Console.Write(" ");
            #region Board Drawing
            #region EXPLANATION*
            /*In short, it's a LONG section of code for making the board
             * look how it does. It reads each register in the Game board 
             * array and prints out the piece's symbol. If there is no piece,
             * it writes out a space character instead.*/
            #endregion
            for (int g = 0; g < board_size_input; g++)
            {
                CL = (ColumnLetters)g;
                Console.Write(CL + " ");
            }
            Console.WriteLine();
            Console.Write("");
            for (int h = 0; h < board_size_input; h++)
            {
                Console.Write(" -");
            }
            Console.WriteLine();
            for (int r = 0; r < board_size_input; r++)
            {
                Console.Write("|");
                for (int c = 0; c < board_size_input; c++)
                {
                    if (Gameboard[r, c] == null)
                    {
                        Console.Write(" |");
                    }
                    else
                    {
                        Console.Write(Gameboard[r, c].Symbol + "|");
                    }
                }
                Console.Write((r + 1));
                Console.WriteLine();
                Console.Write("");
                for (int l = 0; l < board_size_input; l++)
                {
                    Console.Write(" -");
                }
                Console.WriteLine();
            }
            #endregion
        }
        #endregion
        #region CreateComboArray()
        private static void CreateComboArray(int boardInput)
        {
            #region *EXPLANATION*
            /*This method acts as an input-checker. However, with the way I did it, 
             * for the sake of simplicity, I needed to handle incorrect a different way.
             * Since I used a combination of letters and numbers, and there's over
             * 81/169/361 different combinations for each of the different-sized
             * boards, I decided to run a nested for loop that created all of the
             * possible combinations of letters and numbers for the particular
             * board by using the board_input integer as the limit, for both the letters
             * and the numbers. Then I store all of the combinations into an array.*/
            #endregion
            ComboArray = new string[boardInput * boardInput];
            int arrayIncrementor = 0;
            for (int i = 0; i < boardInput; i++)
            {
                CL = (ColumnLetters)i;
                for (int k = 0; k < boardInput; k++)
                {
                    ComboArray[arrayIncrementor] = ((k + 1).ToString() + CL.ToString());
                    arrayIncrementor++;
                }
            }
            return;
        }
        #endregion
        #region CheckForPossibleCombo()
        static public bool CheckForPossibleCombo(string input)
        {
            #region *EXPLANATION*
            /*This method checks input string up next to each of the
             * different possible combinations in the combo array. If 
             * the input matches with the one of the combinations in the
             * array, then it returns and continues with the game. If
             * not, there's trouble afoot, and the method alerts the 
             * player that their input has been declined.*/
            #endregion
            foreach (string a in ComboArray)
            {
                if (input == a)
                {
                    ComboIsValid = true;
                    return ComboIsValid;
                }
                else
                {
                    ComboIsValid = false;
                }
            }
            return ComboIsValid;
        }
        #endregion
        #region TranslateToColumnLetters()
        static public ColumnLetters TranslateToColumnLetters(string letter)
        {
            #region *EXPLANATION*
            /*There's probably, or DEFINITELY, a better way to handle
             * this problem, but I couldn't think of it. This is a sub-method:
             * not particularly important, but necessary for me to continue
             * in the game. Since I used letters and numbers for input, I had
             * to think of a way to translate string letters into enumerated
             * letters, and then into numbers. This was the bridge between 
             * string letters and enumerated letters. After they're translated
             * the method returns from whence it came. The second bridge
             * was handled for me (thank you, GetHashCode() method!).*/
            #endregion
            switch (letter.ToUpper())
            {
                #region text
                case "A":
                    CL = ColumnLetters.A;
                    break;
                case "B":
                    CL = ColumnLetters.B;
                    break;
                case "C":
                    CL = ColumnLetters.C;
                    break;
                case "D":
                    CL = ColumnLetters.D;
                    break;
                case "E":
                    CL = ColumnLetters.E;
                    break;
                case "F":
                    CL = ColumnLetters.F;
                    break;
                case "G":
                    CL = ColumnLetters.G;
                    break;
                case "H":
                    CL = ColumnLetters.H;
                    break;
                case "I":
                    CL = ColumnLetters.I;
                    break;
                case "J":
                    CL = ColumnLetters.J;
                    break;
                case "K":
                    CL = ColumnLetters.K;
                    break;
                case "L":
                    CL = ColumnLetters.L;
                    break;
                case "M":
                    CL = ColumnLetters.M;
                    break;
                case "N":
                    CL = ColumnLetters.N;
                    break;
                case "O":
                    CL = ColumnLetters.O;
                    break;
                case "P":
                    CL = ColumnLetters.P;
                    break;
                case "Q":
                    CL = ColumnLetters.Q;
                    break;
                case "R":
                    CL = ColumnLetters.R;
                    break;
                case "S":
                    CL = ColumnLetters.S;
                    break;
                #endregion
            }
            return CL;
        }
        #endregion
        #region AskForRematch()
        public static void AskForRematch()
        {
            #region *EXPLANATION*
            /* This method does its name: asks for a rematch. Depending
             * on your response, it'll either restart the game, or kill it.*/
            #endregion
            do
            {
                Console.Clear();
                Console.WriteLine("Would you like to play again?");
                playerInput = Console.ReadLine();
                if (playerInput.ToUpper() == "YES")
                {
                    Console.Clear();
                    GS = GameStates.GameSetUp;
                    return;
                }
                else if (playerInput.ToUpper() == "NO")
                {
                    GS = GameStates.EndGame;
                    return;
                }
                else
                    Console.WriteLine("Invalid input");
            } while (playerInput.ToUpper() != "YES" || playerInput.ToUpper() != "NO");
        }
        #endregion
        #endregion

        #region Piece-Capturing Methods
        #region StartCaptureProcess()
        #region *EXPLANATION*
        /* For the rest of the methods in this section, the explanations
         * will carried over to each method used in this process. For
         * this particular one, it goes through each piece, and places them
         * into a list to be sorted according to the turn in which they were
         * created. The earliest ones are at the front of the list. This way,
         * the newest gets highter priority over the latest piece, a device
         * necessary for most of the captures in the game to work properly.
         * Once that is done, the method goes through each piece, performs the
         * capture process, captures them, and updates the board, so
         * that the rest of the pieces know what happened to the pieces
         * above/left/right/below them.*/
        #endregion
        private static void StartCaptureProcess()
        {
            #region Sort pieces in order of turn placed
            foreach (Piece piece in Gameboard)
            {
                if (piece != null)
                {
                    OrderedPieces.Add(piece);
                }
            }
            for (int first_piece = 0; first_piece < OrderedPieces.Count; first_piece++)
            {
                for (int next_piece = first_piece + 1; next_piece < OrderedPieces.Count; next_piece++)
                {
                    if (OrderedPieces[first_piece].Turn_Created > OrderedPieces[next_piece].Turn_Created)
                    {
                        Piece temp = OrderedPieces[next_piece];
                        OrderedPieces[next_piece] = OrderedPieces[first_piece];
                        OrderedPieces[first_piece] = temp;
                    }
                }
            }
            #endregion
            foreach (Piece piece in OrderedPieces)
            {
                if (piece != null)
                {
                    ConnectPieces(piece);
                    RemoveCopies();
                    CheckforCapture();
                    Capture();
                    UpdatePieces();
                }
            }
            OrderedPieces.Clear();
            return;
        }
        #endregion
        #region ConnectPieces()
        public static void ConnectPieces(Piece piece)
        {
            #region *EXPLANATION*
            /*This method checks each piece to see if there are other
             * friendly pieces above/left.... well, you know. The Search
             * methods are recursive, and move in each of the cardinal
             * directions and adds each of the connected friendly pieces
             * to a friendly list for later reference. Then, once each method
             * have either hit a null space, or an opposing piece, they return
             * to this method. Once all of the methods have returned, this
             * one does the same. */
            #endregion
            ConnectedPieces.Add(piece);
            #region piece.Top
            if (piece.TopSame != true)
                if (piece.Top != null && piece.Top.Symbol == piece.Symbol)
                {
                    piece.TopSame = true;
                    piece.Top.BottomSame = true;
                    ConnectedPieces.Add(piece.Top);
                    if (piece.Top.TopSame != true)
                        if (piece.Top.Top != null && piece.Top.Top.Symbol == piece.Symbol)
                        {
                            piece.Top.TopSame = true;
                            piece.Top.Top.BottomSame = true;
                            ConnectedPieces.Add(piece.Top.Top);
                            predecessor = "from bottom";
                            SearchUp(piece.Top);
                        }
                    if (piece.Top.LeftSame != true)
                        if (piece.Top.Left != null && piece.Top.Left.Symbol == piece.Symbol)
                        {
                            piece.Top.LeftSame = true;
                            piece.Top.Left.RightSame = true;
                            ConnectedPieces.Add(piece.Top.Left);
                            predecessor = "from bottom";
                            SearchLeft(piece.Top);
                        }
                    if (piece.Top.RightSame != true)
                        if (piece.Top.Right != null && piece.Top.Right.Symbol == piece.Symbol)
                        {
                            piece.Top.RightSame = true;
                            piece.Top.Right.LeftSame = true;
                            ConnectedPieces.Add(piece.Top.Right);
                            predecessor = "from bottom";
                            SearchRight(piece.Top);
                        }
                }
            #endregion
            #region piece.Right
            if (piece.RightSame != true)
                if (piece.Right != null && piece.Right.Symbol == piece.Symbol)
                {
                    piece.RightSame = true;
                    piece.Right.LeftSame = true;
                    ConnectedPieces.Add(piece.Right);
                    if (piece.Right.TopSame != true)
                        if (piece.Right.Top != null && piece.Right.Top.Symbol == piece.Symbol)
                        {
                            piece.Right.TopSame = true;
                            piece.Right.Top.BottomSame = true;
                            ConnectedPieces.Add(piece.Right.Top);
                            predecessor = "from left";
                            SearchUp(piece.Right);
                        }
                    if (piece.Right.BottomSame != true)
                        if (piece.Right.Bottom != null && piece.Right.Bottom.Symbol == piece.Symbol)
                        {
                            piece.Right.BottomSame = true;
                            piece.Right.Bottom.TopSame = true;
                            ConnectedPieces.Add(piece.Right.Bottom);
                            predecessor = "from left";
                            SearchDown(piece.Right);
                        }
                    if (piece.Right.RightSame != true)
                        if (piece.Right.Right != null && piece.Right.Right.Symbol == piece.Symbol)
                        {
                            piece.Right.RightSame = true;
                            piece.Right.Right.LeftSame = true;
                            ConnectedPieces.Add(piece.Right.Right);
                            predecessor = "from left";
                            SearchRight(piece.Right);
                        }
                }
            #endregion
            #region piece.Bottom
            if (piece.BottomSame != true)
                if (piece.Bottom != null && piece.Bottom.Symbol == piece.Symbol)
                {
                    piece.BottomSame = true;
                    piece.Bottom.TopSame = true;
                    ConnectedPieces.Add(piece.Bottom);
                    if (piece.Bottom.LeftSame != true)
                        if (piece.Bottom.Left != null && piece.Bottom.Left.Symbol == piece.Symbol)
                        {
                            piece.Bottom.LeftSame = true;
                            piece.Bottom.Left.RightSame = true;
                            ConnectedPieces.Add(piece.Bottom.Left);
                            predecessor = "from top";
                            SearchLeft(piece.Bottom);
                        }
                    if (piece.Bottom.BottomSame != true)
                        if (piece.Bottom.Bottom != null && piece.Bottom.Bottom.Symbol == piece.Symbol)
                        {
                            piece.Bottom.BottomSame = true;
                            piece.Bottom.Bottom.TopSame = true;
                            ConnectedPieces.Add(piece.Bottom.Bottom);
                            predecessor = "from top";
                            SearchDown(piece.Bottom);
                        }
                    if (piece.Bottom.RightSame != true)
                        if (piece.Bottom.Right != null && piece.Bottom.Right.Symbol == piece.Symbol)
                        {
                            piece.Bottom.RightSame = true;
                            piece.Bottom.Right.LeftSame = true;
                            ConnectedPieces.Add(piece.Bottom.Right);
                            predecessor = "from top";
                            SearchRight(piece.Bottom);
                        }
                }
            #endregion
            #region piece.Left
            if (piece.LeftSame != true)
                if (piece.Left != null && piece.Left.Symbol == piece.Symbol)
                {
                    piece.LeftSame = true;
                    piece.Left.RightSame = true;
                    ConnectedPieces.Add(piece.Left);
                    if (piece.Left.TopSame != true)
                        if (piece.Left.Top != null && piece.Left.Top.Symbol == piece.Symbol)
                        {
                            piece.Left.TopSame = true;
                            piece.Left.Top.BottomSame = true;
                            ConnectedPieces.Add(piece.Left.Top);
                            predecessor = "from right";
                            SearchUp(piece.Left);
                        }
                    if (piece.Left.LeftSame != true)
                        if (piece.Left.Left != null && piece.Left.Left.Symbol == piece.Symbol)
                        {
                            piece.Left.LeftSame = true;
                            piece.Left.Left.RightSame = true;
                            ConnectedPieces.Add(piece.Left.Left);
                            predecessor = "from right";
                            SearchLeft(piece.Left);
                        }
                    if (piece.Left.BottomSame != true)
                        if (piece.Left.Bottom != null && piece.Left.Bottom.Symbol == piece.Symbol)
                        {
                            piece.Left.BottomSame = true;
                            piece.Left.Bottom.TopSame = true;
                            ConnectedPieces.Add(piece.Left.Bottom);
                            predecessor = "from right";
                            SearchDown(piece.Left);
                        }
                }
            #endregion
            return;
        }
        #endregion
        #region Cardinal Search Methods
        #region EXPLANATION*
        /*These have already been explained in the 
         * StartCaptureProcess() method. Read its 
         * explanation for their explanation.*/
        #endregion
        #region SearchRight()
        public static void SearchRight(Piece current)
        {
            #region piece.Top
            if (predecessor != "from top")
                if (current.Top != null && current.Top.Symbol == current.Symbol)
                {
                    current.TopSame = true;
                    current.Top.BottomSame = true;
                    ConnectedPieces.Add(current.Top);
                    if (current.Top.TopSame != true)
                        if (current.Top.Top != null && current.Top.Top.Symbol == current.Top.Symbol)
                        {
                            current.Top.TopSame = true;
                            current.Top.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Top.Top);
                            predecessor = "from bottom";
                            SearchUp(current.Top);
                        }
                    if (current.Top.LeftSame != true)
                        if (current.Top.Left != null && current.Top.Left.Symbol == current.Top.Symbol)
                        {
                            current.Top.LeftSame = true;
                            current.Top.Left.RightSame = true;
                            ConnectedPieces.Add(current.Top.Left);
                            predecessor = "from bottom";
                            SearchLeft(current.Top);
                        }
                    if (current.Top.RightSame != true)
                        if (current.Top.Right != null && current.Top.Right.Symbol == current.Top.Symbol)
                        {
                            current.Top.RightSame = true;
                            current.Top.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Top.Right);
                            predecessor = "from bottom";
                            SearchRight(current.Top);
                        }
                }
            #endregion
            #region piece.Right
            if (current.Right != null && current.Right.Symbol == current.Symbol)
            {
                if (current.Right.TopSame != true)
                    if (current.Right.Top != null && current.Right.Top.Symbol == current.Symbol)
                    {
                        current.Right.TopSame = true;
                        current.Right.Top.BottomSame = true;
                        ConnectedPieces.Add(current.Right.Top);
                        predecessor = "from left";
                        SearchUp(current.Right);
                    }
                if (current.Right.BottomSame != true)
                    if (current.Right.Bottom != null && current.Right.Bottom.Symbol == current.Symbol)
                    {
                        current.Right.BottomSame = true;
                        current.Right.Bottom.TopSame = true;
                        ConnectedPieces.Add(current.Right.Bottom);
                        predecessor = "from left";
                        SearchDown(current.Right);
                    }
                if (current.Right.RightSame != true)
                    if (current.Right.Right != null && current.Right.Right.Symbol == current.Symbol)
                    {
                        current.Right.RightSame = true;
                        current.Right.Right.LeftSame = true;
                        ConnectedPieces.Add(current.Right.Right);
                        predecessor = "from left";
                        SearchRight(current.Right);
                    }
            }
            #endregion
            #region piece.Bottom
            if (predecessor != "from bottom")
                if (current.Bottom != null && current.Bottom.Symbol == current.Symbol)
                {
                    current.BottomSame = true;
                    current.Bottom.TopSame = true;
                    ConnectedPieces.Add(current.Bottom);
                    if (current.Bottom.LeftSame != true)
                        if (current.Bottom.Left != null && current.Bottom.Left.Symbol == current.Symbol)
                        {
                            current.Bottom.LeftSame = true;
                            current.Bottom.Left.RightSame = true;
                            ConnectedPieces.Add(current.Bottom.Left);
                            predecessor = "from top";
                            SearchLeft(current.Bottom);
                        }
                    if (current.Bottom.BottomSame != true)
                        if (current.Bottom.Bottom != null && current.Bottom.Bottom.Symbol == current.Symbol)
                        {
                            current.Bottom.BottomSame = true;
                            current.Bottom.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Bottom.Bottom);
                            predecessor = "from top";
                            SearchDown(current.Bottom);
                        }
                    if (current.Bottom.RightSame != true)
                        if (current.Bottom.Right != null && current.Bottom.Right.Symbol == current.Symbol)
                        {
                            current.Bottom.RightSame = true;
                            current.Bottom.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Bottom.Right);
                            predecessor = "from top";
                            SearchRight(current.Bottom);
                        }
                }
            #endregion
            return;
        }
        #endregion
        #region SearchDown()
        public static void SearchDown(Piece current)
        {
            #region piece.Right
            if (predecessor != "from right")
                if (current.Right != null && current.Right.Symbol == current.Symbol)
                {
                    current.RightSame = true;
                    current.Right.LeftSame = true;
                    ConnectedPieces.Add(current.Right);
                    if (current.Right.TopSame != true)
                        if (current.Right.Top != null && current.Right.Top.Symbol == current.Symbol)
                        {
                            current.Right.TopSame = true;
                            current.Right.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Right.Top);
                            predecessor = "from left";
                            SearchUp(current.Right);
                        }
                    if (current.Right.BottomSame != true)
                        if (current.Right.Bottom != null && current.Right.Bottom.Symbol == current.Symbol)
                        {
                            current.Right.BottomSame = true;
                            current.Right.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Right.Bottom);
                            predecessor = "from left";
                            SearchDown(current.Right);
                        }
                    if (current.Right.RightSame != true)
                        if (current.Right.Right != null && current.Right.Right.Symbol == current.Symbol)
                        {
                            current.Right.RightSame = true;
                            current.Right.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Right.Right);
                            predecessor = "from left";
                            SearchRight(current.Right);
                        }
                }
            #endregion
            #region piece.Bottom
            if (current.Bottom != null && current.Bottom.Symbol == current.Symbol)
            {
                if (current.Bottom.LeftSame != true)
                    if (current.Bottom.Left != null && current.Bottom.Left.Symbol == current.Symbol)
                    {
                        current.Bottom.LeftSame = true;
                        current.Bottom.Left.RightSame = true;
                        ConnectedPieces.Add(current.Bottom.Left);
                        predecessor = "from top";
                        SearchLeft(current.Bottom);
                    }
                if (current.Bottom.BottomSame != true)
                    if (current.Bottom.Bottom != null && current.Bottom.Bottom.Symbol == current.Symbol)
                    {
                        current.Bottom.BottomSame = true;
                        current.Bottom.Bottom.TopSame = true;
                        ConnectedPieces.Add(current.Bottom.Bottom);
                        predecessor = "from top";
                        SearchDown(current.Bottom);
                    }
                if (current.Bottom.RightSame != true)
                    if (current.Bottom.Right != null && current.Bottom.Right.Symbol == current.Symbol)
                    {
                        current.Bottom.RightSame = true;
                        current.Bottom.Right.LeftSame = true;
                        ConnectedPieces.Add(current.Bottom.Right);
                        predecessor = "from top";
                        SearchRight(current.Bottom);
                    }
            }
            #endregion
            #region piece.Left
            if (predecessor != "from left")
                if (current.Left != null && current.Left.Symbol == current.Symbol)
                {
                    current.LeftSame = true;
                    current.Left.RightSame = true;
                    ConnectedPieces.Add(current.Left);
                    if (current.Left.TopSame != true)
                        if (current.Left.Top != null && current.Left.Top.Symbol == current.Symbol)
                        {
                            current.Left.TopSame = true;
                            current.Left.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Left.Top);
                            predecessor = "from right";
                            SearchUp(current.Left);
                        }
                    if (current.Left.LeftSame != true)
                        if (current.Left.Left != null && current.Left.Left.Symbol == current.Symbol)
                        {
                            current.Left.LeftSame = true;
                            current.Left.Left.RightSame = true;
                            ConnectedPieces.Add(current.Left.Left);
                            predecessor = "from right";
                            SearchLeft(current.Left);
                        }
                    if (current.Left.BottomSame != true)
                        if (current.Left.Bottom != null && current.Left.Bottom.Symbol == current.Symbol)
                        {
                            current.Left.BottomSame = true;
                            current.Left.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Left.Bottom);
                            predecessor = "from right";
                            SearchDown(current.Left);
                        }
                }
            #endregion
            return;
        }
        #endregion
        #region SearchUp()
        public static void SearchUp(Piece current)
        {
            #region piece.Top
            if (current.Top != null && current.Top.Symbol == current.Symbol)
            {
                if (current.Top.TopSame != true)
                    if (current.Top.Top != null && current.Top.Top.Symbol == current.Top.Symbol)
                    {
                        current.Top.TopSame = true;
                        current.Top.Top.BottomSame = true;
                        ConnectedPieces.Add(current.Top.Top);
                        predecessor = "from bottom";
                        SearchUp(current.Top);
                    }
                if (current.Top.LeftSame != true)
                    if (current.Top.Left != null && current.Top.Left.Symbol == current.Top.Symbol)
                    {
                        current.Top.LeftSame = true;
                        current.Top.Left.RightSame = true;
                        ConnectedPieces.Add(current.Top.Left);
                        predecessor = "from bottom";
                        SearchLeft(current.Top);
                    }
                if (current.Top.RightSame != true)
                    if (current.Top.Right != null && current.Top.Right.Symbol == current.Top.Symbol)
                    {
                        current.Top.RightSame = true;
                        current.Top.Right.LeftSame = true;
                        ConnectedPieces.Add(current.Top.Right);
                        predecessor = "from bottom";
                        SearchRight(current.Top);
                    }
            }
            #endregion
            #region piece.Right
            if (predecessor != "from right")
                if (current.Right != null && current.Right.Symbol == current.Symbol)
                {
                    current.RightSame = true;
                    current.Right.LeftSame = true;
                    ConnectedPieces.Add(current.Right);
                    if (current.Right.TopSame != true)
                        if (current.Right.Top != null && current.Right.Top.Symbol == current.Symbol)
                        {
                            current.Right.TopSame = true;
                            current.Right.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Right.Top);
                            predecessor = "from left";
                            SearchUp(current.Right);
                        }
                    if (current.Right.BottomSame != true)
                        if (current.Right.Bottom != null && current.Right.Bottom.Symbol == current.Symbol)
                        {
                            current.Right.BottomSame = true;
                            current.Right.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Right.Bottom);
                            predecessor = "from left";
                            SearchDown(current.Right);
                        }
                    if (current.Right.RightSame != true)
                        if (current.Right.Right != null && current.Right.Right.Symbol == current.Symbol)
                        {
                            current.Right.RightSame = true;
                            current.Right.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Right.Right);
                            predecessor = "from left";
                            SearchRight(current.Right);
                        }
                }
            #endregion
            #region piece.Left
            if (predecessor != "from left")
                if (current.Left != null && current.Left.Symbol == current.Symbol)
                {
                    current.LeftSame = true;
                    current.Left.RightSame = true;
                    ConnectedPieces.Add(current.Left);
                    if (current.Left.TopSame != true)
                        if (current.Left.Top != null && current.Left.Top.Symbol == current.Symbol)
                        {
                            current.Left.TopSame = true;
                            current.Left.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Left.Top);
                            predecessor = "from right";
                            SearchUp(current.Left);
                        }
                    if (current.Left.LeftSame != true)
                        if (current.Left.Left != null && current.Left.Left.Symbol == current.Symbol)
                        {
                            current.Left.LeftSame = true;
                            current.Left.Left.RightSame = true;
                            ConnectedPieces.Add(current.Left.Left);
                            predecessor = "from right";
                            SearchLeft(current.Left);
                        }
                    if (current.Left.BottomSame != true)
                        if (current.Left.Bottom != null && current.Left.Bottom.Symbol == current.Symbol)
                        {
                            current.Left.BottomSame = true;
                            current.Left.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Left.Bottom);
                            predecessor = "from right";
                            SearchDown(current.Left);
                        }
                }
            #endregion
            return;
        }
        #endregion
        #region SearchLeft()
        public static void SearchLeft(Piece current)
        {
            #region piece.Top
            if (predecessor != "from top")
                if (current.Top != null && current.Top.Symbol == current.Symbol)
                {
                    current.TopSame = true;
                    current.Top.BottomSame = true;
                    ConnectedPieces.Add(current.Top);
                    if (current.Top.TopSame != true)
                        if (current.Top.Top != null && current.Top.Top.Symbol == current.Top.Symbol)
                        {
                            current.Top.TopSame = true;
                            current.Top.Top.BottomSame = true;
                            ConnectedPieces.Add(current.Top.Top);
                            predecessor = "from bottom";
                            SearchUp(current.Top);
                        }
                    if (current.Top.LeftSame != true)
                        if (current.Top.Left != null && current.Top.Left.Symbol == current.Top.Symbol)
                        {
                            current.Top.LeftSame = true;
                            current.Top.Left.RightSame = true;
                            ConnectedPieces.Add(current.Top.Left);
                            predecessor = "from bottom";
                            SearchLeft(current.Top);
                        }
                    if (current.Top.RightSame != true)
                        if (current.Top.Right != null && current.Top.Right.Symbol == current.Top.Symbol)
                        {
                            current.Top.RightSame = true;
                            current.Top.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Top.Right);
                            predecessor = "from bottom";
                            SearchRight(current.Top);
                        }
                }
            #endregion
            #region piece.Bottom
            if (predecessor != "from bottom")
                if (current.Bottom != null && current.Bottom.Symbol == current.Symbol)
                {
                    current.BottomSame = true;
                    current.Bottom.TopSame = true;
                    ConnectedPieces.Add(current.Bottom);
                    if (current.Bottom.LeftSame != true)
                        if (current.Bottom.Left != null && current.Bottom.Left.Symbol == current.Symbol)
                        {
                            current.Bottom.LeftSame = true;
                            current.Bottom.Left.RightSame = true;
                            ConnectedPieces.Add(current.Bottom.Left);
                            predecessor = "from top";
                            SearchLeft(current.Bottom);
                        }
                    if (current.Bottom.BottomSame != true)
                        if (current.Bottom.Bottom != null && current.Bottom.Bottom.Symbol == current.Symbol)
                        {
                            current.Bottom.BottomSame = true;
                            current.Bottom.Bottom.TopSame = true;
                            ConnectedPieces.Add(current.Bottom.Bottom);
                            predecessor = "from top";
                            SearchDown(current.Bottom);
                        }
                    if (current.Bottom.RightSame != true)
                        if (current.Bottom.Right != null && current.Bottom.Right.Symbol == current.Symbol)
                        {
                            current.Bottom.RightSame = true;
                            current.Bottom.Right.LeftSame = true;
                            ConnectedPieces.Add(current.Bottom.Right);
                            predecessor = "from top";
                            SearchRight(current.Bottom);
                        }
                }
            #endregion
            #region piece.Left
            if (current.Left != null && current.Left.Symbol == current.Symbol)
            {
                if (current.Left.TopSame != true)
                    if (current.Left.Top != null && current.Left.Top.Symbol == current.Symbol)
                    {
                        current.Left.TopSame = true;
                        current.Left.Top.BottomSame = true;
                        ConnectedPieces.Add(current.Left.Top);
                        predecessor = "from right";
                        SearchUp(current.Left);
                    }
                if (current.Left.LeftSame != true)
                    if (current.Left.Left != null && current.Left.Left.Symbol == current.Symbol)
                    {
                        current.Left.LeftSame = true;
                        current.Left.Left.RightSame = true;
                        ConnectedPieces.Add(current.Left.Left);
                        predecessor = "from right";
                        SearchLeft(current.Left);
                    }
                if (current.Left.BottomSame != true)
                    if (current.Left.Bottom != null && current.Left.Bottom.Symbol == current.Symbol)
                    {
                        current.Left.BottomSame = true;
                        current.Left.Bottom.TopSame = true;
                        ConnectedPieces.Add(current.Left.Bottom);
                        predecessor = "from right";
                        SearchDown(current.Left);
                    }
            }
            #endregion
            return;
        }
        #endregion
        #endregion
        #region RemoveCopies()
        private static void RemoveCopies()
        {
            #region *EXPLANATION*
            /*Since it's very possible that each piece could be referenced
             * more than once, there's bound to be multiple copies of one 
             * piece. This removes those copies so there are less pieces to 
             * deal with.*/
            #endregion
            for (int i = 0; i < ConnectedPieces.Count; i++)
            {
                for (int k = i + 1; k < ConnectedPieces.Count; k++)
                {
                    if (ConnectedPieces[i].Register == ConnectedPieces[k].Register)
                        ConnectedPieces.RemoveAt(k);
                }
            }
            return;
        }
        #endregion
        #region CheckforCapture()
        private static void CheckforCapture()
        {
            #region *Explanation*
            /*These check each piece if they have an  opposing (or no) piece on each side: Top,
             * Left, Bottom, and Right. If there is no piece there, then the amount needed to
             * capture the group of pieces stays the same, while an opposing piece occupying  
             * the space will cause the number needed to capture the pieces to decrease. This 
             * number is handled by the opposing_amount integer. The opposing_amount 
             * increments as it moves through the checkpoints that says there is an opposing 
             * piece. The capture_prerequisite integer increments regardless of whether the
             * space being checked is null or not. It is literally the number that MUST be
             * reached in order for the pieces to be captured.*/
            #endregion
            int opposing_amount = 0;
            int capture_prerequisite = 0;
            foreach (Piece piece in ConnectedPieces)
            {
                #region piece.Top
                if (piece.Top != null)
                {
                    if (piece.Top.Symbol != piece.Symbol)
                    {
                        opposing_amount++;
                        capture_prerequisite++;
                    }
                }
                else if (piece.Top == null)
                {
                    capture_prerequisite++;
                }
                #endregion
                #region piece.Right
                if (piece.Right != null)
                {
                    if (piece.Right.Symbol != piece.Symbol)
                    {
                        opposing_amount++;
                        capture_prerequisite++;
                    }
                }
                else if (piece.Right == null)
                {
                    capture_prerequisite++;
                }
                #endregion
                #region piece.Bottom
                if (piece.Bottom != null)
                {
                    if (piece.Bottom.Symbol != piece.Symbol)
                    {
                        opposing_amount++;
                        capture_prerequisite++;
                    }
                }
                else if (piece.Bottom == null)
                {
                    capture_prerequisite++;
                }
                #endregion
                #region piece.Left
                if (piece.Left != null)
                {
                    if (piece.Left.Symbol != piece.Symbol)
                    {
                        opposing_amount++;
                        capture_prerequisite++;
                    }
                }
                else if (piece.Left == null)
                {
                    capture_prerequisite++;
                }
                #endregion
            }
            #region Check for Special Pieces
            #region *EXPLANATION
            /* Because pieces that are on the edge and in corner have special properties to their
             * capture, these piece must remove one or two pieces needed to capture away from the
             * capture_prerequisite integer. That way, you can now capture pieces both on the edge
             * and in the corner.*/
            #endregion
            foreach (Piece piece in ConnectedPieces)
            {
                if (piece.InCorner == true)
                    capture_prerequisite -= 2;
                if (piece.OnEdge == true)
                    capture_prerequisite -= 1;
            }
            #endregion
            #region Check for Equal amounts
            /*If both integers are equal to each other, then it goes to each piece and tells it that
             * it will be captured. The method then moves to the next method that will capture the
             * designated pieces.*/
            if (opposing_amount == capture_prerequisite)
            {
                foreach (Piece piece in ConnectedPieces)
                {
                    piece.Captured = true;
                }
            }
            #endregion
            return;
        }
        #endregion
        #region Capture()
        private static void Capture()
        {
            #region *EXPLANATION*
            /*For each piece that's been designated "to be destroyed
             * at sight", this method fulfills the command. It goes into 
             * the actual game board and takes them out. Don't worry:
             * no pieces were harmed in the performance of this method.*/
            #endregion
            foreach (Piece piece in ConnectedPieces)
            {
                if (piece.Captured)
                {
                    Gameboard[piece.Register.Row, piece.Register.Column] = null;
                }
            }
            ConnectedPieces.Clear();
            return;
        }
        #endregion
        #region UpdatePieces()
        public static void UpdatePieces()
        {
            #region *EPLANATION*
            /*This method updates each of the pieces on the Game board
             * to see what's around each piece. The piece's surrounding 
             * areas could either be null or contain a piece. Also, it checks
             * to see if the piece is a corner or edge piece. If so, then it
             * has special properties that must be handled later. Nothing else
             * happens in this method concerning what type of piece it is.
             * I let another method do that when it's more necessary.*/
            #endregion
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    if (Gameboard[row, column] != null)
                    {
                        /*Set Piece's Top, Right, Bottom, and Left equal
                        to the right addresses on the Gameboard.*/
                        #region Piece Update
                        #region Top
                        if (row != 0)
                        {
                            if (Gameboard[row - 1, column] == null)
                                Gameboard[row, column].Top = null;
                            else
                                Gameboard[row, column].Top = Gameboard[row - 1, column];
                        }
                        else
                            Gameboard[row, column].Top = null;
                        #endregion
                        #region Right
                        if (column != board_size_input - 1)
                        {
                            if (Gameboard[row, column + 1] == null)
                                Gameboard[row, column].Right = null;
                            else
                                Gameboard[row, column].Right = Gameboard[row, column + 1];
                        }
                        else
                            Gameboard[row, column].Right = null;
                        #endregion
                        #region Bottom
                        if (row != board_size_input - 1)
                        {
                            if (Gameboard[row + 1, column] == null)
                                Gameboard[row, column].Bottom = null;
                            else
                                Gameboard[row, column].Bottom = Gameboard[row + 1, column];
                        }
                        else
                            Gameboard[row, column].Bottom = null;
                        #endregion
                        #region Left
                        if (column != 0)
                        {
                            if (Gameboard[row, column - 1] == null)
                                Gameboard[row, column].Left = null;
                            else
                                Gameboard[row, column].Left = Gameboard[row, column - 1];
                        }
                        else
                            Gameboard[row, column].Left = null;
                        #endregion
                        #endregion
                        #region Check for Corner and Edge Pieces
                        if ((row == 0 && column == 0) ||
                            (row == 0 && column == board_size_input - 1) ||
                            (row == board_size_input - 1 && column == board_size_input - 1) ||
                            (row == board_size_input - 1 && column == 0))
                        {
                            Gameboard[row, column].InCorner = true;
                        }
                        if ((row == 0 && (column != 0 && column != board_size_input - 1)) ||
                            (row == board_size_input - 1 && (column != 0 && column != board_size_input - 1)) ||
                            ((row != 0 && row != board_size_input - 1) && column == 0) ||
                            ((row != 0 && row != board_size_input - 1) && column == board_size_input - 1))
                        {
                            Gameboard[row, column].OnEdge = true;
                        }
                        #endregion
                    }
                }
            }
            return;
        }
        #endregion
        #endregion

        #region Ko-Checking Methods
        #region CheckForKo()
        public static void CheckForKo(Piece newPiece)
        {
            CopyGameBoardToTestBoard();
            UpdatePiecesInTestBoard();
            StartTestCapturingProcess();
            CompareSnapshotsToTestBoard(newPiece);
            if (KoViolationFound)
                return;
        }
        #endregion
        #region CompareSnapshotsToTestBoard()
        public static void CompareSnapshotsToTestBoard(Piece newPiece)
        {
            Piece temp = newPiece;
            int similarities = 0;
            //int TestTurnNum = 0;
            //int SnapTurnNum = 0;
            #region SnapShotOfTurn1
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    #region Temporarily Erase Turn_Created values
                    //if (TestBoardForKo[row, column] != null)
                    //{
                    //    TestTurnNum = TestBoardForKo[row, column].Turn_Created;
                    //    TestBoardForKo[row, column].Turn_Created = 0;
                    //}
                    //if (SnapShotOfTurn1[row, column] != null)
                    //{
                    //    SnapTurnNum = SnapShotOfTurn1[row, column].Turn_Created;
                    //    SnapShotOfTurn1[row, column].Turn_Created = 0;
                    //}
                    #endregion
                    if (SnapShotOfTurn1[row, column] != null && TestBoardForKo[row, column] != null)
                    {
                        if (TestBoardForKo[row, column].Symbol == SnapShotOfTurn1[row, column].Symbol)
                        {
                            similarities++;
                        }
                    }
                    else if (SnapShotOfTurn1[row, column] == null && TestBoardForKo[row, column] == null)
                    {
                        similarities++;
                    }
                    else
                    { }
                    #region Put them back
                    //if (TestBoardForKo[row, column] != null)
                    //    TestBoardForKo[row, column].Turn_Created = TestTurnNum;
                    //if (SnapShotOfTurn1[row, column] != null)
                    //    SnapShotOfTurn1[row, column].Turn_Created = SnapTurnNum;
                    #endregion
                }
            }
            if (similarities == (board_size_input * board_size_input))
            {
                KoViolationFound = true;
                Gameboard[newPiece.Register.Row, newPiece.Register.Column] = null;
                return;
            }
            #endregion
            else
            {
                similarities = 0;
                #region SnapShotOfTurn2
                for (int row = 0; row < board_size_input; row++)
                {
                    for (int column = 0; column < board_size_input; column++)
                    {
                        #region Temporarily Erase Turn_Created values
                        //if (TestBoardForKo[row, column] != null)
                        //{
                        //    TestTurnNum = TestBoardForKo[row, column].Turn_Created;
                        //    TestBoardForKo[row, column].Turn_Created = 0;
                        //}
                        //if (SnapShotOfTurn2[row, column] != null)
                        //{
                        //    SnapTurnNum = SnapShotOfTurn2[row, column].Turn_Created;
                        //    SnapShotOfTurn2[row, column].Turn_Created = 0;
                        //}
                        #endregion
                        if (SnapShotOfTurn2[row, column] != null && TestBoardForKo[row, column] != null)
                        {
                            if (TestBoardForKo[row, column].Symbol == SnapShotOfTurn2[row, column].Symbol)
                            {
                                similarities++;
                            }
                        }
                        else if (SnapShotOfTurn2[row, column] == null && TestBoardForKo[row, column] == null)
                        {
                            similarities++;
                        }
                        else
                        { }
                        #region Put them back
                        //if (TestBoardForKo[row, column] != null)
                        //    TestBoardForKo[row, column].Turn_Created = TestTurnNum;
                        //if (SnapShotOfTurn2[row, column] != null)
                        //    SnapShotOfTurn2[row, column].Turn_Created = SnapTurnNum;
                        #endregion
                    }
                }
                if (similarities == (board_size_input * board_size_input))
                {
                    KoViolationFound = true;
                    Gameboard[newPiece.Register.Row, newPiece.Register.Column] = null;
                    return;
                }
            }
                #endregion
        }
        #endregion
        #region TakeSnapshotOfGameboardAtTurn1()
        public static void TakeSnapshotOfGameboardAtTurn1()
        {
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    if (Gameboard[row, column] != null)
                    {
                        SnapShotOfTurn1[row, column] = new Piece(Gameboard[row, column].Register.Row,
                            Gameboard[row, column].Register.Column,
                            Gameboard[row, column].Symbol,
                            Gameboard[row, column].Turn_Created);
                    }
                    else
                        SnapShotOfTurn1[row, column] = null;
                }
            }
        }
        #endregion
        #region TakeSnapshotOfGameboardAtTurn2()
        public static void TakeSnapshotOfGameboardAtTurn2()
        {
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    if (Gameboard[row, column] != null)
                    {
                        SnapShotOfTurn2[row, column] = new Piece(Gameboard[row, column].Register.Row,
                            Gameboard[row, column].Register.Column,
                            Gameboard[row, column].Symbol,
                            Gameboard[row, column].Turn_Created);
                    }
                    else
                        SnapShotOfTurn2[row, column] = null;
                }
            }
        }
        #endregion
        #region CopyGameBoardToTestBoard()
        public static void CopyGameBoardToTestBoard()
        {
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    if (Gameboard[row, column] != null)
                    {
                        TestBoardForKo[row, column] = new Piece(Gameboard[row, column].Register.Row,
                            Gameboard[row, column].Register.Column,
                            Gameboard[row, column].Symbol,
                            Gameboard[row, column].Turn_Created);
                    }
                    else
                        TestBoardForKo[row, column] = null;
                }
            }
        }
        #endregion
        #region StartTestCapturingProcess()
        public static void StartTestCapturingProcess()
        {
            #region Sort pieces in order of turn placed
            foreach (Piece piece in TestBoardForKo)
            {
                if (piece != null)
                {
                    OrderedPieces.Add(piece);
                }
            }
            for (int first_piece = 0; first_piece < OrderedPieces.Count; first_piece++)
            {
                for (int next_piece = first_piece + 1; next_piece < OrderedPieces.Count; next_piece++)
                {
                    if (OrderedPieces[first_piece].Turn_Created > OrderedPieces[next_piece].Turn_Created)
                    {
                        Piece temp = OrderedPieces[next_piece];
                        OrderedPieces[next_piece] = OrderedPieces[first_piece];
                        OrderedPieces[first_piece] = temp;
                    }
                }
            }
            #endregion
            foreach (Piece piece in OrderedPieces)
            {
                if (piece != null)
                {
                    ConnectPieces(piece);
                    RemoveCopies();
                    CheckforCapture();
                    CaptureForTestBoard();
                    UpdatePiecesInTestBoard();
                }
            }
            OrderedPieces.Clear();
        }
        #endregion
        #region UpdatePiecesInTestBoard()
        private static void UpdatePiecesInTestBoard()
        {
            for (int row = 0; row < board_size_input; row++)
            {
                for (int column = 0; column < board_size_input; column++)
                {
                    if (TestBoardForKo[row, column] != null)
                    {
                        /*Set Piece's Top, Right, Bottom, and Left equal
                        to the right addresses on the Gameboard.*/
                        #region Piece Update
                        #region Top
                        if (row != 0)
                        {
                            if (TestBoardForKo[row - 1, column] == null)
                                TestBoardForKo[row, column].Top = null;
                            else
                                TestBoardForKo[row, column].Top = TestBoardForKo[row - 1, column];
                        }
                        else
                            TestBoardForKo[row, column].Top = null;
                        #endregion
                        #region Right
                        if (column != board_size_input - 1)
                        {
                            if (TestBoardForKo[row, column + 1] == null)
                                TestBoardForKo[row, column].Right = null;
                            else
                                TestBoardForKo[row, column].Right = TestBoardForKo[row, column + 1];
                        }
                        else
                            TestBoardForKo[row, column].Right = null;
                        #endregion
                        #region Bottom
                        if (row != board_size_input - 1)
                        {
                            if (TestBoardForKo[row + 1, column] == null)
                                TestBoardForKo[row, column].Bottom = null;
                            else
                                TestBoardForKo[row, column].Bottom = TestBoardForKo[row + 1, column];
                        }
                        else
                            TestBoardForKo[row, column].Bottom = null;
                        #endregion
                        #region Left
                        if (column != 0)
                        {
                            if (TestBoardForKo[row, column - 1] == null)
                                TestBoardForKo[row, column].Left = null;
                            else
                                TestBoardForKo[row, column].Left = TestBoardForKo[row, column - 1];
                        }
                        else
                            TestBoardForKo[row, column].Left = null;
                        #endregion
                        #endregion
                        #region Check for Corner and Edge Pieces
                        if ((row == 0 && column == 0) ||
                            (row == 0 && column == board_size_input - 1) ||
                            (row == board_size_input - 1 && column == board_size_input - 1) ||
                            (row == board_size_input - 1 && column == 0))
                        {
                            TestBoardForKo[row, column].InCorner = true;
                        }
                        if ((row == 0 && (column != 0 && column != board_size_input - 1)) ||
                            (row == board_size_input - 1 && (column != 0 && column != board_size_input - 1)) ||
                            ((row != 0 && row != board_size_input - 1) && column == 0) ||
                            ((row != 0 && row != board_size_input - 1) && column == board_size_input - 1))
                        {
                            TestBoardForKo[row, column].OnEdge = true;
                        }
                        #endregion
                    }
                }
            }
            return;
        }
        #endregion
        #region CaptureForTestBoard()
        public static void CaptureForTestBoard()
        {
            foreach (Piece piece in ConnectedPieces)
            {
                if (piece.Captured)
                {
                    TestBoardForKo[piece.Register.Row, piece.Register.Column] = null;
                }
            }
            ConnectedPieces.Clear();
            return;
        }
        #endregion
        #endregion

        #region GamePlayHandler()
        public static void GamePlayHandler()
        {
            #region EXPLANATION*
            /*This method handles all of the aspects of gameplay. As soon as 
             * the StartGame() has does its part, it immediately moves to one 
             * of the cases in this method. In an effort to keep this explanation
             * short, each of the cases will have it's own explanations. They will
             * be scattered throughout each case.*/
            #endregion
            string substring = "";
            int subnum = 0;
            GS = GameStates.GameSetUp;
            do
            {
                do
                {
                    switch (GS)
                    {
                        case GameStates.GameSetUp:
                            #region *EXPLANATION*
                            /*This case sets up each player's name and symbol, and then
                             * lets you choose what size board you want. You can choose
                             * any name you want, there are 7 choices for symbols, and
                             * three board choices.*/
                            #endregion
                            #region text
                            /*This method was explained in its own method block.*/
                            StartGame();
                            if (gameOver != true && GS != GameStates.Instructions)
                            {
                                #region Name Registration
                                player1 = new Player();
                                player2 = new Player();
                                Console.Clear();
                                Console.WriteLine("What is the name of Player 1?");
                                player1Name = Console.ReadLine();
                                player1.Name = player1Name.Trim();
                                Console.WriteLine("Welcome, {0}.\n-----------------------------------------------", player1.Name);
                                do
                                {
                                    Console.WriteLine("What is the name of Player 2?");
                                    player2Name = Console.ReadLine();
                                    player2.Name = player2Name.Trim();
                                    if (player2.Name == player1.Name)
                                        Console.WriteLine("You can't have the same name as your opponent.");
                                } while (player2.Name == player1.Name);
                                Console.WriteLine("Welcome, {0}.\n-----------------------------------------------", player2.Name);
                                #endregion
                                #region Symbol Registration
                                do
                                {
                                    Console.WriteLine("{0}, pick your piece. (+, /, @, #, $, %, &)", player1.Name);
                                    player1.Symbol = Console.ReadLine();
                                    if (player1.Symbol != "+" && player1.Symbol != "/" && player1.Symbol != "@"
                                        && player1.Symbol != "#" && player1.Symbol != "$" && player1.Symbol != "%"
                                        && player1.Symbol != "&")
                                    {
                                        Console.WriteLine("Invalid piece selected.");
                                    }
                                } while (player1.Symbol != "+" && player1.Symbol != "/" && player1.Symbol != "@"
                                        && player1.Symbol != "#" && player1.Symbol != "$" && player1.Symbol != "%"
                                        && player1.Symbol != "&");
                                //------------------------------------------------------------------------------------------------------------------------Player2
                                do
                                {
                                    Console.WriteLine("{0}, pick your piece. (+, /, @, #, $, %, &)", player2.Name);
                                    player2.Symbol = Console.ReadLine();
                                    if (player2.Symbol != "+" && player2.Symbol != "/" && player2.Symbol != "@"
                                        && player2.Symbol != "#" && player2.Symbol != "$" && player2.Symbol != "%"
                                        && player2.Symbol != "&")
                                    {
                                        Console.WriteLine("Invalid piece selected.");
                                    }
                                    if (player2.Symbol == player1.Symbol)
                                        Console.WriteLine("You can't have the same symbol as your opponent.");
                                } while ((player2.Symbol != "+" && player2.Symbol != "/" && player2.Symbol != "@"
                                        && player2.Symbol != "#" && player2.Symbol != "$" && player2.Symbol != "%"
                                        && player2.Symbol != "&") || player2.Symbol == player1.Symbol);
                                #endregion
                                #region Board Registration
                                do
                                {
                                    try
                                    {
                                        Console.WriteLine("What is the size of the board? (4, 9, 13, 19)");
                                        board_size_input = int.Parse(Console.ReadLine());
                                    }
                                    catch (FormatException)
                                    {
                                        board_size_input = 0;
                                    }
                                    if (board_size_input != 4 && board_size_input != 9 && board_size_input != 13 && board_size_input != 19)
                                    {
                                        Console.WriteLine("Incorrect board diminsions entered.");
                                    }
                                    else
                                    {
                                        /*These methods were explained in their own method blocks.*/
                                        CreateComboArray(board_size_input);
                                        CreateBoard(board_size_input);
                                        PrintBoard();
                                        turn_number = 1;
                                        KoViolationFound = false;
                                        GS = GameStates.FirstTimeIn;
                                    }
                                } while (board_size_input != 4 && board_size_input != 9 && board_size_input != 13 && board_size_input != 19);
                                #endregion
                            }
                            else if (matchOver && gameOver)
                                GS = GameStates.EndGame;
                            #endregion
                            break;
                        case GameStates.Instructions:
                            #region *EXPLANATION*
                            /* These are instructions.............what, you need more info?*/
                            #endregion
                            #region Instruction Pages
                            Ins = Instructions.Page1;
                            do
                            {
                                switch (Ins)
                                {
                                    case Instructions.Page1:
                                        HandleAndPrintPage1();
                                        break;
                                    case Instructions.Page2:
                                        HandleAndPrintPage2();
                                        break;
                                    case Instructions.Page3:
                                        HandleAndPrintPage3();
                                        break;
                                    case Instructions.Page4:
                                        HandleAndPrintPage4();
                                        break;
                                    case Instructions.Page5:
                                        HandleAndPrintPage5();
                                        break;
                                }
                            } while (reply.ToUpper() != "BACK");
                            if (reply.ToUpper() == "BACK")
                                GS = GameStates.GameSetUp;
                            #endregion
                            break;
                        case GameStates.FirstTimeIn:
                            #region *EXPLANATION*
                            /*I might have to fix this later. No explanation for now.*/
                            #endregion
                            #region text
                            if (turn_number >= 2)
                            {
                                if (turn_number % 2 == 1)
                                    GS = GameStates.Tp1;
                                else
                                    GS = GameStates.Tp2;
                            }
                            else
                            {
                                GS = GameStates.Tp1;
                            }
                            #endregion
                            break;
                        case GameStates.Tp1:
                            /*Since Tp2 is identical to Tp1, I will only explain this one.*/
                            #region text
                            Console.WriteLine("{0}, Where will you place your piece? Insert row, then column (example: 2a).", player1.Name);
                            playerInput = Console.ReadLine();
                            #region Checks for Pass input
                            #region *EXPLANATION*
                            /* If the player types "pass" into the console, they pass
                             * their turn, and play goes to the next player's turn.*/
                            #endregion
                            if (playerInput.ToUpper() == "PASS")
                            {
                                turn_number++;
                                turn_pass_count++;
                                if (turn_pass_count != 2)
                                    GS = GameStates.Tp2;
                                else
                                {
                                    matchOver = true;
                                    GS = GameStates.Rematch;
                                }
                            }
                            #endregion
                            #region Handles correct input
                            else
                            {
                                /*This method was explained in its own method block.*/
                                CheckForPossibleCombo(playerInput.ToUpper());
                                if (ComboIsValid == true)
                                {
                                    #region *EXPLANATION*
                                    /* This section splits up the player's input into letters and numbers.
                                     * However, since there is a possibility of there being a number with
                                     * more than one digit, I had to handle each contingency differently.
                                     * After the input has been split, the letters gets translated (more 
                                     * info on this process in the TranslateToColumnLetters() method), 
                                     * and then the piece gets instantiated.*/
                                    #endregion
                                    #region Splits up taken-in string
                                    if (playerInput.Length == 2)
                                    {
                                        subnum = int.Parse(playerInput.Substring(0, 1)) - 1;
                                        substring = playerInput.Substring(1, 1);
                                    }
                                    else
                                    {
                                        subnum = int.Parse(playerInput.Substring(0, 2)) - 1;
                                        substring = playerInput.Substring(2, 1);
                                    }
                                    /*This method was explained in its own method block.*/
                                    TranslateToColumnLetters(substring);
                                    if (Gameboard[subnum, CL.GetHashCode()] == null)
                                    {
                                        Gameboard[subnum, CL.GetHashCode()] = new Piece(subnum, CL.GetHashCode(), player1.Symbol, turn_number);
                                    #endregion
                                        /*These methods were explained in their own method blocks.*/
                                        UpdatePieces();
                                        CheckForKo(Gameboard[subnum, CL.GetHashCode()]);
                                        if (!KoViolationFound)
                                        {
                                            StartCaptureProcess();
                                            TakeSnapshotOfGameboardAtTurn1();
                                            PrintBoard();
                                            turn_number++;
                                            turn_pass_count = 0;
                                            GS = GameStates.Tp2;
                                        }
                                        else if (KoViolationFound)
                                        {
                                            UpdatePieces();
                                            PrintBoard();
                                            Console.WriteLine("Cannot recapture this piece until you move somewhere else. Otherwise it would cause a Ko.");
                                            KoViolationFound = false;
                                            GS = GameStates.Tp1;
                                        }
                                    }
                            #endregion
                                    #region Handles incorrect/Occupied space input
                                    else
                                    {
                                        Console.WriteLine("That slot is occupied by another piece. Choose again.");
                                        GS = GameStates.Tp1;
                                    }
                                }
                                else if (ComboIsValid == false)
                                {
                                    Console.WriteLine("Invalid input. Try again.");
                                    GS = GameStates.Tp1;
                                }
                            }
                                    #endregion
                            #endregion
                            break;
                        case GameStates.Tp2:
                            #region text
                            Console.WriteLine("{0}, Where will you place your piece? Insert row, then column (example: 2a).", player2.Name);
                            playerInput = Console.ReadLine();
                            #region Handles Pass input
                            if (playerInput.ToUpper() == "PASS")
                            {
                                turn_number++;
                                turn_pass_count++;
                                if (turn_pass_count != 2)
                                    GS = GameStates.Tp1;
                                else
                                {
                                    matchOver = true;
                                    GS = GameStates.Rematch;
                                }
                            }
                            #endregion
                            #region Handles correct input
                            else
                            {
                                CheckForPossibleCombo(playerInput.ToUpper());
                                if (ComboIsValid == true)
                                {
                                    #region Splits up taken-in string
                                    if (playerInput.Length == 2)
                                    {
                                        subnum = int.Parse(playerInput.Substring(0, 1)) - 1;
                                        substring = playerInput.Substring(1, 1);
                                    }
                                    else
                                    {
                                        subnum = int.Parse(playerInput.Substring(0, 2)) - 1;
                                        substring = playerInput.Substring(2, 1);
                                    }
                                    TranslateToColumnLetters(substring);
                                    if (Gameboard[subnum, CL.GetHashCode()] == null)
                                    {
                                        Gameboard[subnum, CL.GetHashCode()] = new Piece(subnum, CL.GetHashCode(), player2.Symbol, turn_number);
                                    #endregion
                                        UpdatePieces();
                                        CheckForKo(Gameboard[subnum, CL.GetHashCode()]);
                                        if (!KoViolationFound)
                                        {
                                            StartCaptureProcess();
                                            TakeSnapshotOfGameboardAtTurn2();
                                            PrintBoard();
                                            turn_number++;
                                            turn_pass_count = 0;
                                            GS = GameStates.Tp1;
                                        }
                                        else if (KoViolationFound)
                                        {
                                            UpdatePieces();
                                            PrintBoard();
                                            Console.WriteLine("Cannot recapture this piece until you move somewhere else. Otherwise, it would cause a Ko.");
                                            KoViolationFound = false;
                                            GS = GameStates.Tp2;
                                        }
                                    }
                            #endregion
                                    #region Handles incorrect/occupied input
                                    else
                                    {
                                        Console.WriteLine("That slot is occupied by another piece. Choose again.");
                                        GS = GameStates.Tp2;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Input.");
                                    GS = GameStates.Tp2;
                                }
                            }
                                    #endregion
                            #endregion
                            break;
                    }
                } while (matchOver == false);
                switch (GS)
                {
                    case GameStates.Rematch:
                        /* This method is explained in its own method block.*/
                        AskForRematch();
                        break;
                    case GameStates.EndGame:
                        #region text
                        Console.Clear();
                        Console.WriteLine("Thanks for playing this game. \nAlthough it was created by some people a long time ago,\nit was programmed by me, Kasha. \n\n-Hope you come back to play again.-\n\nPress Enter to exit.");
                        Console.ReadLine();
                        #endregion
                        return;
                }
            } while (gameOver == false);
            return;
        }
        #endregion

        #region Instruction Page Methods
        #region Page1()
        private static void HandleAndPrintPage1()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("This is Page 1. Not sure what else I'll put yet.....maybe more.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Enter \"next\" for the next page and \"back\" to go back to the main menu.");
                reply = Console.ReadLine();
                while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK")
                {
                    Console.Clear();
                    Console.WriteLine("This is Page 1. Not sure what else I'll put yet.....maybe more.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("That is an invalid answer, you moron. Try again.\n");
                    Console.WriteLine("Enter \"next\" for the next page and \"back\" to go back to the main menu.");
                    reply = Console.ReadLine();
                }
                if (reply.ToUpper() == "NEXT")
                {
                    Ins = Instructions.Page2;
                    return;
                }
                else if (reply.ToUpper() == "PREVIOUS")
                { }
                else if (reply.ToUpper() == "BACK")
                {
                    return;
                }
            } while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK");
        }
        #endregion
        #region Page2()
        private static void HandleAndPrintPage2()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("This is not Page 1. Too bad I had to tell you that before you figured it out.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                reply = Console.ReadLine();
                while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK")
                {
                    Console.Clear();
                    Console.WriteLine("This is not Page 1. Too bad I had to tell you that before you figured it out.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("Thanks for my daily dose of idiocy, but I'm on a diet. Type a smarter answer.\n");
                    Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                    reply = Console.ReadLine();
                }
                if (reply.ToUpper() == "NEXT")
                {
                    Ins = Instructions.Page3;
                    return;
                }
                if (reply.ToUpper() == "PREVIOUS")
                {
                    Ins = Instructions.Page1;
                    return;
                }
            } while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK");
        }
        #endregion
        #region Page3()
        private static void HandleAndPrintPage3()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("This is still not Page 1, but keep trying. Maybe you'll get it soon...idiot.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                reply = Console.ReadLine();
                while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK")
                {
                    Console.Clear();
                    Console.WriteLine("This is still not not Page 1, but keep trying. Maybe you'll get it soon...idiot.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("They leave people like YOU alone with computers all day? They must \neither all be desperate, or this place is run by Mac users....\n");
                    Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                    reply = Console.ReadLine();
                }
                if (reply.ToUpper() == "NEXT")
                {
                    Ins = Instructions.Page4;
                    return;
                }
                if (reply.ToUpper() == "PREVIOUS")
                {
                    Ins = Instructions.Page2;
                    return;
                }
            } while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK");
        }
        #endregion
        #region Page4()
        private static void HandleAndPrintPage4()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("I wonder when the person reading this sentence will realize that there's nothing here? I guess never.....\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                reply = Console.ReadLine();
                while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK")
                {
                    Console.Clear();
                    Console.WriteLine("I wonder when the person reading this sentence will realize that there's nothing here? I guess never.....\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("When the time comes for you to get married, remember to never reproduce.\n");
                    Console.WriteLine("Enter \"next\" for the next page and \"previous\" for the, well, previous one,\n and \"back\" to go back to the main menu.");
                    reply = Console.ReadLine();
                }
                if (reply.ToUpper() == "NEXT")
                {
                    Ins = Instructions.Page5;
                    return;
                }
                if (reply.ToUpper() == "PREVIOUS")
                {
                    Ins = Instructions.Page3;
                    return;
                }
            } while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK");
        }
        #endregion
        #region Page5()
        private static void HandleAndPrintPage5()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Congratulations, you made it to the last page alive. As a token of our\n appreciation, we would like to present you with the special information listed below...\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Enter \"previous\" to go back to the, well, previous one,\n and \"back\" to go back to the main menu.");
                reply = Console.ReadLine();
                while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK")
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations, you made it to the last page alive. As a token of our\n appreciation, we would like to present with you the special information listed below...\n\n\n\n\n\n\n\n\n\n\n\n\n");
                    Console.WriteLine("I think I get it now: your mother and father must be siblings! Well, even\n though that's true, you have to try your hardest to follow the instructions\n below, like, right below this line....This one....right here....that I\n can't point to because they don't have an arrow character in this font\n library....You'll have to get it soon.....\n");
                    Console.WriteLine("Enter \"previous\" to go back to the, well, previous one,\n and \"back\" to go back to the main menu.");
                    reply = Console.ReadLine();
                }
                if (reply.ToUpper() == "NEXT")
                {
                    return;
                }
                if (reply.ToUpper() == "PREVIOUS")
                {
                    Ins = Instructions.Page4;
                    return;
                }

            } while (reply.ToUpper() != "NEXT" && reply.ToUpper() != "PREVIOUS" && reply.ToUpper() != "BACK");
        }
        #endregion
        #endregion
        #endregion
        static void Main(string[] args)
        {
            /*This method was explained in its own method block.*/
            GamePlayHandler();
        }
    }
}
