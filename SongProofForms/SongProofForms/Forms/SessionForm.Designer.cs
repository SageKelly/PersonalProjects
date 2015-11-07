namespace SongProofForms
{
    partial class SessionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SC_Left = new System.Windows.Forms.SplitContainer();
            this.SC_NoteTime = new System.Windows.Forms.SplitContainer();
            this.L_NoteCount = new System.Windows.Forms.Label();
            this.L_NoteNumber = new System.Windows.Forms.Label();
            this.L_TimeLeft = new System.Windows.Forms.Label();
            this.L_Timer = new System.Windows.Forms.Label();
            this.B_Quit = new System.Windows.Forms.Button();
            this.B_ViewData = new System.Windows.Forms.Button();
            this.B_Start = new System.Windows.Forms.Button();
            this.L_Scale = new System.Windows.Forms.Label();
            this.B_B = new System.Windows.Forms.Button();
            this.B_AsBb = new System.Windows.Forms.Button();
            this.B_A = new System.Windows.Forms.Button();
            this.B_GsAb = new System.Windows.Forms.Button();
            this.B_G = new System.Windows.Forms.Button();
            this.B_FsGb = new System.Windows.Forms.Button();
            this.B_F = new System.Windows.Forms.Button();
            this.B_E = new System.Windows.Forms.Button();
            this.B_DsEb = new System.Windows.Forms.Button();
            this.B_D = new System.Windows.Forms.Button();
            this.B_CsDb = new System.Windows.Forms.Button();
            this.B_C = new System.Windows.Forms.Button();
            this.TickDownTimer = new System.Windows.Forms.Timer(this.components);
            this.SC_Core = new System.Windows.Forms.SplitContainer();
            this.LB_Notes = new System.Windows.Forms.ListBox();
            this.difficultiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SC_Left)).BeginInit();
            this.SC_Left.Panel1.SuspendLayout();
            this.SC_Left.Panel2.SuspendLayout();
            this.SC_Left.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC_NoteTime)).BeginInit();
            this.SC_NoteTime.Panel1.SuspendLayout();
            this.SC_NoteTime.Panel2.SuspendLayout();
            this.SC_NoteTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC_Core)).BeginInit();
            this.SC_Core.Panel1.SuspendLayout();
            this.SC_Core.Panel2.SuspendLayout();
            this.SC_Core.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.difficultiesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SC_Left
            // 
            this.SC_Left.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SC_Left.IsSplitterFixed = true;
            this.SC_Left.Location = new System.Drawing.Point(0, 0);
            this.SC_Left.Name = "SC_Left";
            this.SC_Left.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SC_Left.Panel1
            // 
            this.SC_Left.Panel1.Controls.Add(this.SC_NoteTime);
            // 
            // SC_Left.Panel2
            // 
            this.SC_Left.Panel2.Controls.Add(this.B_Quit);
            this.SC_Left.Panel2.Controls.Add(this.B_ViewData);
            this.SC_Left.Panel2.Controls.Add(this.B_Start);
            this.SC_Left.Panel2.Controls.Add(this.L_Scale);
            this.SC_Left.Panel2.Controls.Add(this.B_B);
            this.SC_Left.Panel2.Controls.Add(this.B_AsBb);
            this.SC_Left.Panel2.Controls.Add(this.B_A);
            this.SC_Left.Panel2.Controls.Add(this.B_GsAb);
            this.SC_Left.Panel2.Controls.Add(this.B_G);
            this.SC_Left.Panel2.Controls.Add(this.B_FsGb);
            this.SC_Left.Panel2.Controls.Add(this.B_F);
            this.SC_Left.Panel2.Controls.Add(this.B_E);
            this.SC_Left.Panel2.Controls.Add(this.B_DsEb);
            this.SC_Left.Panel2.Controls.Add(this.B_D);
            this.SC_Left.Panel2.Controls.Add(this.B_CsDb);
            this.SC_Left.Panel2.Controls.Add(this.B_C);
            this.SC_Left.Size = new System.Drawing.Size(484, 367);
            this.SC_Left.SplitterDistance = 183;
            this.SC_Left.TabIndex = 1;
            // 
            // SC_NoteTime
            // 
            this.SC_NoteTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SC_NoteTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC_NoteTime.IsSplitterFixed = true;
            this.SC_NoteTime.Location = new System.Drawing.Point(0, 0);
            this.SC_NoteTime.Name = "SC_NoteTime";
            // 
            // SC_NoteTime.Panel1
            // 
            this.SC_NoteTime.Panel1.Controls.Add(this.L_NoteCount);
            this.SC_NoteTime.Panel1.Controls.Add(this.L_NoteNumber);
            // 
            // SC_NoteTime.Panel2
            // 
            this.SC_NoteTime.Panel2.Controls.Add(this.L_TimeLeft);
            this.SC_NoteTime.Panel2.Controls.Add(this.L_Timer);
            this.SC_NoteTime.Size = new System.Drawing.Size(484, 183);
            this.SC_NoteTime.SplitterDistance = 116;
            this.SC_NoteTime.TabIndex = 0;
            // 
            // L_NoteCount
            // 
            this.L_NoteCount.AutoSize = true;
            this.L_NoteCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_NoteCount.Location = new System.Drawing.Point(11, 11);
            this.L_NoteCount.Name = "L_NoteCount";
            this.L_NoteCount.Size = new System.Drawing.Size(55, 24);
            this.L_NoteCount.TabIndex = 1;
            this.L_NoteCount.Text = "1/100";
            // 
            // L_NoteNumber
            // 
            this.L_NoteNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_NoteNumber.AutoSize = true;
            this.L_NoteNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_NoteNumber.Location = new System.Drawing.Point(29, 34);
            this.L_NoteNumber.Name = "L_NoteNumber";
            this.L_NoteNumber.Size = new System.Drawing.Size(0, 108);
            this.L_NoteNumber.TabIndex = 0;
            this.L_NoteNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_TimeLeft
            // 
            this.L_TimeLeft.AutoSize = true;
            this.L_TimeLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_TimeLeft.Location = new System.Drawing.Point(3, 7);
            this.L_TimeLeft.Name = "L_TimeLeft";
            this.L_TimeLeft.Size = new System.Drawing.Size(114, 29);
            this.L_TimeLeft.TabIndex = 1;
            this.L_TimeLeft.Text = "Time Left";
            this.L_TimeLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // L_Timer
            // 
            this.L_Timer.AutoSize = true;
            this.L_Timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Timer.Location = new System.Drawing.Point(19, 48);
            this.L_Timer.Margin = new System.Windows.Forms.Padding(0);
            this.L_Timer.Name = "L_Timer";
            this.L_Timer.Size = new System.Drawing.Size(0, 91);
            this.L_Timer.TabIndex = 0;
            this.L_Timer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B_Quit
            // 
            this.B_Quit.AutoSize = true;
            this.B_Quit.BackColor = System.Drawing.Color.Firebrick;
            this.B_Quit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Quit.Location = new System.Drawing.Point(3, 139);
            this.B_Quit.Name = "B_Quit";
            this.B_Quit.Size = new System.Drawing.Size(82, 34);
            this.B_Quit.TabIndex = 12;
            this.B_Quit.Text = "Quit";
            this.B_Quit.UseVisualStyleBackColor = false;
            this.B_Quit.Click += new System.EventHandler(this.B_Quit_Click);
            // 
            // B_ViewData
            // 
            this.B_ViewData.AutoSize = true;
            this.B_ViewData.BackColor = System.Drawing.Color.DodgerBlue;
            this.B_ViewData.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_ViewData.Location = new System.Drawing.Point(373, 139);
            this.B_ViewData.Name = "B_ViewData";
            this.B_ViewData.Size = new System.Drawing.Size(104, 34);
            this.B_ViewData.TabIndex = 12;
            this.B_ViewData.Text = "View Data";
            this.B_ViewData.UseVisualStyleBackColor = false;
            this.B_ViewData.Click += new System.EventHandler(this.B_ViewData_Click);
            // 
            // B_Start
            // 
            this.B_Start.AutoSize = true;
            this.B_Start.BackColor = System.Drawing.Color.Chartreuse;
            this.B_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Start.Location = new System.Drawing.Point(200, 139);
            this.B_Start.Name = "B_Start";
            this.B_Start.Size = new System.Drawing.Size(82, 34);
            this.B_Start.TabIndex = 12;
            this.B_Start.Text = "Start";
            this.B_Start.UseVisualStyleBackColor = false;
            this.B_Start.Click += new System.EventHandler(this.B_Start_Click);
            // 
            // L_Scale
            // 
            this.L_Scale.AutoSize = true;
            this.L_Scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Scale.Location = new System.Drawing.Point(6, 0);
            this.L_Scale.Name = "L_Scale";
            this.L_Scale.Size = new System.Drawing.Size(113, 24);
            this.L_Scale.TabIndex = 1;
            this.L_Scale.Text = "Scale Name";
            this.L_Scale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B_B
            // 
            this.B_B.AutoSize = true;
            this.B_B.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_B.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_B.Location = new System.Drawing.Point(163, 75);
            this.B_B.Name = "B_B";
            this.B_B.Size = new System.Drawing.Size(32, 34);
            this.B_B.TabIndex = 11;
            this.B_B.Text = "B";
            this.B_B.UseVisualStyleBackColor = true;
            this.B_B.Click += new System.EventHandler(this.B_B_Click);
            // 
            // B_AsBb
            // 
            this.B_AsBb.AutoSize = true;
            this.B_AsBb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_AsBb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_AsBb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_AsBb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_AsBb.Location = new System.Drawing.Point(326, 75);
            this.B_AsBb.Name = "B_AsBb";
            this.B_AsBb.Size = new System.Drawing.Size(78, 34);
            this.B_AsBb.TabIndex = 10;
            this.B_AsBb.Text = "A♯ / B♭";
            this.B_AsBb.UseVisualStyleBackColor = false;
            this.B_AsBb.Click += new System.EventHandler(this.B_AsBf_Click);
            // 
            // B_A
            // 
            this.B_A.AutoSize = true;
            this.B_A.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_A.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_A.Location = new System.Drawing.Point(124, 75);
            this.B_A.Name = "B_A";
            this.B_A.Size = new System.Drawing.Size(33, 34);
            this.B_A.TabIndex = 9;
            this.B_A.Text = "A";
            this.B_A.UseVisualStyleBackColor = true;
            this.B_A.Click += new System.EventHandler(this.B_A_Click);
            // 
            // B_GsAb
            // 
            this.B_GsAb.AutoSize = true;
            this.B_GsAb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_GsAb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_GsAb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_GsAb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_GsAb.Location = new System.Drawing.Point(287, 115);
            this.B_GsAb.Name = "B_GsAb";
            this.B_GsAb.Size = new System.Drawing.Size(80, 34);
            this.B_GsAb.TabIndex = 8;
            this.B_GsAb.Text = "G♯ / A♭";
            this.B_GsAb.UseVisualStyleBackColor = false;
            this.B_GsAb.Click += new System.EventHandler(this.B_GsAf_Click);
            // 
            // B_G
            // 
            this.B_G.AutoSize = true;
            this.B_G.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_G.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_G.Location = new System.Drawing.Point(85, 75);
            this.B_G.Name = "B_G";
            this.B_G.Size = new System.Drawing.Size(34, 34);
            this.B_G.TabIndex = 7;
            this.B_G.Text = "G";
            this.B_G.UseVisualStyleBackColor = true;
            this.B_G.Click += new System.EventHandler(this.B_G_Click);
            // 
            // B_FsGb
            // 
            this.B_FsGb.AutoSize = true;
            this.B_FsGb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_FsGb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_FsGb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_FsGb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_FsGb.Location = new System.Drawing.Point(241, 75);
            this.B_FsGb.Name = "B_FsGb";
            this.B_FsGb.Size = new System.Drawing.Size(79, 34);
            this.B_FsGb.TabIndex = 6;
            this.B_FsGb.Text = "F♯ / G♭";
            this.B_FsGb.UseVisualStyleBackColor = false;
            this.B_FsGb.Click += new System.EventHandler(this.B_FsGf_Click);
            // 
            // B_F
            // 
            this.B_F.AutoSize = true;
            this.B_F.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_F.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_F.Location = new System.Drawing.Point(202, 35);
            this.B_F.Name = "B_F";
            this.B_F.Size = new System.Drawing.Size(32, 34);
            this.B_F.TabIndex = 5;
            this.B_F.Text = "F";
            this.B_F.UseVisualStyleBackColor = true;
            this.B_F.Click += new System.EventHandler(this.B_F_Click);
            // 
            // B_E
            // 
            this.B_E.AutoSize = true;
            this.B_E.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_E.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_E.Location = new System.Drawing.Point(163, 35);
            this.B_E.Name = "B_E";
            this.B_E.Size = new System.Drawing.Size(33, 34);
            this.B_E.TabIndex = 4;
            this.B_E.Text = "E";
            this.B_E.UseVisualStyleBackColor = true;
            this.B_E.Click += new System.EventHandler(this.B_E_Click);
            // 
            // B_DsEb
            // 
            this.B_DsEb.AutoSize = true;
            this.B_DsEb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_DsEb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_DsEb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_DsEb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_DsEb.Location = new System.Drawing.Point(325, 35);
            this.B_DsEb.Name = "B_DsEb";
            this.B_DsEb.Size = new System.Drawing.Size(79, 34);
            this.B_DsEb.TabIndex = 3;
            this.B_DsEb.Text = "D♯ / E♭";
            this.B_DsEb.UseVisualStyleBackColor = false;
            this.B_DsEb.Click += new System.EventHandler(this.B_DsEf_Click);
            // 
            // B_D
            // 
            this.B_D.AutoSize = true;
            this.B_D.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_D.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_D.Location = new System.Drawing.Point(124, 35);
            this.B_D.Name = "B_D";
            this.B_D.Size = new System.Drawing.Size(33, 34);
            this.B_D.TabIndex = 2;
            this.B_D.Text = "D";
            this.B_D.UseVisualStyleBackColor = true;
            this.B_D.Click += new System.EventHandler(this.B_D_Click);
            // 
            // B_CsDb
            // 
            this.B_CsDb.AutoSize = true;
            this.B_CsDb.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_CsDb.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.B_CsDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_CsDb.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_CsDb.Location = new System.Drawing.Point(240, 35);
            this.B_CsDb.Name = "B_CsDb";
            this.B_CsDb.Size = new System.Drawing.Size(79, 34);
            this.B_CsDb.TabIndex = 1;
            this.B_CsDb.Text = "C♯ / D♭";
            this.B_CsDb.UseVisualStyleBackColor = false;
            this.B_CsDb.Click += new System.EventHandler(this.B_CsDf_Click);
            // 
            // B_C
            // 
            this.B_C.AutoSize = true;
            this.B_C.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_C.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_C.Location = new System.Drawing.Point(84, 35);
            this.B_C.Name = "B_C";
            this.B_C.Size = new System.Drawing.Size(33, 34);
            this.B_C.TabIndex = 0;
            this.B_C.Text = "C";
            this.B_C.UseVisualStyleBackColor = true;
            this.B_C.Click += new System.EventHandler(this.B_C_Click);
            // 
            // TickDownTimer
            // 
            this.TickDownTimer.Interval = 1;
            this.TickDownTimer.Tick += new System.EventHandler(this.TickDownTimer_Tick);
            // 
            // SC_Core
            // 
            this.SC_Core.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SC_Core.Location = new System.Drawing.Point(0, 0);
            this.SC_Core.Name = "SC_Core";
            // 
            // SC_Core.Panel1
            // 
            this.SC_Core.Panel1.Controls.Add(this.SC_Left);
            // 
            // SC_Core.Panel2
            // 
            this.SC_Core.Panel2.Controls.Add(this.LB_Notes);
            this.SC_Core.Size = new System.Drawing.Size(664, 367);
            this.SC_Core.SplitterDistance = 488;
            this.SC_Core.TabIndex = 2;
            // 
            // LB_Notes
            // 
            this.LB_Notes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB_Notes.FormattingEnabled = true;
            this.LB_Notes.Location = new System.Drawing.Point(0, 0);
            this.LB_Notes.Name = "LB_Notes";
            this.LB_Notes.Size = new System.Drawing.Size(172, 367);
            this.LB_Notes.TabIndex = 0;
            // 
            // difficultiesBindingSource
            // 
            this.difficultiesBindingSource.DataSource = typeof(SongProofForms.Resources.Difficulties);
            // 
            // SessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 367);
            this.Controls.Add(this.SC_Core);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 406);
            this.Name = "SessionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SessionForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormClosed_ShutDown);
            this.SC_Left.Panel1.ResumeLayout(false);
            this.SC_Left.Panel2.ResumeLayout(false);
            this.SC_Left.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC_Left)).EndInit();
            this.SC_Left.ResumeLayout(false);
            this.SC_NoteTime.Panel1.ResumeLayout(false);
            this.SC_NoteTime.Panel1.PerformLayout();
            this.SC_NoteTime.Panel2.ResumeLayout(false);
            this.SC_NoteTime.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SC_NoteTime)).EndInit();
            this.SC_NoteTime.ResumeLayout(false);
            this.SC_Core.Panel1.ResumeLayout(false);
            this.SC_Core.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SC_Core)).EndInit();
            this.SC_Core.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.difficultiesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SC_Left;
        private System.Windows.Forms.SplitContainer SC_NoteTime;
        private System.Windows.Forms.Label L_NoteNumber;
        private System.Windows.Forms.Label L_Timer;
        private System.Windows.Forms.Button B_B;
        private System.Windows.Forms.Button B_AsBb;
        private System.Windows.Forms.Button B_A;
        private System.Windows.Forms.Button B_GsAb;
        private System.Windows.Forms.Button B_G;
        private System.Windows.Forms.Button B_FsGb;
        private System.Windows.Forms.Button B_F;
        private System.Windows.Forms.Button B_E;
        private System.Windows.Forms.Button B_DsEb;
        private System.Windows.Forms.Button B_D;
        private System.Windows.Forms.Button B_CsDb;
        private System.Windows.Forms.Button B_C;
        private System.Windows.Forms.Label L_Scale;
        private System.Windows.Forms.Label L_TimeLeft;
        private System.Windows.Forms.Timer TickDownTimer;
        private System.Windows.Forms.Button B_Quit;
        private System.Windows.Forms.Button B_ViewData;
        private System.Windows.Forms.Button B_Start;
        private System.Windows.Forms.Label L_NoteCount;
        private System.Windows.Forms.SplitContainer SC_Core;
        private System.Windows.Forms.ListBox LB_Notes;
        private System.Windows.Forms.BindingSource difficultiesBindingSource;
    }
}