using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SongProofForms
{
    public partial class SessionForm : Form, INotifyPropertyChanged
    {
        MainForm MF;
        public Session curSession { get; private set; }
        int curIndex;
        int note_index;
        Resources.Notes last_note;
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
                    NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Represents the previous note being tested
        /// </summary>
        public Resources.Notes LastNote
        {
            get
            {
                return last_note;
            }
            private set
            {
                if (last_note != value)
                {
                    last_note = value;
                    NotifyPropertyChanged();
                }
            }
        }
        bool countingDown;
        bool SessionStarted = false;
        int remainingTime;
        public event PropertyChangedEventHandler PropertyChanged;

        public SessionForm(MainForm mf, Session s)
        {
            InitializeComponent();
            MF = mf;
            curSession = s;
            L_Scale.Text = curSession.ScaleUsed.Name;
            curIndex = 0;
            countingDown = true;
            remainingTime = 3000;
            TickDownTimer.Interval = 1000;
            SetTimerText();
            B_ViewData.Enabled = false;
            L_NoteCount.Text = (curIndex + 1) + " / " + curSession.Notes.Length;
            L_No.DataBindings.Add("Text", this, "NoteIndex");
            L_Note.DataBindings.Add("Text", this, "LastNote");
        }

        /// <summary>
        /// Handler for each time the Timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickDownTimer_Tick(object sender, EventArgs e)
        {
            remainingTime -= countingDown ? 1000 : 100;
            if (remainingTime == 0)
            {
                if (countingDown)
                {
                    countingDown = false;
                    TickDownTimer.Interval = 100;
                }
                if (curIndex > 0)
                {
                    //you ran out of time on the last note
                    RecordNoteInput(curIndex - 1, false);


                }
                NextNote();
            }
            SetTimerText();
        }

        private void RecordNoteInput(int index, bool correct)
        {
            curSession.StoreNoteInput(index, correct, remainingTime);
            NoteIndex = curSession.Notes[curIndex - 1] + 1;
            LastNote = curSession.ScaleUsed.Notes[NoteIndex - 1];
            L_Note.ForeColor = correct ? Color.Green : Color.Red;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Input Checks
        /// <summary>
        /// Handler for the C button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_C_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.C);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the C#/Db button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_CsDf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Cs ||
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Df));
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the D button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_D_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.D);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the D#/Eb button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_DsEf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Ds ||
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Ef));
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the E button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_E_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.E);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the F button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_F_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.F);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the F#/Gb button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_FsGf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Fs ||
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Gf));
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the G button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_G_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.G);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the G#/Ab button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_GsAf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Gs ||
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Af));
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the A button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_A_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.A);
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the A#/Bb button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_AsBf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.As ||
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.Bf));
                NextNote();
            }
        }

        /// <summary>
        /// Handler for the B button being clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_B_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.ScaleUsed.Notes[noteIndex] == Resources.Notes.B);
                NextNote();
            }
        }
        #endregion

        private void NextNote()
        {
            if (curIndex < curSession.Notes.Length)
            {
                L_NoteNumber.Text = (curSession.Notes[curIndex++] + 1).ToString();
                remainingTime = curSession.Diff.GetHashCode();
                L_NoteCount.Text = curIndex + " / " + curSession.Notes.Length;
            }
            else
            {
                TickDownTimer.Stop();
                B_ViewData.Enabled = true;
                L_Timer.Text = "0.000";
            }
        }

        private void SetTimerText()
        {
            L_Timer.Text = (remainingTime / 1000) + "." + (remainingTime % 1000);
        }

        #region Extra Button Handlers
        private void B_Start_Click(object sender, EventArgs e)
        {
            if (!SessionStarted)
            {
                TickDownTimer.Start();
                SessionStarted = true;
                B_Start.Enabled = false;
            }
        }

        private void B_Quit_Click(object sender, EventArgs e)
        {
            ShutDown();
        }

        private void FormClosed_ShutDown(object sender, FormClosedEventArgs e)
        {
            ShutDown();
        }

        private void ShutDown()
        {
            TickDownTimer.Stop();
            MF.Sessions.RemoveAt(MF.Sessions.Count - 1);
            this.Hide();
            MF.Location = this.Location;
            MF.Show();
        }

        private void B_ViewData_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
