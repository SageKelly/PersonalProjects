using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{

    /// <summary>
    /// An object used for writing discrete canned text to a precise location on the console
    /// </summary>
    public class TextBlock : IDrawable
    {
        /// <summary>
        /// the x coordinate for the console
        /// </summary>
        public int Left;
        /// <summary>
        /// the y coordinate for the console
        /// </summary>
        public int Top;
        /// <summary>
        /// What the button says
        /// </summary>
        public string Text;
        /// <summary>
        /// the left padding of the textblock
        /// </summary>
        public int PadLeft;
        /// <summary>
        /// the right paddin of the textlblock
        /// </summary>
        public int PadRight;

        /// <summary>
        /// The color of the text when printed
        /// </summary>
        public ConsoleColor TextColor;

        private ConsoleColor backgroundColor;

        /// <summary>
        /// The color of the background when printed.
        /// Used when textblock is highlighted.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                if (backgroundColor != Console.BackgroundColor)
                {
                    bgColorSetup = true;
                }
                else
                {
                    bgColorSetup = false;
                }
            }
        }

        /// <summary>
        /// Denotes whether or not background color has been setup
        /// </summary>
        bool bgColorSetup;

        bool inFocus;

        /// <summary>
        /// Determines if the textblock is currently highlighted
        /// </summary>
        public bool InFocus
        {
            get
            {
                return inFocus;
            }
            set
            {
                if (value != InFocus)
                {
                    //a change has occurred
                    InFocus = value;
                }

            }
        }

        public bool NeedsUpdating { get; set; }


        /// <summary>
        /// Creates a textblock
        /// </summary>
        public TextBlock()
        {
            Left = Top = 0;
            Text = "";
            PadLeft = PadRight = 0;
            InFocus = false;
        }

        /// <summary>
        /// Creates a textblock
        /// </summary>
        /// <param name="left">the x coordinate for the console</param>
        /// <param name="top">the y coordinate of the console</param>
        /// <param name="text">the text for the textblock</param>
        public TextBlock(int left, int top, string text)
            : this()
        {
            if (left > -1 && left < Console.BufferWidth)
                Left = left;
            if (top > -1 && top < Console.BufferHeight)
                Top = top;
            this.Text = text;
        }

        /// <summary>
        /// Creates a TextBlock 
        /// </summary>
        /// <param name="left">the x coordinate for the console</param>
        /// <param name="top">the y coordinate of the console</param>
        /// <param name="text">the text for the textblock</param>
        /// <param name="padLeft">Denotes whether or not the TextBlock will be padded</param>
        /// <param name="pad">The amount of pad to the string</param>
        public TextBlock(int left, int top, string text, bool padLeft, int pad = 0)
            : this(left, top, text)
        {
            if (pad > 0 && pad < Console.WindowWidth - text.Length)
            {
                if (padLeft)
                    PadLeft = pad;
                else
                    PadRight = pad;
            }
        }

        /// <summary>
        /// Creates a textblock
        /// </summary>
        /// <param name="left">the x coordinate for the console</param>
        /// <param name="top">the y coordinate of the console</param>
        /// <param name="text">the text for the textblock</param>
        /// <param name="padRight"the right padding for the textblock></param>
        public TextBlock(int left, int top, string text, int padRight)
            : this(left, top, text)
        {
            this.PadRight = padRight;
        }

        /// <summary>
        /// Prints the textblock
        /// The console's cursor will not be disturbed
        /// </summary>
        public void Print()
        {
            //store cursor location
            int tempLeft = Console.CursorLeft;
            int temptop = Console.CursorTop;

            Console.CursorLeft = Left;
            Console.CursorTop = Top;

            //print button
            if (PadLeft != 0)
                Console.Write(Text.PadLeft(PadLeft));
            else if (PadRight != 0)
                Console.Write(Text.PadRight(PadRight));
            else
                Console.Write(Text);

            //reset cursor
            Console.CursorLeft = tempLeft;
            Console.CursorTop = temptop;
            NeedsUpdating = false;
        }

    }
}
