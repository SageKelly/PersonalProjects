using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    public class NoteAnalytics : INotifyPropertyChanged
    {
        private int _count;
        private double _avg_guessing_time;
        private int _correct_guesses;
        private string _sentinel_value;

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }
        public double AvgGuessingTime
        {
            get
            {
                return _avg_guessing_time;
            }
            set
            {
                if (_avg_guessing_time != value)
                {
                    _avg_guessing_time = value;
                    NotifyPropertyChanged("AvgGuessingTime");
                }
            }
        }
        public int CorrectGuesses
        {
            get
            {
                return _correct_guesses;
            }
            set
            {
                if (_correct_guesses != value)
                {
                    _correct_guesses = value;
                    NotifyPropertyChanged("CorrectGuesses");
                }
            }
        }

        public string SentinelValue
        {
            get
            {
                return _sentinel_value;
            }
            set
            {
                if (_sentinel_value != value)
                {
                    _sentinel_value = value;
                    NotifyPropertyChanged("SentinelValue");
                }
            }
        }

        public string StrCount
        {
            get
            {
                return "Occurrences within session: " + Count;
            }
            private set { }
        }
        public string StrAvgGuessingTime
        {
            get
            {
                return "Average Guessing Time: " + AvgGuessingTime;
            }
            private set { }
        }
        public string StrCorrectGuesses
        {
            get
            {
                return "Correct Guesses: " + CorrectGuesses;
            }
            private set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NoteAnalytics()
        {
            Count = 0;
            AvgGuessingTime = 0;
            CorrectGuesses = 0;
            SentinelValue = "C";
        }

        public NoteAnalytics(string note)
            : this()
        {
            SentinelValue = note;
        }

        private void NotifyPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
            }
        }
    }
}
