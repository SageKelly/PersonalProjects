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
    public partial class SessionSetupForm : Form
    {
        MainForm MF;
        SessionForm SF;
        public int NoteCount { get; private set; }
        public SessionSetupForm(MainForm mf, Point location)
        {
            InitializeComponent();

            NUD_NoteCount.Increment = Resources.LOWEST_INC;
            NUD_NoteCount.Minimum = NUD_NoteCount.Value = Resources.LOWEST_SET;
            NUD_NoteCount.Maximum = Resources.HIGHEST_SET;

            CB_Scales.DataSource = Resources.ConstantScaleNames;
            CB_Diff.DataSource = Resources.DifficultyLevels;
            MF = mf;
            this.Location = location;
            NoteCount = Resources.LOWEST_SET;
        }

        private void CloseProgram(object sender, FormClosedEventArgs e)
        {
            MF.Location = this.Location;
            MF.Show();
            this.Hide();
        }

        private void B_Begin_Click(object sender, EventArgs e)
        {
            Scale temp = Resources.MakeScale((string)CB_Scales.SelectedItem,
                C_IsSharp.Checked);
            Resources.Difficulties Diff = (Resources.Difficulties)CB_Diff.SelectedItem;
            Session newSession = new Session(Diff, temp, Resources.MakeQuiz(temp,
                (int)NUD_NoteCount.Value));
            MF.Sessions.Add(newSession.UniqueID);
            this.Hide();
            SF = new SessionForm(MF, newSession);
            SF.Location = this.Location;
            SF.Show();
        }
    }
}
