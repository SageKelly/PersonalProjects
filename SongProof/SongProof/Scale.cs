using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleProof
{
    /// <summary>
    /// Reprsents the notes of a particular Scale
    /// </summary>
    public class Scale
    {
        /// <summary>
        /// The notes of a scale
        /// </summary>
        public Resources.Notes[] Notes;
        public string Name;
        public Scale(string name, Resources.Notes[] notes)
        {
            Name = name;
            Notes = notes;
        }

        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4, Resources.Notes n5)
        {
            Name = name;
            Notes = new Resources.Notes[5] { n1, n2, n3, n4, n5 };
        }

        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3,
            Resources.Notes n4, Resources.Notes n5, Resources.Notes n6)
        {
            Name = name;
            Notes = new Resources.Notes[6] { n1, n2, n3, n4, n5, n6 };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3,
            Resources.Notes n4, Resources.Notes n5, Resources.Notes n6, Resources.Notes n7)
        {
            Name = name;
            Notes = new Resources.Notes[7] { n1, n2, n3, n4, n5, n6, n7 };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4,
            Resources.Notes n5, Resources.Notes n6, Resources.Notes n7, Resources.Notes n8)
        {
            Name = name;
            Notes = new Resources.Notes[8] { n1, n2, n3, n4, n5, n6, n7, n8 };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4,
           Resources.Notes n5, Resources.Notes n6, Resources.Notes n7, Resources.Notes n8, Resources.Notes n9)
        {
            Name = name;
            Notes = new Resources.Notes[9] { n1, n2, n3, n4, n5, n6, n7, n8, n9 };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4, Resources.Notes n5,
            Resources.Notes n6, Resources.Notes n7, Resources.Notes n8, Resources.Notes n9, Resources.Notes n10)
        {
            Name = name;
            Notes = new Resources.Notes[10] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4, Resources.Notes n5, Resources.Notes n6,
            Resources.Notes n7, Resources.Notes n8, Resources.Notes n9, Resources.Notes n10, Resources.Notes n11)
        {
            Name = name;
            Notes = new Resources.Notes[11] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11 };
        }
        public Scale(string name, Resources.Notes n1, Resources.Notes n2, Resources.Notes n3, Resources.Notes n4, Resources.Notes n5, Resources.Notes n6,
            Resources.Notes n7, Resources.Notes n8, Resources.Notes n9, Resources.Notes n10, Resources.Notes n11, Resources.Notes n12)
        {
            Name = name;
            Notes = new Resources.Notes[12] { n1, n2, n3, n4, n5, n6, n7, n8, n9, n10, n11, n12 };
        }


    }
}
