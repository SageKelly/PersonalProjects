using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    public static class DataHolder
    {
        public static SessionManager SM;
        public static bool ShowSharp;
        static string Key;
        static KVTuple<string, string> Scale;
        static ScaleResources.Difficulties Diff;
        static int NoteCount;

        public static void SetupTest(string key, KVTuple<string, string> scale, bool sharp, ScaleResources.Difficulties diff, int note_count)
        {
            Key = key;
            Scale = scale;
            ShowSharp = sharp;
            Diff = diff;
            NoteCount = note_count;
            Scale temp = ScaleResources.MakeScale(key, scale, sharp);
            SM = new SessionManager(new Session(Diff, temp,
                sharp ? ScaleResources.PianoSharp : ScaleResources.PianoFlat,
                ScaleResources.MakeQuiz(temp, note_count)));
            SM.ParsedScale = ScaleResources.ParseScale(Scale);
            ShowSharp = sharp;
        }

        public static void SetupTest()
        {
            if (Scale != null)
            if(Scale!=null)
            {
                SetupTest(Key, Scale, ShowSharp, Diff, NoteCount);
            }
        }
    }
}
