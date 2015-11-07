using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongProofForms
{
    public partial class SessionForm : Form
    {
        MainForm MF;
        public Session curSession { get; private set; }
        int curIndex;
        bool countingDown;
        bool SessionStarted = false;
        int remainingTime;
        Resources.Notes[] Notes;
        public SessionForm(MainForm mf, Session s)
        {
            InitializeComponent();
            MF = mf;
            curSession = s;
            L_Scale.Text = curSession.Scale.Name;
            curIndex = 0;
            countingDown = true;
            remainingTime = 3000;
            TickDownTimer.Interval = 1000;
            SetTimerText();
            B_ViewData.Enabled = false;
            L_NoteCount.Text = (curIndex + 1) + " / " + curSession.Notes.Length;
            Notes=new Resources.Notes[s.Notes.Length];
            LB_Notes.DataSource = Notes;
            //LB_Notes.ValueMember = "";
        }

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
            curSession.StoreNoteInput(index, correct);
            Notes[curIndex - 1] = curSession.Scale.Notes[index];
            LB_Notes.Refresh();
            /*
            Label NoteData = new Label();
            int note = curSession.Notes[curIndex - (curIndex == 0 ? 0 : 1)];
            NoteData.Text = curSession.Scale.Notes[note].ToString();
            NoteData.ForeColor = correct ? Color.Green : Color.Red;
            LV_CompletedNotes.Controls.Add(NoteData);
            */
        }

        #region Input Checks
        private void B_C_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.C);
                NextNote();
            }
        }

        private void B_CsDf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.Scale.Notes[noteIndex] == Resources.Notes.Cs ||
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.Df));
                NextNote();
            }
        }

        private void B_D_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.D);
                NextNote();
            }
        }

        private void B_DsEf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.Scale.Notes[noteIndex] == Resources.Notes.Ds ||
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.Ef));
                NextNote();
            }
        }

        private void B_E_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.E);
                NextNote();
            }
        }

        private void B_F_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.F);
                NextNote();
            }
        }

        private void B_FsGf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.Scale.Notes[noteIndex] == Resources.Notes.Fs ||
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.Gf));
                NextNote();
            }
        }

        private void B_G_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.G);
                NextNote();
            }
        }

        private void B_GsAf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.Scale.Notes[noteIndex] == Resources.Notes.Gs ||
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.Af));
                NextNote();
            }
        }

        private void B_A_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.A);
                NextNote();
            }
        }

        private void B_AsBf_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    (curSession.Scale.Notes[noteIndex] == Resources.Notes.As ||
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.Bf));
                NextNote();
            }
        }

        private void B_B_Click(object sender, EventArgs e)
        {
            if (!countingDown)
            {
                int noteIndex = curSession.Notes[curIndex - 1];
                RecordNoteInput(curIndex - 1,
                    curSession.Scale.Notes[noteIndex] == Resources.Notes.B);
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
