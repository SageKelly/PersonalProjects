using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{
    class Program
    {
        #region Serialized Object
       
        #endregion
        private static SerialObject SO;
        #region Screen Stuff        
        static Screen StartScreen;
        static Screen MenuScreen;
        static Screen DataScreen;
        

        #endregion

        static void Main(string[] args)
        {
        }

        static void Setup()
        {
            StartScreen = new Screen(0, 0, Console.WindowWidth - 1, Console.WindowHeight);
            StartScreen.AddTextBlock(StartScreen.Width / 2, StartScreen.Height / 2, "Start Game");
            StartScreen.AddTextBlock(StartScreen.Width/ 2, (StartScreen.Height / 2)+1, "Menu");

            MenuScreen = new Screen(0, 0, Console.WindowWidth - 1, Console.WindowHeight);
            MenuScreen.AddTextBlock(MenuScreen.Width / 2, MenuScreen.Height / 2, "Data");
            MenuScreen.AddTextBlock(MenuScreen.Width/2, (MenuScreen.Height/2)+1, "Quit");

            DataScreen = new Screen(0, 0, Console.WindowWidth, Console.WindowHeight);
            DataScreen.AddTextBlock(DataScreen.Width / 2, DataScreen.Height / 2, "Select Data to Delete");
            DataScreen.AddTextBlock(DataScreen.Width / 2, (DataScreen.Height / 2) + 1, "Delete All");


        }
    }
}
