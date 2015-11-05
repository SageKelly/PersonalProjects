using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsUI
{
    public class TextBlock : Element
    {
        public string Text;

        public TextBlock()
        {
            Text = "";
            Rect = Rectangle.One;
        }

        public TextBlock(Element parent)
            : this()
        {
            Parent = parent;
        }

        public void Print()
        {

        }

    }
}
