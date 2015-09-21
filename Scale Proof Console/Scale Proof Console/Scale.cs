using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale_Proof_Console
{
    /// <summary>
    /// Reprsents the notes of a particular Scale
    /// </summary>
    [Serializable]
    public class Scale
    {
        /// <summary>
        /// The notes of a scale
        /// </summary>
        public Resources.Notes[] Notes;
        public string Name;

        /// <summary>
        /// Creates a Scale
        /// </summary>
        /// <param name="name">the string-based name of the scale. Refer to the Resource Class for determination.</param>
        public Scale(string name, bool isSharp = true)
        {
            Name = name;
            Notes = Resources.MakeScale(name, isSharp);
        }

    }
}
