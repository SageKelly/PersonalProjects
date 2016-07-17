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
            BebopHalfDiminished,
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
            Hard = 500,
            Zen = 0
        }

        public static string[] PianoSharp = new string[] { "C", "C♯", "D", "D♯", "E", "F", "F♯", "G", "G♯", "A", "A♯", "B" };
        public static string[] PianoFlat = new string[] { "C", "D♭", "D", "E♭", "E", "F", "G♭", "G", "A♭", "A", "B♭", "B" };
        //A♯ / B♭

        public static Dictionary<ScaleTypes, string> ScaleNames;
        public static Dictionary<ScaleTypes, string> ScaleLib;

        public static List<KVTuple<string, string>> MajorScales;
        public static List<KVTuple<string, string>> MinorScales;
        public static List<KVTuple<string, string>> DominantScales;
        public static List<KVTuple<string, string>> SuspendedScales;
        public static List<KVTuple<string, string>> HalfDiminishedScales;
        public static List<KVTuple<string, string>> DiminishedScales;

        public static Dictionary<string, List<KVTuple<string, string>>> ScaleDivisionNames;

        /// <summary>
        /// Represents the lowest amount of notes that can exist within a test
        /// </summary>
        public const int LOWEST_SET = 50;
        /// <summary>
        /// Represents the incrementation amount that should be used to
        /// increase/decrease the note amount
        /// </summary>
        public const int LOWEST_INC = 10;
        /// <summary>
        /// Represents the highest amuont of notes that can exist within a test
        /// </summary>
        public const int HIGHEST_SET = 300;


        /// <summary>
        /// Holds the list of all versions of a scale's name
        /// </summary>
        public static string[] ConstantScaleNames { get; private set; }
        public static Difficulties[] DifficultyLevels;

        static ScaleResources()
        {
            ScaleNames = new Dictionary<ScaleTypes, string>();
            ScaleNames.Add(ScaleTypes.Augmented, "Augmented");
            ScaleNames.Add(ScaleTypes.Aeolian, "Aeolian / Natural Minor");
            ScaleNames.Add(ScaleTypes.Bebop, "Bebop");
            ScaleNames.Add(ScaleTypes.BebopHalfDiminished, "Bebop Half-Diminished");
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
            ScaleNames.Add(ScaleTypes.Locrian1, "Locrian 1 / Half-Diminished 1");
            ScaleNames.Add(ScaleTypes.Locrian2, "Locrian 2 / Half-Diminished 2");
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
            ScaleLib.Add(ScaleTypes.Bebop, "22122111");
            ScaleLib.Add(ScaleTypes.BebopHalfDiminished, "12211122");
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

            MajorScales = new List<KVTuple<string, string>>();
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Ionian], ScaleLib[ScaleTypes.Ionian]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.PentatonicMajor], ScaleLib[ScaleTypes.PentatonicMajor]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Lydian], ScaleLib[ScaleTypes.Lydian]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.BebopMajor], ScaleLib[ScaleTypes.BebopMajor]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.HarmonicMajor], ScaleLib[ScaleTypes.HarmonicMajor]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.LydianAugmented], ScaleLib[ScaleTypes.LydianAugmented]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Augmented], ScaleLib[ScaleTypes.Augmented]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.HarmonicMinorMode6], ScaleLib[ScaleTypes.HarmonicMinorMode6]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.DiminishedH], ScaleLib[ScaleTypes.DiminishedH]));
            MajorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Blues], ScaleLib[ScaleTypes.Blues]));

            DominantScales = new List<KVTuple<string, string>>();
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Mixolydian], ScaleLib[ScaleTypes.Mixolydian]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.PentatonicMajor], ScaleLib[ScaleTypes.PentatonicMajor]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Bebop], ScaleLib[ScaleTypes.Bebop]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.SpanishJewish], ScaleLib[ScaleTypes.SpanishJewish]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.LydianDominant], ScaleLib[ScaleTypes.LydianDominant]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Hindu], ScaleLib[ScaleTypes.Hindu]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.WholeTone], ScaleLib[ScaleTypes.WholeTone]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.DiminishedH], ScaleLib[ScaleTypes.DiminishedH]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.DiminishedWholeTone], ScaleLib[ScaleTypes.DiminishedWholeTone]));
            DominantScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Blues], ScaleLib[ScaleTypes.Blues]));

            SuspendedScales = new List<KVTuple<string, string>>();
            SuspendedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Mixolydian], ScaleLib[ScaleTypes.Mixolydian]));
            SuspendedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.PentatonicMajor], ScaleLib[ScaleTypes.PentatonicMajor]));
            SuspendedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Bebop], ScaleLib[ScaleTypes.Bebop]));

            MinorScales = new List<KVTuple<string, string>>();
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Dorian], ScaleLib[ScaleTypes.Dorian]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.PentatonicMinor], ScaleLib[ScaleTypes.PentatonicMinor]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.BebopMinor1], ScaleLib[ScaleTypes.BebopMinor1]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.MelodicMinorAscending], ScaleLib[ScaleTypes.MelodicMinorAscending]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.BebopMinor2], ScaleLib[ScaleTypes.BebopMinor2]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Blues], ScaleLib[ScaleTypes.Blues]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.HarmonicMinor], ScaleLib[ScaleTypes.HarmonicMinor]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.DiminishedW], ScaleLib[ScaleTypes.DiminishedW]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Phrygian], ScaleLib[ScaleTypes.Phrygian]));
            MinorScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Aeolian], ScaleLib[ScaleTypes.Aeolian]));

            HalfDiminishedScales = new List<KVTuple<string, string>>();
            HalfDiminishedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Locrian1], ScaleLib[ScaleTypes.Locrian1]));
            HalfDiminishedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.Locrian2], ScaleLib[ScaleTypes.Locrian2]));
            HalfDiminishedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.BebopHalfDiminished], ScaleLib[ScaleTypes.BebopHalfDiminished]));

            DiminishedScales = new List<KVTuple<string, string>>();
            DiminishedScales.Add(new KVTuple<string, string>(ScaleNames[ScaleTypes.DiminishedW], ScaleLib[ScaleTypes.DiminishedW]));

            ScaleDivisionNames = new Dictionary<string, List<KVTuple<string, string>>>();
            ScaleDivisionNames.Add("Major", MajorScales);
            ScaleDivisionNames.Add("Dominant", DominantScales);
            ScaleDivisionNames.Add("Suspended", SuspendedScales);
            ScaleDivisionNames.Add("Minor", MinorScales);
            ScaleDivisionNames.Add("Half Diminished", HalfDiminishedScales);
            ScaleDivisionNames.Add("Diminished", DiminishedScales);

            DifficultyLevels = new Difficulties[4] { Difficulties.Zen, Difficulties.Easy, Difficulties.Medium, Difficulties.Hard };
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
        public static Scale MakeScale(string starting_key, KVTuple<string, string> scale, bool showSharp)
        {
            string[] scale_result = new string[scale.Value.Length];
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
                    //..the starting key will be the last note on the piano
                    starting_key = piano[i];
                    index = i;
                    break;
                    //This case should never happen, but for the sake of the integrity of the program, it's included.
                }
            }
            //put it in
            scale_result[0] = piano[index];

            //then find the rest
            for (int i = 0; i < scale.Value.Length - 1; i++)
            {
                index = (index + int.Parse(scale.Value[i].ToString())) % piano.Length;
                scale_result[i + 1] = piano[index];
            }
            Scale result = new Scale(starting_key + " " + scale.Key, scale_result);
            return result;
        }

        /// <summary>
        /// Creates the notes for the current session
        /// </summary>
        /// <param name="s">The scale being used in the session</param>
        /// <param name="noteCount">The amount of notes quizzed</param>
        /// <returns>An array of ints directly related to the scale used</returns>
        public static int[] MakePTNQuiz(Scale s, int noteCount)
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

        /// <summary>
        /// Creates the interval array for the current session
        /// </summary>
        /// <param name="noteCount">The amount of notes quizzed</param>
        /// <returns>An array of ints directly related to the intervals used</returns>
        public static int[] MakeHW3Quiz(int noteCount)
        {
            Random r = new Random();
            int[] result = new int[noteCount];
            for (int i = 0; i < noteCount; i++)
            {
                result[i] = r.Next(1, 4);
            }

            return result;
        }


        public static bool IsANumber(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (input[i] == j.ToString()[0])
                        break;
                    else if (input[i] != j.ToString()[0] && j == 9)
                        return false;
                }
            }
            return true;
        }


        public static string[] ParseScale(KVTuple<string, string> scale)
        {
            string[] result = new string[scale.Value.Length];
            for (int i = 0; i < result.Length; i++)
            {
                switch (scale.Value[i])
                {
                    case '1':
                        result[i] = "H";
                        break;
                    case '2':
                        result[i] = "W";
                        break;
                    case '3':
                    default:
                        result[i] = "-3";
                        break;
                }
            }
            return result;
        }

    }
}