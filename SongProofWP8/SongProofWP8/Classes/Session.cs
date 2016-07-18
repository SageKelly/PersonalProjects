using System;
using System.ComponentModel;

namespace SongProofWP8
{
    public class Session : INotifyPropertyChanged
    {
        public struct SessionData
        {
            /// <summary>
            /// Represents the related index number for the data in question.
            /// For example, the the C Major Scale D is index 2, G is index 5.
            /// </summary>
            public int DataIndex;
            public bool Correct;
            public int GuessTime;

            public SessionData(int note_index, int guess_time, bool correct)
            {
                DataIndex = note_index;
                Correct = correct;
                GuessTime = guess_time;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ScaleResources.Difficulties Diff;

        public Scale ScaleUsed;

        /// <summary>
        /// What uniquely identifies this session; based off of date created
        /// </summary>
        public DateTime ID { get; private set; }

        public string UniqueID { get; private set; }

        /// <summary>
        /// Holds the proofing data/answers for one proofing
        /// </summary>
        public int[] ProofingData { get; private set; }

        /// <summary>
        /// Represents a list of stored input during a proofing
        /// </summary>
        public SessionData[] Data { get; private set; }

        public string[] Piano;

        private int internalIndex;

        public DataHolder.ProofingTypes ProofingType;

        /// <summary>
        /// Creates a  Session
        /// </summary>
        /// <param name="piano">The piano used for the proofing session</param>
        /// <param name="proofingType">The type of the proofing session this will represent</param>
        /// <param name="difficulty">The difficult of the session, refer to the Resources Class for determination.</param>
        /// <param name="scale_used">The scale to be used. Refer to the Resources Class for determination.</param>
        /// <param name="notes">The notes being used within this Session</param>
        public Session(DataHolder.ProofingTypes proofingType, ScaleResources.Difficulties difficulty, Scale scale_used, string[] piano, int[] notes)
        {
            ID = DateTime.Now;
            ProofingType = proofingType;
            ScaleUsed = scale_used;
            Piano = piano;
            Diff = difficulty;
            UniqueID = ID.Date + ", " + ID.TimeOfDay + ": " + "," + proofingType + "," + ScaleUsed.Name + ", " + Diff;
            ProofingData = notes;
            Data = new SessionData[notes.Length];
            internalIndex = 0;
        }

        /// <summary>
        /// Creates a  Session
        /// </summary>
        /// <param name="piano">The piano used for the proofing session</param>
        /// <param name="proofingType">The type of the proofing session this will represent</param>
        /// <param name="difficulty">The difficult of the session, refer to the Resources Class for determination.</param>
        /// <param name="data">The notes being used within this Session</param>
        public Session(DataHolder.ProofingTypes proofingType, ScaleResources.Difficulties difficulty, string[] piano, int[] data)
        {
            ID = DateTime.Now;
            ProofingType = proofingType;
            Piano = piano;
            Diff = difficulty;
            UniqueID = ID.Date + ", " + ID.TimeOfDay + ": " + "," + proofingType + "," + Diff;
            ProofingData = data;
            Data = new SessionData[data.Length];
            internalIndex = 0;
        }


        /// <summary>
        /// Stores Note input for each guessed
        /// </summary>
        /// <param name="index">The index of the note the player is currently on</param>
        /// <param name="correct">Was the guess correct?</param>
        /// <param name="remaining_time">How long did it take for them to guess?</param>
        public void StoreData(int index, bool correct, int remaining_time)
        {
            if (internalIndex < Data.Length)
                Data[internalIndex++] = new SessionData(index, remaining_time, correct);
        }

        private void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
            }
        }

        public void Reset()
        {
            ID = DateTime.Now;
            UniqueID = ID.Date + ", " + ID.TimeOfDay + ": " + ScaleUsed.Name + ", " + Diff;
            Data = new SessionData[ProofingData.Length];
            internalIndex = 0;
        }
    }
}
