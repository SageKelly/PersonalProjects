using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Box
{
    public static class Note
    {
        private static string[] MajorNotes;
        private static string[] Accidentals;
        private static string[][] Notes;
        //A♯ / B♭

        /// <summary>
        /// Will make a random table of Note objects
        /// </summary>
        /// <param name="table">the array in which to place the Note objects</param>
        public static void MakeTable(out string[,] table)
        {
            table = new string[3, 4];

            Random rand = new Random();

            bool[] MajorBools = new bool[7] { false, false, false, false, false, false, false };
            bool[] AcciBools = new bool[10] { false, false, false, false, false, false, false, false, false, false };
            bool[][] Bools = new bool[2][] { MajorBools, AcciBools };

            for (int i = 0; i < 12; i++)
            {
                int r = i / 3;
                int c = i % 4;

                int arr_num = rand.Next(0, 10) % 2;

                int note_num = rand.Next(0, Bools[arr_num].Length);

                while (Bools[arr_num][note_num])
                {
                    note_num = (note_num + 1) % Bools[arr_num].Length;
                }

                table[r, c] = Notes[arr_num][note_num];

                Bools[arr_num][note_num] = true;
                if (arr_num == 1)//If it's the boolean array...
                {
                    //check one more array as used.
                    if (note_num % 2 == 0)
                        AcciBools[note_num + 1] = true;
                    else
                        AcciBools[note_num - 1] = true;
                }
            }
        }

        public static List<string> MakeTable()
        {
            List<string> table = new List<string>();
            SetupArrays();
            Random rand = new Random();

            bool[] MajorBools = new bool[7] { false, false, false, false, false, false, false };
            bool[] AcciBools = new bool[10] { false, false, false, false, false, false, false, false, false, false };
            bool[][] Bools = new bool[2][] { MajorBools, AcciBools };

            for (int i = 0; i < 12; i++)
            {
                int arr_num = rand.Next(0, 10) % 2;

                int note_num = rand.Next(0, Bools[arr_num].Length);

                int j = 0;
                while (Bools[arr_num][note_num])
                {
                    note_num = (note_num + 1) % Bools[arr_num].Length;
                    j++;
                    if (j >= Bools[arr_num].Length)
                    {
                        arr_num = (arr_num + 1) % 2;
                        j = 0;
                        note_num = 0;
                    }
#if DEBUG
                    Debug.WriteLine("a: {0}, n: {1}, j: {2}", arr_num, note_num, j);
#endif
                }

                table.Add(Notes[arr_num][note_num]);

                Bools[arr_num][note_num] = true;
                if (arr_num == 1)//If it's the boolean array...
                {
                    //check one more array as used.
                    if (note_num % 2 == 0)
                        AcciBools[note_num + 1] = true;
                    else
                        AcciBools[note_num - 1] = true;
                }
            }
            return table;
        }

        private static void SetupArrays()
        {
            MajorNotes = new string[7];
            MajorNotes[0] = "A";
            MajorNotes[1] = "B";
            MajorNotes[2] = "C";
            MajorNotes[3] = "D";
            MajorNotes[4] = "E";
            MajorNotes[5] = "F";
            MajorNotes[6] = "G";

            Accidentals = new string[10];
            Accidentals[0] = "A♯";
            Accidentals[1] = "B♭";
            Accidentals[2] = "C♯";
            Accidentals[3] = "D♭";
            Accidentals[4] = "D♯";
            Accidentals[5] = "E♭";
            Accidentals[6] = "F♯";
            Accidentals[7] = "G♭";
            Accidentals[8] = "G♯";
            Accidentals[9] = "A♭";

            Notes = new string[2][];
            Notes[0] = MajorNotes;
            Notes[1] = Accidentals;
        }
    }
}
