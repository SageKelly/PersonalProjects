using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace IsoGridWorld
{
    public class DirProb
    {
        /// <summary>
        /// Direction/ Action
        /// </summary>
        public Direction Dir;
        /// <summary>
        /// Utility of action
        /// </summary>
        public float Utility;
        /// <summary>
        /// Denotes whether or not this is a valid action
        /// </summary>
        public bool IsValid;

        private int i_trod_number;

        int Red, Green, Blue;

        public Color DrawColor;

        /// <summary>
        /// Denotes how many times this action was taken
        /// </summary>
        public int TrodNumber
        {
            get
            {
                return i_trod_number;
            }
            set
            {
                int temp = i_trod_number;
                i_trod_number = value;
                if (temp < i_trod_number)
                {
                    EditArrowDraw();
                }
            }
        }
        public DirProb()
        {

        }
        public DirProb(Direction direction, float utility, bool isValid = true)
        {
            Dir = direction;
            Utility = utility;
            IsValid = isValid;
            i_trod_number = 0;
            Red = 255; Green = 255; Blue = 255;
            DrawColor = new Color(Red, Green, Blue);
        }

        private void EditArrowDraw()
        {
            DrawColor.B -= 2;
            DrawColor.G -= 2;
        }

        public void ResetTrodding()
        {
            DrawColor.B = 255;
            DrawColor.G = 255;
            i_trod_number = 0;
        }

    }
}
