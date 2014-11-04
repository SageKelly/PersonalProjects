using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicAnimator_V1_1
{
    class LoopingException : Exception
    {
        public LoopingException()
        {
        }

        public override string ToString()
        {
            return "Looping data already exists. To add finite data to infinite, it must be added before the infinite data.";
        }
    }
}
