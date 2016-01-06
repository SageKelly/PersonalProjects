using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scale_Proof_Console
{
    class Program
    {
        public enum GameStates
        {
            Main,
            Setup,
            Game,
            Settings,
            Pause,
            Results,
            Analytics
        }
        public static GameStates gameState;


        public int SetLength = Resources.LOWEST_SET;

        public static Session curSession;

        public static Resources.Difficulties chosenDifficulty;
        public static string chosenScale;
        public static bool isSharp;

        /// <summary>
        /// used for determining what
        /// </summary>
        static List<int> SetupList;

        #region Screen Strings
        string StrStartGame = "Start Game";
        string StrSettings = "Settings";
        string StrManageData = "Manage Data";
        #endregion
        public static Thread ReadKey;

        public static ConsoleKeyInfo CKI;

        #region Print Regions
        int center_X = Console.WindowWidth / 2;
        int center_Y = Console.WindowHeight / 2;
        #endregion

        static void Main(string[] args)
        {
            ReadKey = new Thread(HandleInput);

        }

        public static void Setup()
        {
            Scale tempScale = Resources.MakeScale(chosenScale, isSharp);
            curSession = new Session(chosenDifficulty, tempScale, Resources.MakeQuiz(tempScale, chosenDifficulty));
        }

        public static void SetupGame()
        {
            //Scale

            //Length

            //Difficulty
        }

        public static void PrintMain()
        {

        }

        public static void PrintSetup()
        {

        }

        public static void PrintGame()
        {

        }

        public static void PrintResult()
        {

        }

        public static void HandleInput()
        {
            CKI = Console.ReadKey(true);
            switch (gameState)
            {
                case GameStates.Analytics:
                    break;
                case GameStates.Game:
                    break;
                case GameStates.Main:
                    break;
                case GameStates.Pause:
                    break;
                case GameStates.Results:
                    break;
                case GameStates.Settings:
                    break;
                case GameStates.Setup:
                    switch (CKI.Key)
                    {
                        case ConsoleKey.Tab:
                            break;
                        case ConsoleKey.UpArrow:
                            break;
                        case ConsoleKey.DownArrow:
                            break;
                    }
                    break;
                default:
                    gameState = GameStates.Main;
                    break;
            }
        }

    }
}
