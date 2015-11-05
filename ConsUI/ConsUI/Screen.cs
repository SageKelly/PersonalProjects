using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsUI
{
    public class Screen:Element
    {
        bool WordWrap;

        public List<Element> Elements;

        public Screen(Rectangle rect)
        {
            Rect = rect;
        }



    }
}
