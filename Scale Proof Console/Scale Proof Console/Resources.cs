using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale_Proof_Console
{
    /// <summary>
    /// Has static resources for scales and notes
    /// </summary>
    public static class Resources
    {
        /// <summary>
        /// All possible notes within a European music system
        /// </summary>
        public enum Notes
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

        /// <summary>
        /// The per-note guessing times for each difficulty setting
        /// </summary>
        public enum Difficulties
        {
            Easy = 3000,
            Medium = 1000,
            Hard = 500
        }

        #region Scales
        private static Notes[] ScaleC = { Notes.C, Notes.D, Notes.E, Notes.F, Notes.G, Notes.A, Notes.B };
        private static Notes[] ScaleCs = { Notes.Cs, Notes.Ds, Notes.F, Notes.Fs, Notes.Gs, Notes.As, Notes.C };
        private static Notes[] ScaleD = { Notes.D, Notes.E, Notes.Fs, Notes.G, Notes.A, Notes.B, Notes.C };
        private static Notes[] ScaleDs = { Notes.Ds, Notes.F, Notes.G, Notes.Gs, Notes.As, Notes.C, Notes.D };
        private static Notes[] ScaleE = { Notes.E, Notes.Fs, Notes.Gs, Notes.A, Notes.B, Notes.Cs, Notes.Ds };
        private static Notes[] ScaleF = { Notes.F, Notes.G, Notes.A, Notes.Bf, Notes.C, Notes.D, Notes.E };
        private static Notes[] ScaleFs = { Notes.Fs, Notes.Gs, Notes.As, Notes.B, Notes.Cs, Notes.Ds, Notes.F };
        private static Notes[] ScaleG = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.Fs };
        private static Notes[] ScaleGs = { Notes.G, Notes.A, Notes.B, Notes.C, Notes.D, Notes.E, Notes.F };
        private static Notes[] ScaleA = { Notes.A, Notes.B, Notes.Cs, Notes.D, Notes.E, Notes.Fs, Notes.Gs };
        private static Notes[] ScaleAs = { Notes.As, Notes.C, Notes.D, Notes.Ds, Notes.F, Notes.G, Notes.A };
        private static Notes[] ScaleB = { Notes.B, Notes.Cs, Notes.Ds, Notes.E, Notes.Fs, Notes.Gs, Notes.As };
        #endregion

        #region string values for notes
        public const string C = "C";
        public const string CS = "C#";
        private const string DB = "Db";
        public const string D = "D";
        public const string DS = "D#";
        private const string EB = "Eb";
        public const string E = "E";
        public const string F = "F";
        public const string FS = "F#";
        private const string GB = "Gb";
        public const string G = "G";
        public const string GS = "G#";
        private const string AB = "Ab";
        public const string A = "A";
        public const string AS = "A#";
        private const string BB = "Bb";
        public const string B = "B";
        #endregion

        /// <summary>
        /// List of Scales, document by scale name. (e.g. C, C#, etc.)
        /// </summary>
        public static Dictionary<string, Notes[]> Scales { get; private set; }
        /// <summary>
        /// Holds the string values of each of the notes
        /// </summary>
        public static Dictionary<Notes, string> NoteValues { get; private set; }

        private static bool DictionariesBuilt = false;

        /// <summary>
        /// Populates the dictionaries with scales and notes for later reference
        /// </summary>
        private static void BuildDictionary()
        {
            Scales = new Dictionary<string, Notes[]>();

            Scales.Add(C, ScaleC);
            Scales.Add(CS, ScaleCs);
            Scales.Add(D, ScaleD);
            Scales.Add(DS, ScaleDs);
            Scales.Add(E, ScaleE);
            Scales.Add(F, ScaleF);
            Scales.Add(FS, ScaleFs);
            Scales.Add(G, ScaleG);
            Scales.Add(GS, ScaleGs);
            Scales.Add(A, ScaleA);
            Scales.Add(AS, ScaleAs);
            Scales.Add(B, ScaleB);

            NoteValues = new Dictionary<Notes, string>();

            NoteValues.Add(Notes.C, C);
            NoteValues.Add(Notes.Cs, CS);
            NoteValues.Add(Notes.Df, DB);
            NoteValues.Add(Notes.D, D);
            NoteValues.Add(Notes.Ds, DS);
            NoteValues.Add(Notes.Ef, EB);
            NoteValues.Add(Notes.E, E);
            NoteValues.Add(Notes.F, F);
            NoteValues.Add(Notes.Fs, FS);
            NoteValues.Add(Notes.Gf, GB);
            NoteValues.Add(Notes.G, G);
            NoteValues.Add(Notes.Gs, GS);
            NoteValues.Add(Notes.Af, AB);
            NoteValues.Add(Notes.A, A);
            NoteValues.Add(Notes.As, AS);
            NoteValues.Add(Notes.Bf, BB);
            NoteValues.Add(Notes.B, B);

            DictionariesBuilt = true;
        }

        /// <summary>
        /// Produces a manipulated scale
        /// </summary>
        /// <param name="scale_name">The string name of the scale to use. One can
        /// use the strings from the Resources class for reference</param>
        /// <param name="showSharp">Denotes if this scale
        /// should represented with sharps or flats</param>
        /// <returns>The manipulated scale</returns>
        public static Notes[] MakeScale(string scale_name, bool showSharp)
        {
            if (!DictionariesBuilt)
                BuildDictionary();

            Notes[] result = Scales[scale_name].ToArray();

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
            return result;
        }


    }
}