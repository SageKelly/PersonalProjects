using ScaleProof.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleProof
{
    /// <summary>
    /// Has static resources for scales and notes
    /// </summary>
    public static class Resources
    {
        public enum Notes
        {
            //DO NOT GIVE THESE VALUES
            C ,
            Cs,
            Df,
            D ,
            Ds,
            Ef,
            E ,
            F ,
            Fs,
            Gf,
            G ,
            Gs,
            Af,
            A ,
            As,
            Bf,
            B 
        }
        public Notes Note;

        public enum Difficulties
        {
            Easy = 3000,
            Medium = 1000,
            Hard = 500
        }

        public static Notes[] ScaleCMajor = { Notes.C, Notes.D, Notes.E, Notes.F, Notes.G, Notes.A, Notes.B };
        public static Notes[] ScaleCsMajor = { Notes.Cs, Notes.Ds, Notes.F, Notes.Fs, Notes.Gs, Notes.As, Notes.C };
        public static Notes[] ScaleDMajor= { Notes.D, Notes.E, Notes.Fs, Notes.G, Notes.A, Notes.B, Notes.C };
        public static Notes[] ScaleDsMajor = { Notes.Ds, Notes.F, Notes.G, Notes.Gs, Notes.As, Notes.C, Notes.D };
        public static Notes[] ScaleEMajor= { Notes.E, Notes.Fs, Notes.Gs, Notes.A, Notes.B, Notes.Cs, Notes.Ds };
        public static Notes[] ScaleFMajor= { Notes.F, Notes.G, Notes.A, Notes.Bf, Notes.C, Notes.D, Notes.E };
        public static Notes[] ScaleFsMajor = { Notes.Fs, Notes.Gs, Notes.As, Notes.B, Notes.Cs, Notes.Ds, Notes.F };
        public static Notes[] ScaleGMajor= { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.Fs };
        public static Notes[] ScaleGsMajor = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.F };
        public static Notes[] ScaleAMajor= { Notes.A, Notes.B, Notes.Cs, Notes.D, Notes.E, Notes.Fs, Notes.Gs };
        public static Notes[] ScaleAsMajor = { Notes.As, Notes.C, Notes.D, Notes.Ds, Notes.F, Notes.G, Notes.A };
        public static Notes[] ScaleBMajor= { Notes.B, Notes.Cs, Notes.Ds, Notes.E, Notes.Fs, Notes.Gs, Notes.As };

        /// <summary>
        /// Holds the list of all versions of a scale's name
        /// </summary>
        public static List<string>ScaleNames;
        /// <summary>
        /// The list of all unique scales
        /// </summary>
        public static List<Notes[]> Scales;
        /// <summary>
        /// A relational table mapping scale names to unique scales
        /// </summary>
        public static Dictionary<string, Notes[]> ScaleTable { get; private set; }
        /// <summary>
        /// Holds the string values of each of the notes for display purposes
        /// </summary>
        public static Dictionary<Notes, string> NoteValues { get; private set; }

        static bool DictionariesBuilt = false;

        /// <summary>
        /// Populates the dictionaries with scales and notes for later reference
        /// </summary>
        private static void BuildDictionary()
        {
            ScaleNames = new List<string>();
            Scales = new List<Notes[]>();
            ScaleTable = new Dictionary<string, Notes[]>();
            NoteValues = new Dictionary<Notes, string>();

            ScaleNames.Add("C Major");
            ScaleNames.Add("C# Major");
            ScaleNames.Add("Db Major");
            ScaleNames.Add("D Major");
            ScaleNames.Add("D# Major");
            ScaleNames.Add("Eb Major");
            ScaleNames.Add("E Major");
            ScaleNames.Add("F Major");
            ScaleNames.Add("F# Major");
            ScaleNames.Add("Gb Major");
            ScaleNames.Add("G Major");
            ScaleNames.Add("G# Major");
            ScaleNames.Add("Ab Major");
            ScaleNames.Add("A Major");
            ScaleNames.Add("A# Major");
            ScaleNames.Add("Bb Major");
            ScaleNames.Add("B Major");
            ScaleNames.Add("C Minor");
            ScaleNames.Add("C Minor");
            ScaleNames.Add("C Minor");


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


            NoteValues.Add(Notes.Cs, "C#");
            NoteValues.Add(Notes.Df, "Db");
            NoteValues.Add(Notes.D, "D");
            NoteValues.Add(Notes.Ds, "D#");
            NoteValues.Add(Notes.Ef, "Eb");
            NoteValues.Add(Notes.E, "E");
            NoteValues.Add(Notes.F, "F");
            NoteValues.Add(Notes.Fs, "F#");
            NoteValues.Add(Notes.Gf, "Gb");
            NoteValues.Add(Notes.G, "G");
            NoteValues.Add(Notes.Gs, "G#");
            NoteValues.Add(Notes.Af, "Ab");
            NoteValues.Add(Notes.A, "A");
            NoteValues.Add(Notes.As, "A#");
            NoteValues.Add(Notes.Bf, "Bb");
            NoteValues.Add(Notes.B, "B");

            DictionariesBuilt = true;
        }

        /// <summary>
        /// Produces a manipulated scale
        /// </summary>
        /// <param name="Scale">The scale to use. One can
        /// use the dictionaries from this for quicker and
        /// cleaner reference</param>
        /// <param name="showSharp">Denotes if this scale
        /// should represented with sharps or flats</param>
        /// <returns>The manipulated scale</returns>
        public static Notes[] MakeScale(Notes[] Scale, bool showSharp)
        {
            if (!DictionariesBuilt)
                BuildDictionary();

            Notes[] result = Scale;
            if (!showSharp)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    switch (result[i])
                    {
                        case Notes.Cs:
                            result[i] = Notes.Df;
                            break;
                        case Notes.Ds:
                            result[i] = Notes.Ef;
                            break;
                        case Notes.Fs:
                            result[i] = Notes.Gf;
                            break;
                        case Notes.Gs:
                            result[i] = Notes.Af;
                            break;
                        case Notes.As:
                            result[i] = Notes.Bf;
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }


    }
}