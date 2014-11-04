using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PicAnimator
{
    class LoopingException : Exception
    {
        public string ErrorText
        {
            get;
            private set;
        }

        public LoopingException(string text = "Looping data already exists. To add finite data to infinite"+
            ", it must be added before the infinite data.")
            : base(text)
        {
            ErrorText = text;
            if (text == null)
                ErrorText = ToString();
        }

        public override string ToString()
        {
            return "Looping data already exists. To add finite data to infinite, it must be added before the infinite data.";
        }
    }
}
