using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    public static class DataHolder
    {
        public enum ProofingTypes
        {
            Dummy,
            BuildTheScale,
            PlacingTheNote,
            HW3,
            GrabBag,
            ScaleWriting
        }

        /// <summary>
        /// Denote the type of the intended proofing Used with the Main Page and SessionSetup
        /// </summary>
        public static ProofingTypes ProofType;
        public static SessionManager SM;
        public static bool ShowSharp;

        private static ProofingTypes _prevProofType = ProofingTypes.Dummy;
        private static string Key;
        private static KVTuple<string, string> Scale;
        private static ScaleResources.Difficulties Diff;
        private static int NoteCount;


        public static void SetupPTNTest(ProofingTypes proofingType, string key, KVTuple<string, string> scale, bool sharp, ScaleResources.Difficulties diff, int note_count)
        {
            ProofType = proofingType;
            _prevProofType = ProofType;
            Key = key;
            Scale = scale;
            ShowSharp = sharp;
            Diff = diff;
            NoteCount = note_count;
            Scale temp = ScaleResources.MakeScale(key, scale, sharp);
            SM = new SessionManager(new Session(ProofType, Diff, temp,
                sharp ? ScaleResources.PianoSharp : ScaleResources.PianoFlat,
                ScaleResources.MakePTNQuiz(temp, note_count)));
            SM.ParsedScale = ScaleResources.ParseScale(Scale);
            ShowSharp = sharp;
        }

        private static void SetupPTNTest()
        {
            if (Scale != null)
            {
                SetupPTNTest(ProofType, Key, Scale, ShowSharp, Diff, NoteCount);
            }
        }

        public static void SetupHW3Test(ProofingTypes proofingType, bool sharp, ScaleResources.Difficulties diff, int note_count)
        {
            ProofType = proofingType;
            _prevProofType = ProofType;
            ShowSharp = sharp;
            Diff = diff;
            NoteCount = note_count;
            SM = new SessionManager(new Session(ProofType, Diff,
                sharp ? ScaleResources.PianoSharp : ScaleResources.PianoFlat,
                ScaleResources.MakeHW3Quiz(note_count)));
        }

        private static void SetupHW3Test()
        {
            if (NoteCount != null)
            {
                SetupHW3Test(ProofType, ShowSharp, Diff, NoteCount);
            }
        }

        public static void ResetTest()
        {
            switch (ProofType)
            {
                case ProofingTypes.PlacingTheNote:
                    SetupPTNTest();
                    break;
                case ProofingTypes.HW3:
                    SetupHW3Test();
                    break;
            }
        }


    }
}
