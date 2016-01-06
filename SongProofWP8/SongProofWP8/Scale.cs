using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    /// <summary>
    /// Reprsents the notes of a particular Scale
    /// </summary>
    public class Scale
    {
        /// <summary>
        /// The notes of a scale
        /// </summary>
        public string[] Notes;
        public string Name;
        public Scale(string name, string[] notes)
        {
            Name = name;
            Notes = notes;
        }

        public Scale(string name, string n1, string n2, string n3, string n4, string n5)
        {
            Name = name;
            Notes = new string[5] { n1, n2, n3, n4, n5 };
        }

        public Scale(string name, string n1, string n2, string n3,
            string n4, string n5, string n6)
        {
            Name = name;
            Notes = new string[6] { n1, n2, n3, n4, n5, n6 };
        }
        public Scale(string name, string n1, string n2, string n3,
            string n4, string n5, string n6, string n7)
        {
            Name = name;
            Notes = new string[7] { n1, n2, n3, n4, n5, n6, n7 };
        }
        public Scale(string name, string n1, string n2, string n3, string n4,
            string n5, string n6, string n7, string n8)
        {
            Name = name;
            Notes = new string[8] { n1, n2, n3, n4, n5, n6, n7, n8 };
        }
        public Scale(string name, string n1, string n2, string n3, string n4,
           string n5, string n6, string n7, string n8, string n9)
        {
            Name = name;
            Notes = new string[9] { n1, n2, n3, n4, n5, n6, n7, n8, n9 };
        }
        public Scale(string name, string n1, string n2, string n3, string n4, string n5,
            string n6, string n7, string n8, string n9, string n10)
        {
            Name = name;
            Notes = new string[10] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, };
        }
        public Scale(string name, string n1, string n2, string n3, string n4, string n5, string n6,
            string n7, string n8, string n9, string n10, string n11)
        {
            Name = name;
            Notes = new string[11] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11 };
        }
        public Scale(string name, string n1, string n2, string n3, string n4, string n5, string n6,
            string n7, string n8, string n9, string n10, string n11, string n12)
        {
            Name = name;
            Notes = new string[12] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12 };
        }


    }
}
