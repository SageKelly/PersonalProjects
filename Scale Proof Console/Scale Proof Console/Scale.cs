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
        public Scale(string name, Resources.Notes[] notes)
        {
            Name = name;
            Notes = notes;
        }

    }
}
