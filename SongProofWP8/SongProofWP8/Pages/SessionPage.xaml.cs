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
        private int note_number;
        private string scale_name;
        private bool SessionStarted = false;
        private string[] piano;

        private const string IH = "H";
        private const string IW = "W";
        private const string I3 = "-3";

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

        #region HW3 Variables

        private int incrementor;
        #endregion

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
        #endregion

        ProgressTrackerControl ptc;
        SessionButtonsControl sbc;
        HW3ProgressTrackerControl hptc;

        public event PropertyChangedEventHandler PropertyChanged;

        public SessionPage()
        {
            this.InitializeComponent();
            curSession = DataHolder.SM.CurrentSession;
            if (DataHolder.ProofType == DataHolder.ProofingTypes.PlacingTheNote)
            {
                ScaleName = curSession.ScaleUsed.Name;
            }
            piano = curSession.Piano;
            DataContext = this;
            curIndex = 0;

            incrementor = 0;

            SetTitleText();
            SetupProgressTracker();
            SetupButtons();


            sbc = new SessionButtonsControl("B_Start_Click", "B_Quit_Click", "B_ViewResults_Click", this, typeof(SessionPage));

            LayoutRoot.Children.Add(sbc);
            Grid.SetRow(sbc, 3);
            sbc.EnableViewResults(false);

            if (DataHolder.ProofType == DataHolder.ProofingTypes.PlacingTheNote)
            {
                ptc.MaxValue = curSession.ProofingData.Length;
                ptc.PBValue = 0;
            }

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            Frame.Navigate(typeof(MainPage));
        }
        #region Global Methods
        private void SetTitleText()
        {
            TitleBarControl tbc = new TitleBarControl("");
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.PlacingTheNote:
                    tbc = new TitleBarControl(ScaleName, 32);
                    break;
                case DataHolder.ProofingTypes.HW3:
                    tbc = new TitleBarControl("HW3");
                    break;
            }
            LayoutRoot.Children.Add(tbc);
            Grid.SetRow(tbc, 0);
        }

        private void SetupProgressTracker()
        {
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.PlacingTheNote:
                    ptc = new ProgressTrackerControl("Note Amount", "TickDownTimer_Tick", this, typeof(SessionPage));
                    LayoutRoot.Children.Add(ptc);
                    Grid.SetRow(ptc, 1);
                    break;
                case DataHolder.ProofingTypes.HW3:
                    hptc = new HW3ProgressTrackerControl(curSession.Data.Length, "HW3TickDown", this, typeof(SessionPage));
                    LayoutRoot.Children.Add(hptc);
                    Grid.SetRow(hptc, 1);
                    break;
            }
        }

        private void SetupButtons()
        {
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.PlacingTheNote:
                    PianoKeyControl pkc = new PianoKeyControl(curSession.Piano, curSession.ScaleUsed.Notes, "NoteClick", this, typeof(SessionPage));
                    LayoutRoot.Children.Add(pkc);
                    Grid.SetRow(pkc, 2);
                    break;
                case DataHolder.ProofingTypes.HW3:
                    HW3ButtonsControl hbc = new HW3ButtonsControl(IH, IW, I3, "IntervalClick", this, typeof(SessionPage));
                    LayoutRoot.Children.Add(hbc);
                    Grid.SetRow(hbc, 2);
                    break;
            }
        }

        private void RecordData(int index, bool correct)
        {
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.PlacingTheNote:
                    curSession.StoreData(index, correct, curSession.Diff.GetHashCode() - ptc.RemainingTime);
                    break;
                case DataHolder.ProofingTypes.HW3:
                    curSession.StoreData(index, correct, curSession.Diff.GetHashCode() - hptc.RemainingTime);
                    break;
            }
        }
        #endregion

        #region Particular Methods
        #region Placing The Note
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
                    RecordData(NoteIndex, false);
                }
                NextNote();
            }
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
                int noteIndex = curSession.ProofingData[curIndex - 1];
                bool correct = curSession.ScaleUsed.Notes[noteIndex] == note;

                RecordData(noteIndex, correct);
                NextNote();
                ptc.ShowResultPic(correct);
                //NoteCheckSymbol.Data = correct ? (Geometry)Resources["Checkmark"] : (Geometry)Resources["X"];
                //PathInOut.Begin();
            }
        }
        private void NextNote()
        {
            if (curIndex < curSession.ProofingData.Length)
            {
                NoteNumber = (curSession.ProofingData[curIndex++] + 1);
                ptc.TestingValue = NoteNumber.ToString();
                ptc.RemainingTime = curSession.Diff.GetHashCode();
                ptc.PBValue++;
            }
            else
            {
                ptc.StopTimer();
                sbc.EnableViewResults(true);
                ptc.SetTimerText(true);
            }
        }
        #endregion

        #region HW3 Reaction
        private void IntervalClick(object sender)
        {
            Button sentinel = sender as Button;
            int interval = 1;
            if (sentinel != null && !hptc.CountingDown)
            {
                string pressedButton = sentinel.Content.ToString();
                switch (pressedButton)
                {
                    case IH:
                        interval = 1;
                        break;
                    case IW:
                        interval = 2;
                        break;
                    case I3:
                        interval = 3;
                        break;
                }
                int curData = curSession.ProofingData[curIndex - (hptc.PBValue > 3 ? 1 : 3)];
                bool correct = curData == interval;
                RecordData(curData, correct);
                GetNextHW3Note();
            }
        }
        private void HPTCTimer_Tick(object sender)
        {
            hptc.RemainingTime -= hptc.CountingDown ? 1000 : 100;
            if (hptc.RemainingTime == 0)
            {
                if (hptc.CountingDown)
                {
                    hptc.CountingDown = false;
                    hptc.SetTimerInterval(TimeSpan.Parse("00:00:0.100"));
                    GetNextHW3Interval();
                    GetNextHW3Interval();
                    GetNextHW3Interval();
                    hptc.ToggleViewIntervals();
                }
                if (curIndex > 0)
                {
                    //you ran out of time on the last note
                    RecordData(NoteIndex, false);
                }
                GetNextHW3Note();
            }
        }

        private void GetNextHW3Interval(bool Slide = false)
        {
            incrementor = (incrementor + curSession.ProofingData[curIndex++]) % piano.Length;
            if (!Slide)
            {
                hptc.SetNextNote(piano[incrementor]);
            }
            else
            {
                hptc.NextInterval(piano[incrementor]);
            }
        }

        private void GetNextHW3Note()
        {
            if (curIndex < curSession.ProofingData.Length)
            {
                if (hptc.PBValue > 3)
                {
                    GetNextHW3Interval();
                }
                hptc.RemainingTime = curSession.Diff.GetHashCode();
                hptc.IncrementProgress();
                GetNextHW3Interval(true);
            }
            else
            {
                hptc.StopTimer();
                sbc.EnableViewResults(true);
                hptc.SetTimerText(true);
            }
        }
        #endregion
        #endregion


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



        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
                switch (DataHolder.ProofType)
                {
                    case DataHolder.ProofingTypes.PlacingTheNote:
                        if (DataHolder.SM.CurrentSession.Diff != ScaleResources.Difficulties.Zen)
                        {
                            ptc.StartTimer();
                        }
                        else
                        {
                            ptc.CountingDown = false;
                            NextNote();
                        }
                        break;
                    case DataHolder.ProofingTypes.HW3:
                        if (DataHolder.SM.CurrentSession.Diff != ScaleResources.Difficulties.Zen)
                        {
                            hptc.StartTimer();
                        }
                        else
                        {
                            hptc.CountingDown = false;
                            GetNextHW3Interval();
                            GetNextHW3Interval();
                            GetNextHW3Interval();
                            hptc.ToggleViewIntervals();
                            hptc.RemainingTime = curSession.Diff.GetHashCode();
                        }
                        break;
                }


                SessionStarted = true;
                sbc.EnableStart(false);
            }
        }

        private void B_Quit_Click(object sender)
        {
            switch (DataHolder.ProofType)
            {
                case DataHolder.ProofingTypes.PlacingTheNote:
                    if (ptc.TimerEnabled())
                        ptc.StopTimer();
                    break;
                case DataHolder.ProofingTypes.HW3:
                    if (hptc.TimerEnabled())
                        hptc.StopTimer();
                    break;
            }
            Frame.Navigate(typeof(MainPage));
        }


        #endregion
    }
}
