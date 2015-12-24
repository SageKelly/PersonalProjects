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
    public partial class MainForm : Form
    {
        SessionSetupForm SSF;
        /// <summary>
        /// Holds the list of UniqueIDs on a file.
        /// </summary>
        const string SESSION_FILENAME = "SPF_Sessions.bin";
        /// <summary>
        /// Made to store the UniqueIDs of all the sessions
        /// </summary>
        public List<string> Sessions { get; private set; }
        public MainForm()
        {
            InitializeComponent();
            Resources.BuildDictionaries();
            Sessions = new List<string>();
            Size = MaximumSize = MinimumSize = FormResource.ScreenSize;
        }

        private void B_Session_Start_Click(object sender, EventArgs e)
        {
            SSF = new SessionSetupForm(this, this.Location);
            SSF.Show();
            this.Hide();
        }

        private void B_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void B_Session_Data_Click(object sender, EventArgs e)
        {

        }
    }
}
