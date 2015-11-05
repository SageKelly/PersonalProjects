using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsUI
{
    public class Rectangle
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        /// <summary>
        /// A Position of (0, 0)
        /// </summary>
        public static Rectangle Zero
        {
            get
            {
                return new Rectangle();
            }
        }

        /// <summary>
        /// A Position of (1, 1)
        /// </summary>
        public static Rectangle One
        {
            get
            {
                return new Rectangle(1, 1, 1, 1);
            }
        }

        /// <summary>
        /// Creates a basic Position of (0,0)
        /// </summary>
        public Rectangle()
        {
            X = Y = Width = Height = 0;
        }

        /// <summary>
        /// Creates a basic Position
        /// </summary>
        /// <param name="x">The starting X position</param>
        /// <param name="y">The starting Y position</param>
        /// <param name="width">The starting width of the Rectangle</param>
        /// <param name="height">The starting height of the Rectangle</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
