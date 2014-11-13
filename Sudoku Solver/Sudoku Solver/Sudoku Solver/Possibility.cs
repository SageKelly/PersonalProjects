using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    public class Possibility
    {
        public string Number;
        public bool IsUnique;

        /// <summary>
        /// A possible number a given space can be
        /// </summary>
        /// <param name="num">The possible number</param>
        /// <param name="unique">Whether or not the number is unique to the other spaces</param>
        public Possibility(string num, bool unique)
        {
            Number = num;
            IsUnique = unique;
        }
    }
}
