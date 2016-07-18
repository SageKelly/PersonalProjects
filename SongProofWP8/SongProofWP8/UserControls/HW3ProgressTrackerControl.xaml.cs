using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SongProofWP8.UserControls
{
    public sealed partial class HW3ProgressTrackerControl : UserControl, INotifyPropertyChanged
    {
        private string[] notes;
        public string[] Notes
        {
            get
            {
                return notes;
            }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    NotifyPropertyChanged("Notes");
                }
            }
        }

        private string note1Var;
        private string note2Var;
        private string note3Var;


        public string Note1Var
        {
            get { return note1Var; }
            set
            {
                if (note1Var != value)
                {
                    note1Var = value;
                    NotifyPropertyChanged("Note1Var");
                }
            }
        }
        public string Note2Var
        {
            get { return note2Var; }
            set
            {
                if (note2Var != value)
                {
                    note2Var = value;
                    NotifyPropertyChanged("Note2Var");
                }
            }
        }
        public string Note3Var
        {
            get { return note3Var; }
            set
            {
                if (note3Var != value)
                {
                    note3Var = value;
                    NotifyPropertyChanged("Note3Var");
                }
            }
        }

        private int _maxValue;
        private int _pBValue;

        private string _timerText;
        public string TimerText
        {
            get
            {
                return _timerText;
            }
            set
            {
                if (_timerText != value)
                {
                    _timerText = value;
                    NotifyPropertyChanged("TimerText");
                }
            }
        }

        private int noteSlideCount = 0;

        public int MaxValue
        {
            get
            { return _maxValue; }
            private set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    NotifyPropertyChanged("MaxValue");
                }
            }
        }
        public int PBValue
        {
            get
            { return _pBValue; }
            private set
            {
                if (_pBValue != value)
                {
                    _pBValue = value;
                    NotifyPropertyChanged("PBValue");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private object _methodTarget;
        private MethodInfo _timerMethod;

        private double _translationX;
        public double TranslationX
        {
            get { return _translationX; }
            set
            {
                if (_translationX != value)
                {
                    _translationX = value;
                    NotifyPropertyChanged("TranslationX");
                }
            }
        }

        public int RemainingTime { get; set; }
        public bool CountingDown { get; set; }
        private DispatcherTimer TickDownTimer;
        string nextNote = string.Empty;
        public HW3ProgressTrackerControl(int maxValue, string timerMethodName, object methodTarget, Type targetType)
        {
            this.InitializeComponent();
            //Notes = new string[3] { "Z", "Z", "Z" };
            DataContext = this;
            PBValue = 0;
            MaxValue = maxValue;

            TranslationX = 0;

            _methodTarget = methodTarget;
            _timerMethod = targetType.GetTypeInfo().GetDeclaredMethod(timerMethodName);
            SetTimerText();
            CountingDown = true;
            RemainingTime = 3000;
            TickDownTimer = new DispatcherTimer();
            TickDownTimer.Tick += TickDownTimer_Tick;
            TickDownTimer.Interval = TimeSpan.Parse("00:00:1");

            SlideAnimation.Completed += SlideAnimation_Completed;
        }

        void SlideAnimation_Completed(object sender, object e)
        {
            ShuffleNotes();

        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void TickDownTimer_Tick(object sender, object e)
        {
            _timerMethod.Invoke(_methodTarget, new object[1] { sender });
            SetTimerText();
        }

        public void StartTimer()
        {
            TickDownTimer.Start();
        }

        public void StopTimer()
        {
            TickDownTimer.Stop();
        }

        public bool TimerEnabled()
        {
            return TickDownTimer.IsEnabled;
        }

        public void SetTimerInterval(TimeSpan interval)
        {
            TickDownTimer.Interval = interval;
        }

        public void SetTimerText(bool defaultText = false)
        {
            if (defaultText)
            {
                TimerText = "0.000";
            }
            else
            {
                TimerText = (RemainingTime / 1000) + "." + (RemainingTime % 1000);
            }
        }

        public void NextInterval(string note)
        {
            //Use animation to Slide everything
            SlideAnimation.Begin();
            nextNote = note;
        }

        private void ShuffleNotes()
        {
            SetNextNote(nextNote);
            //Then reset the row settings for each one
            switch (noteSlideCount)
            {
                case 0://third time
                    // 1 →1 2 →2 3 →3
                    Grid.SetColumn(Note3Label, 5);
                    Grid.SetColumn(Note3, 5);
                    Grid.SetColumn(Arrow3, 6);
                    Grid.SetColumn(Note2Label, 3);
                    Grid.SetColumn(Note2, 3);
                    Grid.SetColumn(Arrow2, 4);
                    Grid.SetColumn(Note1Label, 1);
                    Grid.SetColumn(Note1, 1);
                    Grid.SetColumn(Arrow1, 2);
                    break;
                case 1://first time
                    // 2 →2 3 →3 1 →1
                    Grid.SetColumn(Note1Label, 5);
                    Grid.SetColumn(Note1, 5);
                    Grid.SetColumn(Arrow1, 6);
                    Grid.SetColumn(Note3Label, 3);
                    Grid.SetColumn(Note3, 3);
                    Grid.SetColumn(Arrow3, 4);
                    Grid.SetColumn(Note2Label, 1);
                    Grid.SetColumn(Note2, 1);
                    Grid.SetColumn(Arrow2, 2);
                    break;
                case 2://second time
                    // 3 →3 1 →1 2 →2
                    Grid.SetColumn(Note2Label, 5);
                    Grid.SetColumn(Note2, 5);
                    Grid.SetColumn(Arrow2, 6);
                    Grid.SetColumn(Note1Label, 3);
                    Grid.SetColumn(Note1, 3);
                    Grid.SetColumn(Arrow1, 4);
                    Grid.SetColumn(Note3Label, 1);
                    Grid.SetColumn(Note3, 1);
                    Grid.SetColumn(Arrow3, 2);
                    break;
            }
        }

        public void SetNextNote(string note)
        {
            //This increments over time in order to stay in sync with the shuffling they do.
            switch (noteSlideCount)
            {
                case 0:
                    Note1Var = note;
                    break;
                case 1:
                    Note2Var = note;
                    break;
                case 2:
                    Note3Var = note;
                    break;
            }
            noteSlideCount = (noteSlideCount + 1) % 3;
        }

        public void ToggleViewIntervals()
        {
            switch (RevealRect.Visibility)
            {
                case Windows.UI.Xaml.Visibility.Collapsed:
                    RevealRect.Visibility = Visibility.Visible;
                    break;
                case Windows.UI.Xaml.Visibility.Visible:
                    RevealRect.Visibility = Visibility.Collapsed;
                    break;
            }
        }


        public void IncrementProgress()
        {
            PBValue++;
        }
    }
}
