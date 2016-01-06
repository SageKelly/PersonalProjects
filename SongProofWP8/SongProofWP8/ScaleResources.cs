using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    /// <summary>
    /// Has static resources for scales and notes
    /// </summary>
    public static class ScaleResources
    {
        public enum ScaleTypes
        {
            Augmented,
            Aeolian,
            Bebop,
            BebopDominant,
            BebopMajor,
            BebopMinor1,
            BebopMinor2,
            Blues,
            DiminishedH,
            DiminishedW,
            DiminishedWholeTone,
            Dorian,
            HarmonicMajor,
            HarmonicMinor,
            HarmonicMinorMode6,
            Hindu,
            Ionian,
            Locrian1,
            Locrian2,
            Lydian,
            LydianAugmented,
            LydianDominant,
            MelodicMinorAscending,
            Mixolydian,
            PentatonicMajor,
            PentatonicMinor,
            Phrygian,
            SpanishJewish,
            WholeTone
        }

        public enum Difficulties
        {
            Easy = 3000,
            Medium = 1000,
            Hard = 500
        }

        public enum ScaleGroups
        {
            Major,
            Dominant,
            Suspended,
            Minor,
            HalfDiminished,
            Diminished
        }
        public static string[] PianoSharp = new string[] { "C", "C♯", "D", "D♯", "E", "F", "F♯", "G", "G♯", "A", "A♯", "B" };
        public static string[] PianoFlat = new string[] { "C", "D♭", "D", "E♭", "E", "F", "G♭", "G", "A♭", "A", "B♭", "B" };
        public static string[] FriendlyPiano = new string[] { "C", "C♯ / D♭", "D", "D♯ / E♭", "E", "F", "F♯ / G♭", "G", "G♯ / A♭", "A", "A♯B♭", "B" };
        //A♯ / B♭

        public static Dictionary<ScaleTypes, string> ScaleNames;
        public static Dictionary<ScaleTypes, string> ScaleLib;

        public static Dictionary<ScaleTypes, string> MajorScales;
        public static Dictionary<ScaleTypes, string> MinorScales;
        public static Dictionary<ScaleTypes, string> DominantScales;
        public static Dictionary<ScaleTypes, string> SuspendedScales;
        public static Dictionary<ScaleTypes, string> HalfDiminishedScales;
        public static Dictionary<ScaleTypes, string> DiminishedScales;

        public static Dictionary<string, ScaleGroups> ScaleDivisionNames;

        public const int LOWEST_SET = 50;
        public const int LOWEST_INC = 10;
        public const int HIGHEST_SET = 300;


        /// <summary>
        /// Holds the list of all versions of a scale's name
        /// </summary>
        public static string[] ConstantScaleNames { get; private set; }
        public static Difficulties[] DifficultyLevels;

        static bool DictionariesBuilt = false;

        static ScaleResources()
        {
            ScaleDivisionNames = new Dictionary<string, ScaleGroups>();
            ScaleDivisionNames.Add("Major", ScaleGroups.Major);
            ScaleDivisionNames.Add("Dominant", ScaleGroups.Dominant);
            ScaleDivisionNames.Add("Suspended", ScaleGroups.Suspended);
            ScaleDivisionNames.Add("Minor", ScaleGroups.Minor);
            ScaleDivisionNames.Add("Half Diminished", ScaleGroups.HalfDiminished);
            ScaleDivisionNames.Add("Diminished", ScaleGroups.Diminished);

            ScaleNames = new Dictionary<ScaleTypes, string>();
            ScaleNames.Add(ScaleTypes.Augmented, "Augmented");
            ScaleNames.Add(ScaleTypes.Aeolian, "Aeolian / Natural Minor");
            ScaleNames.Add(ScaleTypes.Bebop, "Bebop");
            ScaleNames.Add(ScaleTypes.BebopDominant, "Bebop Dominant");
            ScaleNames.Add(ScaleTypes.BebopMajor, "Bebop Major");
            ScaleNames.Add(ScaleTypes.BebopMinor1, "Bebop Minor 1");
            ScaleNames.Add(ScaleTypes.BebopMinor2, "Bebop Minor 2");
            ScaleNames.Add(ScaleTypes.Blues, "Blues");
            ScaleNames.Add(ScaleTypes.DiminishedH, "Diminished (Half Note)");
            ScaleNames.Add(ScaleTypes.DiminishedW, "Diminished (Whole Note)");
            ScaleNames.Add(ScaleTypes.DiminishedWholeTone, "Diminished Whole Tone");
            ScaleNames.Add(ScaleTypes.Dorian, "Dorian / Minor");
            ScaleNames.Add(ScaleTypes.HarmonicMajor, "Harmonic Major");
            ScaleNames.Add(ScaleTypes.HarmonicMinor, "Harmonic Minor");
            ScaleNames.Add(ScaleTypes.HarmonicMinorMode6, "6th Mode of Harmonic Minor");
            ScaleNames.Add(ScaleTypes.Hindu, "Hindu");
            ScaleNames.Add(ScaleTypes.Ionian, "Ionian / Major");
            ScaleNames.Add(ScaleTypes.Locrian1, "Locrian 1 / Half Diminished 1");
            ScaleNames.Add(ScaleTypes.Locrian2, "Locrian 2 / Half Diminished 2");
            ScaleNames.Add(ScaleTypes.Lydian, "Lydian");
            ScaleNames.Add(ScaleTypes.LydianAugmented, "Lydian Augmented");
            ScaleNames.Add(ScaleTypes.LydianDominant, "Lydian Dominiant");
            ScaleNames.Add(ScaleTypes.MelodicMinorAscending, "Melodic Minor (Ascending)");
            ScaleNames.Add(ScaleTypes.Mixolydian, "Mixolydian / Dominant 7th");
            ScaleNames.Add(ScaleTypes.PentatonicMajor, "Pentatonic Major");
            ScaleNames.Add(ScaleTypes.PentatonicMinor, "Pentatonic Minor");
            ScaleNames.Add(ScaleTypes.Phrygian, "Phrygian");
            ScaleNames.Add(ScaleTypes.SpanishJewish, "Spanish / Jewish");
            ScaleNames.Add(ScaleTypes.WholeTone, "Whole Tone");

            ScaleLib = new Dictionary<ScaleTypes, string>();
            ScaleLib.Add(ScaleTypes.Augmented, "313131");
            ScaleLib.Add(ScaleTypes.Aeolian, "2122122");
            ScaleLib.Add(ScaleTypes.BebopDominant, "22122111");
            ScaleLib.Add(ScaleTypes.BebopMajor, "22121121");
            ScaleLib.Add(ScaleTypes.BebopMinor1, "21112212");
            ScaleLib.Add(ScaleTypes.BebopMinor2, "21221121");
            ScaleLib.Add(ScaleTypes.Blues, "321132");
            ScaleLib.Add(ScaleTypes.DiminishedH, "12121212");
            ScaleLib.Add(ScaleTypes.DiminishedW, "21212121");
            ScaleLib.Add(ScaleTypes.DiminishedWholeTone, "1212222");
            ScaleLib.Add(ScaleTypes.Dorian, "2122212");
            ScaleLib.Add(ScaleTypes.HarmonicMajor, "2212131");
            ScaleLib.Add(ScaleTypes.HarmonicMinor, "2122131");
            ScaleLib.Add(ScaleTypes.HarmonicMinorMode6, "3121221");
            ScaleLib.Add(ScaleTypes.Hindu, "2212122");
            ScaleLib.Add(ScaleTypes.Ionian, "2212221");
            ScaleLib.Add(ScaleTypes.Locrian1, "1221222");
            ScaleLib.Add(ScaleTypes.Locrian2, "2121222");
            ScaleLib.Add(ScaleTypes.Lydian, "2221221");
            ScaleLib.Add(ScaleTypes.LydianAugmented, "2222121");
            ScaleLib.Add(ScaleTypes.LydianDominant, "2221212");
            ScaleLib.Add(ScaleTypes.MelodicMinorAscending, "2122221");
            ScaleLib.Add(ScaleTypes.Mixolydian, "2212212");
            ScaleLib.Add(ScaleTypes.PentatonicMajor, "22323");
            ScaleLib.Add(ScaleTypes.PentatonicMinor, "32232");
            ScaleLib.Add(ScaleTypes.Phrygian, "1222122");
            ScaleLib.Add(ScaleTypes.SpanishJewish, "1312122");
            ScaleLib.Add(ScaleTypes.WholeTone, "222222");

            MajorScales = new Dictionary<ScaleTypes, string>();
            MajorScales.Add(ScaleTypes.Ionian, ScaleLib[ScaleTypes.Ionian]);
            MajorScales.Add(ScaleTypes.PentatonicMajor, ScaleLib[ScaleTypes.PentatonicMajor]);
            MajorScales.Add(ScaleTypes.Lydian, ScaleLib[ScaleTypes.Lydian]);
            MajorScales.Add(ScaleTypes.BebopMajor, ScaleLib[ScaleTypes.BebopMajor]);
            MajorScales.Add(ScaleTypes.HarmonicMajor, ScaleLib[ScaleTypes.HarmonicMajor]);
            MajorScales.Add(ScaleTypes.LydianAugmented, ScaleLib[ScaleTypes.LydianAugmented]);
            MajorScales.Add(ScaleTypes.Augmented, ScaleLib[ScaleTypes.Augmented]);
            MajorScales.Add(ScaleTypes.HarmonicMinorMode6, ScaleLib[ScaleTypes.HarmonicMinorMode6]);
            MajorScales.Add(ScaleTypes.DiminishedH, ScaleLib[ScaleTypes.DiminishedH]);
            MajorScales.Add(ScaleTypes.Blues, ScaleLib[ScaleTypes.Blues]);

            DominantScales = new Dictionary<ScaleTypes, string>();
            DominantScales.Add(ScaleTypes.Mixolydian, ScaleLib[ScaleTypes.Mixolydian]);
            DominantScales.Add(ScaleTypes.PentatonicMajor, ScaleLib[ScaleTypes.PentatonicMajor]);
            DominantScales.Add(ScaleTypes.BebopDominant, ScaleLib[ScaleTypes.BebopDominant]);
            DominantScales.Add(ScaleTypes.SpanishJewish, ScaleLib[ScaleTypes.SpanishJewish]);
            DominantScales.Add(ScaleTypes.LydianDominant, ScaleLib[ScaleTypes.LydianDominant]);
            DominantScales.Add(ScaleTypes.Hindu, ScaleLib[ScaleTypes.Hindu]);
            DominantScales.Add(ScaleTypes.WholeTone, ScaleLib[ScaleTypes.WholeTone]);
            DominantScales.Add(ScaleTypes.DiminishedH, ScaleLib[ScaleTypes.DiminishedH]);
            DominantScales.Add(ScaleTypes.DiminishedWholeTone, ScaleLib[ScaleTypes.DiminishedWholeTone]);
            DominantScales.Add(ScaleTypes.Blues, ScaleLib[ScaleTypes.Blues]);

            SuspendedScales = new Dictionary<ScaleTypes, string>();
            SuspendedScales.Add(ScaleTypes.Mixolydian, ScaleLib[ScaleTypes.Mixolydian]);
            SuspendedScales.Add(ScaleTypes.PentatonicMajor, ScaleLib[ScaleTypes.PentatonicMajor]);
            SuspendedScales.Add(ScaleTypes.Bebop, "22122111");

            MinorScales = new Dictionary<ScaleTypes, string>();
            MinorScales.Add(ScaleTypes.Dorian, ScaleLib[ScaleTypes.Dorian]);
            MinorScales.Add(ScaleTypes.PentatonicMinor, ScaleLib[ScaleTypes.PentatonicMinor]);
            MinorScales.Add(ScaleTypes.BebopMinor1, ScaleLib[ScaleTypes.BebopMinor1]);
            MinorScales.Add(ScaleTypes.MelodicMinorAscending, ScaleLib[ScaleTypes.MelodicMinorAscending]);
            MinorScales.Add(ScaleTypes.BebopMinor2, ScaleLib[ScaleTypes.BebopMinor2]);
            MinorScales.Add(ScaleTypes.Blues, ScaleLib[ScaleTypes.Blues]);
            MinorScales.Add(ScaleTypes.HarmonicMinor, ScaleLib[ScaleTypes.HarmonicMinor]);
            MinorScales.Add(ScaleTypes.DiminishedW, ScaleLib[ScaleTypes.DiminishedW]);
            MinorScales.Add(ScaleTypes.Phrygian, ScaleLib[ScaleTypes.Phrygian]);
            MinorScales.Add(ScaleTypes.Aeolian, ScaleLib[ScaleTypes.Aeolian]);

            HalfDiminishedScales = new Dictionary<ScaleTypes, string>();
            HalfDiminishedScales.Add(ScaleTypes.Locrian1, ScaleLib[ScaleTypes.Locrian1]);
            HalfDiminishedScales.Add(ScaleTypes.Locrian2, ScaleLib[ScaleTypes.Locrian2]);
            HalfDiminishedScales.Add(ScaleTypes.Bebop, "12211122");

            DiminishedScales = new Dictionary<ScaleTypes, string>();
            DiminishedScales.Add(ScaleTypes.DiminishedW, ScaleLib[ScaleTypes.DiminishedW]);

            DictionariesBuilt = true;

            DifficultyLevels = new Difficulties[3] { Difficulties.Easy, Difficulties.Medium, Difficulties.Hard };
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
        public static Scale MakeScale(string starting_key, ScaleGroups scale_group, ScaleTypes scale_name, bool showSharp)
        {
            Dictionary<ScaleTypes, string> group;
            switch (scale_group)
            {
                case ScaleGroups.Diminished:
                    group = DiminishedScales;
                    break;
                case ScaleGroups.Dominant:
                    group = DominantScales;
                    break;
                case ScaleGroups.HalfDiminished:
                    group = HalfDiminishedScales;
                    break;
                case ScaleGroups.Minor:
                    group = MinorScales;
                    break;
                case ScaleGroups.Suspended:
                    group = SuspendedScales;
                    break;
                case ScaleGroups.Major:
                default:
                    group = MajorScales;
                    break;
            }
            string str_scale = group[scale_name];
            string[] scale = new string[str_scale.Length + 1];
            string[] piano = showSharp ? PianoSharp : PianoFlat;
            int index = 0;

            //find the location of the starting note
            for (int i = 0; i < piano.Length; i++)
            {
                if (piano[i] == starting_key)
                {
                    index = i;
                    break;
                }
                else if (piano[i] != starting_key && i == piano.Length - 1)//If it still doesn't match...
                {
                    //..the starting key
                    starting_key = piano[i];
                    index = i;
                    break;
                    //This case should never happen, but for the sake of the integrity of the program, it's included.
                }
            }
            //put it in
            scale[0] = piano[index];

            //then find the rest
            for (int i = 0; i < scale.Length; i++)
            {
                index += int.Parse(scale[i]);
                scale[i + 1] = piano[index];
            }
            Scale result = new Scale(ScaleNames[scale_name], scale);
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
            int[] results = new int[noteCount];
            Random rng = new Random();
            for (int i = 0; i < results.Length; i++)
            {
                temp = rng.Next(0, s.Notes.Length);
                if (i != 0)
                {
                    //if this note is the same as the last one...
                    if (results[i - 1] == temp)
                    {
                        //...go up one in the scale, or wrap around
                        temp = (temp + 1) % s.Notes.Length;
                    }
                }
                results[i] = temp;
            }
            return results;
        }

    }
}