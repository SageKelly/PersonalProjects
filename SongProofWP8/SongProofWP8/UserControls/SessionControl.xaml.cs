using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SongProofWP8.UserControls
{
    public sealed partial class SessionControl : UserControl, INotifyPropertyChanged
    {
        private int curIndex;
        private int note_index;
        private string current_note;
        private string note_count;
        private string timer_text;
        private string _testingLabel;
        private string _testingValue;
        private string scale_name;
        private static string TIMER_FLAVOR = "Time left: ";
        private bool countingDown;
        private bool SessionStarted = false;
        private int remainingTime;
        private DispatcherTimer TickDownTimer;
        private bool _navigationSetup = false;

        #region Properties
        public Session curSession { get; private set; }

        public string ScaleName
        {
            get
            {
                return "Scale Name: " + scale_name;
            }
            set
            {
                if (scale_name != value)
                {
                    scale_name = value;
                    NotifyPropertyChanged("ScaleName");
                }
            }
        }

        public string TimerText
        {
            get
            {
                return TIMER_FLAVOR + timer_text;
            }
            set
            {
                if (timer_text != value)
                {
                    timer_text = value;
                    NotifyPropertyChanged("TimerText");
                }
            }
        }

        public string TestingLabel
        {
            get
            {
                return _testingLabel;
            }
            set
            {
                if (_testingLabel != value)
                {
                    _testingLabel = value;
                    NotifyPropertyChanged("TestingLabel");
                }
            }
        }

        public string TestingValue
        {
            get
            {
                return _testingValue;
            }
            set
            {
                if (_testingValue != value)
                {
                    _testingValue = value;
                    NotifyPropertyChanged("TestingValue");
                }
            }
        }

        public string NoteCount
        {
            get
            {
                return note_count;
            }
            set
            {
                if (note_count != value)
                {
                    note_count = value;
                    NotifyPropertyChanged("NoteCount");
                }
            }
        }

        public Frame navFrame { get; set; }
        public Type ResultNavigationPageType { get; set; }
        public Type QuitNavigationPageType { get; set; }


        /// <summary>
        /// The note index number in relation to the scale
        /// </summary>
        public int NoteIndex
        {
            get
            { return note_index; }
            private set
            {
                if (note_index != value)
                {
                    note_index = value;
                    NotifyPropertyChanged("NoteIndex");
                }
            }
        }
        /// <summary>
        /// Represents the previous note being tested
        /// </summary>
        public string CurrentNote
        {
            get
            {
                return current_note;
            }
            private set
            {
                if (current_note != value)
                {
                    current_note = value;
                    NotifyPropertyChanged("CurrentNote");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public SessionControl()
        {
            this.InitializeComponent();
            DataContext = this;
            curSession = DataHolder.SM.CurrentSession;
            LBScale.ItemsSource = curSession.ScaleUsed.Notes;
            IC_Buttons.ItemsSource = curSession.Piano;
            //L_Scale.Text = curSession.ScaleUsed.Name;
            ScaleName = curSession.ScaleUsed.Name;
            curIndex = 0;
            countingDown = true;
            remainingTime = 3000;
            TickDownTimer = new DispatcherTimer();
            TickDownTimer.Tick += TickDownTimer_Tick;
            TickDownTimer.Interval = TimeSpan.Parse("00:00:1");
            SetTimerText();
            B_ViewResults.IsEnabled = false;
            NoteCount = (curIndex + 1) + " / " + curSession.Notes.Length;
        }

        private void NoteClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b != null && !countingDown)
            {
                string note = "C";
                foreach (string s in curSession.Piano)
                {
                    if (b.Content.ToString() == s)
                    {
                        note = s;
                        break;
                    }
                }
                int noteIndex = curSession.Notes[curIndex - 1];
                bool correct = curSession.ScaleUsed.Notes[noteIndex] == note;

                RecordNoteInput(noteIndex, correct);
                NextNote();
                if (correct)
                {
                    XBorder.Visibility = Visibility.Collapsed;
                    CheckBorder.Visibility = Visibility.Visible;
                    FadeInCheck.Begin();
                }
                else
                {
                    CheckBorder.Visibility = Visibility.Collapsed;
                    XBorder.Visibility = Visibility.Visible;
                    FadeInX.Begin();
                }
                //NoteCheckSymbol.Data = correct ? (Geometry)Resources["Checkmark"] : (Geometry)Resources["X"];
                //PathInOut.Begin();
            }
        }

        /// <summary>
        /// Handler for each time the Timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickDownTimer_Tick(object sender, object e)
        {
            remainingTime -= countingDown ? 1000 : 100;
            if (remainingTime == 0)
            {
                if (countingDown)
                {
                    countingDown = false;
                    TickDownTimer.Interval = TimeSpan.Parse("00:00:0.100");
                }
                if (curIndex > 0)
                {
                    //you ran out of time on the last note
                    RecordNoteInput(NoteIndex, false);
                }
                NextNote();
            }
            SetTimerText();
        }

        private void RecordNoteInput(int index, bool correct)
        {
            curSession.StoreNoteInput(index, correct, curSession.Diff.GetHashCode() - remainingTime);
            TestingValue = (curSession.Notes[curIndex - 1] + 1).ToString();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NextNote()
        {
            if (curIndex < curSession.Notes.Length)
            {
                TestingValue = (curSession.Notes[curIndex++] + 1).ToString();
                remainingTime = curSession.Diff.GetHashCode();
                NoteCount = curIndex + " / " + curSession.Notes.Length;
            }
            else
            {
                TickDownTimer.Stop();
                B_ViewResults.IsEnabled = true;
                TimerText = "0.000";
            }
        }

        private void SetTimerText()
        {
            TimerText = (remainingTime / 1000) + "." + (remainingTime % 1000);
        }

        /// <summary>
        /// Sets up navigation for the UserControl's buttons
        /// </summary>
        /// <param name="navFrame">The Frame used for navigation</param>
        /// <param name="ResultsPageType">The Page to which to navigate after proofing is complete</param>
        /// <param name="QuitPageType">The Page to which to navigate when the user chooses to quit</param>
        public void SetupNavigation(Frame navFrame, Type ResultsPageType, Type QuitPageType)
        {
            this.navFrame = navFrame;
            ResultNavigationPageType = ResultsPageType;
            QuitNavigationPageType = QuitPageType;
            _navigationSetup = true;
        }

        #region Extra Button Handlers


        private void B_ViewResults_Click(object sender, RoutedEventArgs e)
        {
            DataHolder.SM.CurrentSession = curSession;
            navFrame.Navigate(ResultNavigationPageType);
        }

        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            if (!SessionStarted)
            {
                if (DataHolder.SM.CurrentSession.Diff != ScaleResources.Difficulties.Zen)
                    TickDownTimer.Start();
                else
                {
                    countingDown = false;
                    NextNote();
                }
                SessionStarted = true;
                B_Start.IsEnabled = false;
            }
        }

        private void B_Quit_Click(object sender, RoutedEventArgs e)
        {
            if (TickDownTimer.IsEnabled)
                TickDownTimer.Stop();
            navFrame.Navigate(QuitNavigationPageType);
        }

        private void ToggleScaleView(object sender, RoutedEventArgs e)
        {
            if ((bool)B_Cheat.IsChecked)
            {
                B_Cheat.Content = "Be Honest?";
                FadeIn.Begin();
            }
            else
            {
                B_Cheat.Content = "Cheat?";
                FadeOut.Begin();
            }
        }
        #endregion
    }
}
