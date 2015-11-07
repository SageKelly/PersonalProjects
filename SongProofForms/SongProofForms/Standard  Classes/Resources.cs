using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofForms
{
    /// <summary>
    /// Has static resources for scales and notes
    /// </summary>
    public static class Resources
    {
        public enum Notes
        {
            //DO NOT GIVE THESE VALUES
            Empty,
            C,
            Cs,
            Df,
            D,
            Ds,
            Ef,
            E,
            F,
            Fs,
            Gf,
            G,
            Gs,
            Af,
            A,
            As,
            Bf,
            B
        }

        public enum Difficulties
        {
            Easy = 3000,
            Medium = 1000,
            Hard = 500
        }

        public const int LOWEST_SET = 50;
        public const int LOWEST_INC = 10;
        public const int HIGHEST_SET = 300;

        #region Scales
        private static Notes[] ScaleCMajor = { Notes.C, Notes.D, Notes.E, Notes.F, Notes.G, Notes.A, Notes.B };
        private static Notes[] ScaleCsMajor = { Notes.Cs, Notes.Ds, Notes.F, Notes.Fs, Notes.Gs, Notes.As, Notes.C };
        private static Notes[] ScaleDMajor = { Notes.D, Notes.E, Notes.Fs, Notes.G, Notes.A, Notes.B, Notes.C };
        private static Notes[] ScaleDsMajor = { Notes.Ds, Notes.F, Notes.G, Notes.Gs, Notes.As, Notes.C, Notes.D };
        private static Notes[] ScaleEMajor = { Notes.E, Notes.Fs, Notes.Gs, Notes.A, Notes.B, Notes.Cs, Notes.Ds };
        private static Notes[] ScaleFMajor = { Notes.F, Notes.G, Notes.A, Notes.Bf, Notes.C, Notes.D, Notes.E };
        private static Notes[] ScaleFsMajor = { Notes.Fs, Notes.Gs, Notes.As, Notes.B, Notes.Cs, Notes.Ds, Notes.F };
        private static Notes[] ScaleGMajor = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.Fs };
        private static Notes[] ScaleGsMajor = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.F };
        private static Notes[] ScaleAMajor = { Notes.A, Notes.B, Notes.Cs, Notes.D, Notes.E, Notes.Fs, Notes.Gs };
        private static Notes[] ScaleAsMajor = { Notes.As, Notes.C, Notes.D, Notes.Ds, Notes.F, Notes.G, Notes.A };
        private static Notes[] ScaleBMajor = { Notes.B, Notes.Cs, Notes.Ds, Notes.E, Notes.Fs, Notes.Gs, Notes.As };
        #endregion

        /// <summary>
        /// Holds the list of all versions of a scale's name
        /// </summary>
        public static List<string> ScaleNames { get; private set; }
        public static string[] ConstantScaleNames { get; private set; }
        public static Difficulties[] DifficultyLevels;
        /// <summary>
        /// The list of all unique scales
        /// </summary>
        private static List<Notes[]> Scales;
        /// <summary>
        /// A relational table mapping scale names to unique scales
        /// </summary>
        private static Dictionary<string, Notes[]> ScaleTable;
        /// <summary>
        /// Holds the string values of each of the notes for display purposes
        /// </summary>
        public static Dictionary<Notes, string> NoteValues { get; private set; }

        static bool DictionariesBuilt = false;

        /// <summary>
        /// Populates the dictionaries with scales and notes for later reference
        /// </summary>
        public static void BuildDictionaries()
        {
            if (!DictionariesBuilt)
            {
                ScaleNames = new List<string>();
                Scales = new List<Notes[]>();
                ScaleTable = new Dictionary<string, Notes[]>();
                NoteValues = new Dictionary<Notes, string>();
                DifficultyLevels = new Difficulties[3] { Difficulties.Easy, Difficulties.Medium, Difficulties.Hard };
                #region Scale Names
                ScaleNames.Add("C Major");
                ScaleNames.Add("C♯ Major");
                ScaleNames.Add("D♭ Major");
                ScaleNames.Add("D Major");
                ScaleNames.Add("D♯ Major");
                ScaleNames.Add("E♭ Major");
                ScaleNames.Add("E Major");
                ScaleNames.Add("F Major");
                ScaleNames.Add("F♯ Major");
                ScaleNames.Add("G♭ Major");
                ScaleNames.Add("G Major");
                ScaleNames.Add("G♯ Major");
                ScaleNames.Add("A♭ Major");
                ScaleNames.Add("A Major");
                ScaleNames.Add("A♯ Major");
                ScaleNames.Add("B♭ Major");
                ScaleNames.Add("B Major");
                #endregion
                ConstantScaleNames = ScaleNames.ToArray();


                #region Scales
                Scales.Add(ScaleCMajor);
                Scales.Add(ScaleCsMajor);
                Scales.Add(ScaleDMajor);
                Scales.Add(ScaleDsMajor);
                Scales.Add(ScaleEMajor);
                Scales.Add(ScaleFMajor);
                Scales.Add(ScaleFsMajor);
                Scales.Add(ScaleGMajor);
                Scales.Add(ScaleGsMajor);
                Scales.Add(ScaleAMajor);
                Scales.Add(ScaleAsMajor);
                Scales.Add(ScaleBMajor);
                #endregion

                #region ScaleName → Scale Relational Table
                ScaleTable.Add(ScaleNames[0], Scales[0]);  //"C Major");
                ScaleTable.Add(ScaleNames[1], Scales[1]);  //"C# Major");
                ScaleTable.Add(ScaleNames[2], Scales[1]);  //"Db Major");
                ScaleTable.Add(ScaleNames[3], Scales[2]);  //"D Major");
                ScaleTable.Add(ScaleNames[4], Scales[3]);  //"D# Major");
                ScaleTable.Add(ScaleNames[5], Scales[3]);  //"Eb Major");
                ScaleTable.Add(ScaleNames[6], Scales[4]);  //"E Major");
                ScaleTable.Add(ScaleNames[7], Scales[5]);  //"F Major");
                ScaleTable.Add(ScaleNames[8], Scales[6]);  //"F# Major");
                ScaleTable.Add(ScaleNames[9], Scales[6]);  //"Gb Major");
                ScaleTable.Add(ScaleNames[10], Scales[7]); //"G Major");
                ScaleTable.Add(ScaleNames[11], Scales[8]); //"G# Major");
                ScaleTable.Add(ScaleNames[12], Scales[8]); //"Ab Major");
                ScaleTable.Add(ScaleNames[13], Scales[9]); //"A Major");
                ScaleTable.Add(ScaleNames[14], Scales[10]); //"A# Major");
                ScaleTable.Add(ScaleNames[15], Scales[10]);//"Bb Major");
                ScaleTable.Add(ScaleNames[16], Scales[11]);//"B Major");
                #endregion

                #region Note Values
                NoteValues.Add(Notes.Cs, "C♯");
                NoteValues.Add(Notes.Df, "D♭");
                NoteValues.Add(Notes.D, "D");
                NoteValues.Add(Notes.Ds, "D♯");
                NoteValues.Add(Notes.Ef, "E♭");
                NoteValues.Add(Notes.E, "E");
                NoteValues.Add(Notes.F, "F");
                NoteValues.Add(Notes.Fs, "F♯");
                NoteValues.Add(Notes.Gf, "G♭");
                NoteValues.Add(Notes.G, "G");
                NoteValues.Add(Notes.Gs, "G♯");
                NoteValues.Add(Notes.Af, "A♭");
                NoteValues.Add(Notes.A, "A");
                NoteValues.Add(Notes.As, "A♯");
                NoteValues.Add(Notes.Bf, "B♭");
                NoteValues.Add(Notes.B, "B");
                #endregion

                DictionariesBuilt = true;
            }
        }

        /// <summary>
        /// Produces a manipulated scale
        /// </summary>
        /// <param name="scale_name">The scale to use. One can
        /// use the dictionaries from this for quicker and
        /// cleaner reference</param>
        /// <param name="showSharp">Denotes if this scale
        /// should represented with sharps or flats</param>
        /// <returns>The manipulated scale</returns>
        public static Scale MakeScale(string scale_name, bool showSharp)
        {
            if (!DictionariesBuilt)
                BuildDictionaries();

            Scale result = new Scale(scale_name, ScaleTable[scale_name]);
            if (!showSharp)
            {
                for (int i = 0; i < result.Notes.Length; i++)
                {
                    switch (result.Notes[i])
                    {
                        case Notes.Cs:
                            result.Notes[i] = Notes.Df;
                            break;
                        case Notes.Ds:
                            result.Notes[i] = Notes.Ef;
                            break;
                        case Notes.Fs:
                            result.Notes[i] = Notes.Gf;
                            break;
                        case Notes.Gs:
                            result.Notes[i] = Notes.Af;
                            break;
                        case Notes.As:
                            result.Notes[i] = Notes.Bf;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a the notes for the current session
        /// </summary>
        /// <param name="s">The scale being used in the session</param>
        /// <param name="noteCount">The amount of notes quizzed</param>
        /// <returns>An array of ints directly related to the scale used</returns>
        public static int[] MakeQuiz(Scale s, int noteCount)
        {
            int temp = 0;
            if (!DictionariesBuilt)
                BuildDictionaries();
            int[] results = new int[noteCount];
            Random rng = new Random();
            for (int i = 0; i < results.Length; i++)
            {
                temp = rng.Next(0, s.Notes.Length);
                if (i != 0)
                {
                    if (results[i - 1] == temp)
                    {
                        temp = (temp + 1) % s.Notes.Length;
                    }
                }
                results[i] = temp;
            }
            return results;
        }

    }
}