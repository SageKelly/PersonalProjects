using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale_Proof_Console
{
    /// <summary>
    /// Holds a series of notes and the user's associated answers
    /// </summary>
    [Serializable]
    public class Session
    {
        /// <summary>
        /// Holds note data of each user's answer
        /// </summary>
        public struct NoteData
        {
            public int Note;
            public bool Correct;
            public int GuessTime;

            public NoteData(int note, int guess_time, bool correct)
            {
                Note = note;
                Correct = correct;
                GuessTime = guess_time;
            }
        }


        public Resources.Difficulties Difficulty;

        public Scale SessionScale;

        /// <summary>
        /// Holds the notes for one game
        /// </summary>
        public int[] SessionNotes;


        /// <summary>
        /// Creates a  Session
        /// </summary>
        /// <param name="difficulty">The difficult of the session, refer to the Resources Class for determination.</param>
        /// <param name="scale_used">The scale to be used. Refer to the Resources Class for determination.</param>
        /// <param name="session_length">How many notes will be attempted within this sesson</param>
        public Session(Resources.Difficulties difficulty, Scale scale_used, int session_length)
        {
            Difficulty = difficulty;
            SessionScale = scale_used;
            SessionNotes = new int[session_length];
            ID = DateTime.Now;
        }

        /// <summary>
        /// What uniquely identifies this session; based off of date created
        /// </summary>
        public DateTime ID { get; private set; }


        public void StoreNoteInput()
        {

        }

    }
}
