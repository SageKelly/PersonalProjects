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
        [Serializable]
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

        public List<NoteData> SessionData { get; private set; }

        /// <summary>
        /// Creates a  Session
        /// </summary>
        /// <param name="difficulty">The difficult of the session, refer to the Resources Class for determination.</param>
        /// <param name="scale_used">The scale to be used. Refer to the Resources Class for determination.</param>
        /// <param name="notes">The notes being used within this Session</param>
        public Session(Resources.Difficulties difficulty, Scale scale_used, int[] notes)
        {
            ID = DateTime.Now;
            Difficulty = difficulty;
            SessionScale = scale_used;
            SessionNotes = notes;
            SessionData = new List<NoteData>();
        }

        /// <summary>
        /// What uniquely identifies this session; based off of date created
        /// </summary>
        public DateTime ID { get; private set; }


        /// <summary>
        /// Stores Note input for each guessed
        /// </summary>
        /// <param name="index">The index of the note the player is currently on</param>
        public void StoreNoteInput(int index, int guess_time, bool correct)
        {
            SessionData.Add(new NoteData(index, guess_time, correct));
        }

    }
}
