using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class TextBlock
    {
        /// <summary>
        /// Represents the lines of text within this TextBlock
        /// </summary>
        string[] Lines;
        public int Left;
        public int Top;

        public TextBlock()
        {
            Lines = new string[1];
            Left = Top = 0;
        }

        public TextBlock(string[] lines, int left, int top)
            : this()
        {
            Lines = lines;
            Left = left;
            Top = top;
        }

        public void SetPosition(int left , int top)
        {
            Left = left;
            Top = top;
        }

        public void Print()
        {
            int tempLeft = Console.CursorLeft;
            int tempTop = Console.CursorTop;
            for (int i = 0; i < Lines.Length; i++)
            {
                Console.CursorLeft = Left;
                Console.CursorTop = Top + i;
                Console.Write(Lines[i]);
            }
        }
    }
}
