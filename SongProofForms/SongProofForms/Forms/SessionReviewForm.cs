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
    public partial class SessionReviewForm : Form
    {
        public SessionForm SF { get; private set; }
        private MainForm MF;
        public SessionReviewForm(MainForm mf, SessionForm sf)
        {
            InitializeComponent();
            SF = sf;
            MF = mf;
        }

        private void ScaleTestForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
