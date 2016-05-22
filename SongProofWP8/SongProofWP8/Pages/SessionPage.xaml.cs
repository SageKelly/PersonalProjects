using SongProofWP8.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SongProofWP8.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SessionPage : Page, INotifyPropertyChanged
    {
        private int curIndex;
        private int note_index;
        private string current_note;
        private string note_count;
        private int note_number;
        private string scale_name;
        private static string TIMER_FLAVOR = "Time left: ";
        private bool SessionStarted = false;

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

        public int NoteNumber
        {
            get
            {
                return note_number;
            }
            set
            {
                if (note_number != value)
                {
                    note_number = value;
                    NotifyPropertyChanged("NoteNumber");
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

        ProgressTrackerControl ptc;
        SessionButtonsControl sbc;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public SessionPage()
        {
            this.InitializeComponent();
            curSession = DataHolder.SM.CurrentSession;
            ScaleName = curSession.ScaleUsed.Name;
            DataContext = this;
            curIndex = 0;
            NoteCount = (curIndex + 1) + " / " + curSession.Notes.Length;

            TitleBarControl tbc = new TitleBarControl(ScaleName,32);
            ptc = new ProgressTrackerControl("Note Amount", "TickDownTimer_Tick", this, typeof(SessionPage));
            PianoKeyControl pkc = new PianoKeyControl(curSession.Piano, curSession.ScaleUsed.Notes, "NoteClick", this, typeof(SessionPage));
            sbc = new SessionButtonsControl("B_Start_Click", "B_Quit_Click", "B_ViewResults_Click", this, typeof(SessionPage));

            LayoutRoot.Children.Add(tbc);
            LayoutRoot.Children.Add(ptc);
            LayoutRoot.Children.Add(pkc);
            LayoutRoot.Children.Add(sbc);

            Grid.SetRow(tbc, 0);
            Grid.SetRow(ptc, 1);
            Grid.SetRow(pkc, 2);
            Grid.SetRow(sbc, 3);

            ptc.MaxValue = curSession.Notes.Length;
            ptc.PBValue = 0;

            sbc.EnableViewResults(false);

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void NoteClick(object sender)
        {
            Button b = sender as Button;
            if (b != null && !ptc.CountingDown)
            {
                string note = curSession.Piano[0];
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
                ptc.ShowResultPic(correct);
                //NoteCheckSymbol.Data = correct ? (Geometry)Resources["Checkmark"] : (Geometry)Resources["X"];
                //PathInOut.Begin();
            }
        }

        //TODO: Research TimeSpan.Parse()

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Handler for each time the Timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickDownTimer_Tick(object sender)
        {
            ptc.RemainingTime -= ptc.CountingDown ? 1000 : 100;
            if (ptc.RemainingTime == 0)
            {
                if (ptc.CountingDown)
                {
                    ptc.CountingDown = false;
                    ptc.SetTimerInterval(TimeSpan.Parse("00:00:0.100"));
                }
                if (curIndex > 0)
                {
                    //you ran out of time on the last note
                    RecordNoteInput(NoteIndex, false);
                }
                NextNote();
            }
        }

        private void RecordNoteInput(int index, bool correct)
        {
            curSession.StoreNoteInput(index, correct, curSession.Diff.GetHashCode() - ptc.RemainingTime);
            NoteNumber = curSession.Notes[curIndex - 1] + 1;
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
                NoteNumber = (curSession.Notes[curIndex++] + 1);
                ptc.TestingValue = NoteNumber.ToString();
                ptc.RemainingTime = curSession.Diff.GetHashCode();
                ptc.PBValue++;
                NoteCount = curIndex + " / " + curSession.Notes.Length;
            }
            else
            {
                ptc.StopTimer();
                sbc.EnableViewResults(true);
                ptc.SetTimerText(true);
            }
        }


        #region Extra Button Handlers
        private void B_ViewResults_Click(object sender)
        {
            DataHolder.SM.CurrentSession = curSession;
            Frame.Navigate(typeof(SessionResultsPage));
        }

        private void B_Start_Click(object sender)
        {
            if (!SessionStarted)
            {
                if (DataHolder.SM.CurrentSession.Diff != ScaleResources.Difficulties.Zen)
                    ptc.StartTimer();
                else
                {
                    ptc.CountingDown = false;
                    NextNote();
                }
                SessionStarted = true;
                sbc.EnableStart(false);
            }
        }

        private void B_Quit_Click(object sender)
        {
            if (ptc.TimerEnabled())
                ptc.StopTimer();
            Frame.Navigate(typeof(MainPage));
        }


        #endregion
    }
}
