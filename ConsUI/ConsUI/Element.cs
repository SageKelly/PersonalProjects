using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsUI
{
    public abstract class Element
    {
        public Rectangle Rect;
        public int BorderThickness = 0;
        public bool inFocus;
        public Element Parent;
    }
}
