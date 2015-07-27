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
            Start,
            Game,
            Settings,
            Pause,
            Results,
            Analytics
        }
        public GameStates gameStates;

        public const int EASY = 3000;
        public const int MEDIUM = 1000;
        public const int HARD = 500;

        public const int LOWEST_SET = 50;
        public int SetLength = LOWEST_SET;
        public const int HIGHEST_SET = 300;

        public Resources.Notes[] Scale;

        public List<int>SetMaker;

        public int[] ConcreteSet;

        Thread ReadKey;

        ConsoleKeyInfo CKI;

        #region Print Regions

        #endregion

        static void Main(string[] args)
        {


        }

        
    }
}
