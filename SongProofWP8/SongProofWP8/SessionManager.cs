using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SongProofWP8
{
    public class SessionManager
    {
        public struct NoteData
        {
            public int noteIndex;
            public bool isCorrect;
            public int timeTaken;
            public NoteData(int note_index, bool is_correct, int time_taken)
            {
                noteIndex = note_index;
                isCorrect = is_correct;
                timeTaken = time_taken;
            }
        }
        public List<NoteData> Notes;

        public Session CurrentSession;

        public DispatcherTimer Timer;

        public string[] ParsedScale;

        public SessionManager()
        {
            Notes = new List<NoteData>();
            Timer = new DispatcherTimer();
        }

        public SessionManager(Session sesh):this()
        {
            CurrentSession = sesh;
        }

        public void ResetSession()
        {
            CurrentSession.Reset();
        }
    }
}
