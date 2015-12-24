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
    public partial class SessionViewForm : Form
    {
        MainForm MF;
        /// <summary>
        /// Allows the viewing of a Session
        /// </summary>
        /// <param name="s"></param>
        public SessionViewForm(Session s, MainForm mf)
        {
            InitializeComponent();
            MF = mf;
        }

        private void B_CloseForm_Click(object sender, EventArgs e)
        {
            MF.Show();
            this.Hide();
        }

        private void B_OpenSession_Click(object sender, EventArgs e)
        {

        }
    }
}
