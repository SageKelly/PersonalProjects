using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{
    /// <summary>
    /// Represents a section of the console screen
    /// </summary>
    public class Screen : IDrawable
    {
        /// <summary>
        /// The TextBlocks that belong to the Screen object
        /// </summary>
        public List<TextBlock> TextBlocks { get; private set; }

        private int _width;
        private int _height;
        int left, top;
        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                if (value != left)
                {
                    left = value;
                    Print();
                }
            }
        }
        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                if (value != top)
                {
                    top = value;
                    Print();
                }
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
        }

        /// <summary>
        /// The minimum dimensions possible for the height and width for the screen
        /// </summary>
        const int MIN_DIM = 10;

        int textFocus;

        public bool NeedsUpdating { get; set; }

        /// <summary>
        /// Keeps note of the currently selected TextBlock
        /// </summary>
        int currentSelection;


        private Screen()
        {
            Left = Top = 0;
            _width = _height = MIN_DIM;
            textFocus = 0;
        }

        //                                                                
        //                               _ _ _ _ _ _ _ _                  
        //                           _ 7                 0                
        //                         -                       Bx             
        //                      x                            x            
        //                    x                                x          
        //                  x                                   x         
        //                x                                      x        
        //              x                                         x       
        //             x                                           x      
        //            x                                            x      
        //           x                                              B     
        //          x     _ _ _ _ _      _ _ _ _ _ _                |     
        //          x  -           -    /                           |     
        //         x      _______   \  / _______           _ _ _    |     
        //         x     / ( @ ) \   |  / ( @ ) \         /  o\ B   |     
        //          x    \_(_@_)_/   |  \_(_@_)_/        x oo    |  |     
        //           x              /                   /\   o_  /  |     
        //           x             /                   |  \    /    |     
        //           x            /                    |0  |  /     |     
        //          x            /                     |o    /      |     
        //         x            /                      \    |       |     
        //         x        (\ / \  /)                  \_ o        |     
        //         x        /_(__ )__\                  |           |     
        //          x                                               |     
        //           x                      .                       |     
        //            x        _________                            |     
        //             x      /__ _ _ __\                           |     
        //              x     \_________/        x                   o    
        //                x                    x                     o    
        //                 xx               xx                        o   
        //                   x    s       x o                         o   
        //                    \_ _^_ _ _ / o                           o 
        //                               o                              o 

        /// <summary>
        /// Creates a drawable screen object
        /// </summary>
        /// <param name="left">the x coordinate of the screen</param>
        /// <param name="top">the y coordinate of the screen</param>
        /// <param name="width">the width of the screen</param>
        /// <param name="height">the height</param>
        public Screen(int left, int top, int width, int height)
        {
            //width
            if (width > 10 && width <= Console.WindowWidth)
                _width = width;
            //height
            if (height > 10 && height <= Console.WindowHeight)
                _height = height;
        }

        /// <summary>
        /// Prints the screen and its textblocks
        /// </summary>
        public void Print()
        {
            //store cursor location
            int tempLeft = Console.CursorLeft;
            int tempTop = Console.CursorTop;

            Console.CursorLeft = Left;
            Console.CursorTop = Top;

            Console.Write("+");

            //top bar
            for (int i = 1; i < _width - 1; i++)
            {
                Console.Write("-");
            }

            Console.Write("+");

            //vertical lines
            if (Left != 0)
            {
                for (int i = 1; i < _height - 1; i++)
                {
                    Console.CursorLeft = Left;
                    Console.CursorTop = i + Top;
                    Console.Write("|");
                }
            }

            //textblock
            foreach (TextBlock tb in TextBlocks)
            {
                tb.Print();
            }

            //bottom bar
            Console.CursorLeft = Left;
            Console.CursorTop = Top + _height;

            Console.Write("+");

            for (int i = 0; i < _width - 1; i++)
            {
                Console.Write("-");
            }

            Console.Write("+");

            //reset cursor
            Console.CursorLeft = tempLeft;
            Console.CursorTop = tempTop;
            NeedsUpdating = false;
        }

        /// <summary>
        /// Adds a TextBlock to the screen;  NOTE: TextBlocks added this way
        /// maintain complete control, and will NOT be subject to relative screen-
        /// based displacement.
        /// </summary>
        /// <param name="tb">The TextBlock to add.</param>
        public void AddTextBlock(TextBlock tb)
        {
            TextBlocks.Add(tb);
        }

        /// <summary>
        /// Adds a TextBlock to the screen; TextBlocks added this way
        /// are subject relative screen-based displacement.
        /// </summary>
        /// <param name="left">the x coordinate of the TextBlock</param>
        /// <param name="top">the y coordinate of the TextBlock</param>
        /// <param name="text">the text of the TextBlock</param>
        public void AddTextBlock(int left, int top, string text)
        {
            AddTextBlock(new TextBlock(left + Left, top + Top, text));
        }

        /// <summary>
        /// Adds a TextBlock to the screen; TextBlocks added this way
        /// are subject relative screen-based displacement.
        /// </summary>
        /// <param name="left">the x coordinate of the TextBlock</param>
        /// <param name="top">the y coordinate of the TextBlock</param>
        /// <param name="text">the text of the TextBlock</param>
        /// <param name="padLeft">Denotes whether or not the TextBlock will be padded</param>
        /// <param name="pad">The amount of pad to the string</param>
        public void AddTextBlock(int left, int top, string text, bool padLeft, int pad = 0)
        {
            AddTextBlock(new TextBlock(left + Left, top + Top, text, padLeft, pad));
        }

        /// <summary>
        /// Sets the background color for the Screen's TextBlocks for selection highlighting
        /// </summary>
        /// <param name="color">The color of the background once a TextBlock is highlighted</param>
        public void SetSelectColor(ConsoleColor color)
        {
            foreach(TextBlock tb in TextBlocks)
            {
                tb.BackgroundColor = color;
            }
        }

        /// <summary>
        /// Selects the next available TextBlock
        /// </summary>
        public void NextTextBlock()
        {
            //unselected the last one
            TextBlocks[currentSelection].InFocus = false;
            //Increments selection
            currentSelection = (currentSelection + 1) % TextBlocks.Count;
            //select the new one
            TextBlocks[currentSelection].InFocus = true;
        }

        /// <summary>
        /// Selects the previously-available TextBlock
        /// </summary>
        public void PreviousTextBlock()
        {
            //unselected the last one
            TextBlocks[currentSelection].InFocus = false;
            //Decrements selection
            currentSelection = currentSelection - 1 == 0 ? TextBlocks.Count - 1 : currentSelection - 1;
            //select the new one
            TextBlocks[currentSelection].InFocus = true;
        }
    }
}
