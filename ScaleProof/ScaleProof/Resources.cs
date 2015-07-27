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
        public static enum Notes
        {
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
        public Notes Note;

        public enum Difficulties
        {
            Easy = 3000,
            Medium = 1000,
            Hard = 500
        }

        public static Notes[] ScaleC = { Notes.C, Notes.D, Notes.E, Notes.F, Notes.G, Notes.A, Notes.B };
        public static Notes[] ScaleCs = { Notes.Cs, Notes.Ds, Notes.F, Notes.Fs, Notes.Gs, Notes.As, Notes.C };
        public static Notes[] ScaleD = { Notes.D, Notes.E, Notes.Fs, Notes.G, Notes.A, Notes.B, Notes.C };
        public static Notes[] ScaleDs = { Notes.Ds, Notes.F, Notes.G, Notes.Gs, Notes.As, Notes.C, Notes.D };
        public static Notes[] ScaleE = { Notes.E, Notes.Fs, Notes.Gs, Notes.A, Notes.B, Notes.Cs, Notes.Ds };
        public static Notes[] ScaleF = { Notes.F, Notes.G, Notes.A, Notes.Bf, Notes.C, Notes.D, Notes.E };
        public static Notes[] ScaleFs = { Notes.Fs, Notes.Gs, Notes.As, Notes.B, Notes.Cs, Notes.Ds, Notes.F };
        public static Notes[] ScaleG = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.Fs };
        public static Notes[] ScaleGs = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.F };
        public static Notes[] ScaleA = { Notes.A, Notes.B, Notes.Cs, Notes.D, Notes.E, Notes.Fs, Notes.Gs };
        public static Notes[] ScaleAs = { Notes.As, Notes.C, Notes.D, Notes.Ds, Notes.F, Notes.G, Notes.A };
        public static Notes[] ScaleB = { Notes.B, Notes.Cs, Notes.Ds, Notes.E, Notes.Fs, Notes.Gs, Notes.As };

        /// <summary>
        /// List of Scales, document by scale name. (e.g. C, C#, etc.)
        /// </summary>
        public static Dictionary<string, Notes[]> Scales { get; private set; }
        /// <summary>
        /// Holds the string values of each of the notes
        /// </summary>
        public static Dictionary<Notes, string> NoteValues { get; private set; }

        static bool DictionariesBuilt = false;

        /// <summary>
        /// Populates the dictionaries with scales and notes for later reference
        /// </summary>
        private static void BuildDictionary()
        {
            Scales = new Dictionary<string, Notes[]>();

            Scales.Add("C", ScaleC);
            Scales.Add("C#", ScaleCs);
            Scales.Add("D", ScaleD);
            Scales.Add("D#", ScaleDs);
            Scales.Add("E", ScaleE);
            Scales.Add("F", ScaleF);
            Scales.Add("F#", ScaleFs);
            Scales.Add("G", ScaleG);
            Scales.Add("G#", ScaleGs);
            Scales.Add("A", ScaleA);
            Scales.Add("A#", ScaleAs);
            Scales.Add("B", ScaleB);

            NoteValues = new Dictionary<Notes, string>();

            NoteValues.Add(Notes.C, "C");
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