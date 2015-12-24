using System;

namespace SongProofForms
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


        public Resources.Difficulties Diff;

        public Scale ScaleUsed;

        public string UniqueID { get; private set; }

        /// <summary>
        /// Holds the notes for one game
        /// </summary>
        public int[] Notes { get; private set; }

        public NoteData[] Data { get; private set; }

        private int internalIndex;



        /// <summary>
        /// Creates a  Session
        /// </summary>
        /// <param name="difficulty">The difficult of the session, refer to the Resources Class for determination.</param>
        /// <param name="scale_used">The scale to be used. Refer to the Resources Class for determination.</param>
        /// <param name="notes">The notes being used within this Session</param>
        public Session(Resources.Difficulties difficulty, Scale scale_used, int[] notes)
        {
            ID = DateTime.Now;
            UniqueID = ID.Date + ", " + ID.TimeOfDay + ": " + ScaleUsed.Name + ", " + Diff;
            Diff = difficulty;
            ScaleUsed = scale_used;
            Notes = notes;
            Data = new NoteData[notes.Length];
            internalIndex = 0;
        }

        /// <summary>
        /// What uniquely identifies this session; based off of date created
        /// </summary>
        public DateTime ID { get; private set; }

        /// <summary>
        /// Stores Note input for each guessed
        /// </summary>
        /// <param name="index">The index of the note the player is currently on</param>
        /// <param name="correct">Was the guess correct?</param>
        /// <param name="remaining_time">How long did it take for them to guess?</param>
        public void StoreNoteInput(int index, bool correct, int remaining_time)
        {
            if (internalIndex < Data.Length)
                Data[internalIndex++] = new NoteData(index, remaining_time, correct);
        }
    }
}
