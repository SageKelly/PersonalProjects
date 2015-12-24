namespace SongProofForms
{
    partial class SessionSetupForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_Scales = new System.Windows.Forms.ComboBox();
            this.CB_Diff = new System.Windows.Forms.ComboBox();
            this.L_SetupSession = new System.Windows.Forms.Label();
            this.B_Begin = new System.Windows.Forms.Button();
            this.NUD_NoteCount = new System.Windows.Forms.NumericUpDown();
            this.C_IsSharp = new System.Windows.Forms.CheckBox();
            Size = FormResource.ScreenSize;
            MaximumSize = FormResource.ScreenSize;
            MinimumSize = FormResource.ScreenSize;
            ((System.ComponentModel.ISupportInitialize)(this.NUD_NoteCount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose your Scale";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "How many notes for this session?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "What is the difficulty?";
            // 
            // CB_Scales
            // 
            this.CB_Scales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Scales.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_Scales.FormattingEnabled = true;
            this.CB_Scales.Location = new System.Drawing.Point(16, 126);
            this.CB_Scales.Name = "CB_Scales";
            this.CB_Scales.Size = new System.Drawing.Size(121, 26);
            this.CB_Scales.TabIndex = 2;
            // 
            // CB_Diff
            // 
            this.CB_Diff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Diff.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CB_Diff.FormattingEnabled = true;
            this.CB_Diff.Location = new System.Drawing.Point(16, 272);
            this.CB_Diff.Name = "CB_Diff";
            this.CB_Diff.Size = new System.Drawing.Size(121, 26);
            this.CB_Diff.TabIndex = 6;
            // 
            // L_SetupSession
            // 
            this.L_SetupSession.AutoSize = true;
            this.L_SetupSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_SetupSession.Location = new System.Drawing.Point(13, 13);
            this.L_SetupSession.Name = "L_SetupSession";
            this.L_SetupSession.Size = new System.Drawing.Size(334, 55);
            this.L_SetupSession.TabIndex = 0;
            this.L_SetupSession.Text = "Setup Session";
            // 
            // B_Begin
            // 
            this.B_Begin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Begin.AutoSize = true;
            this.B_Begin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Begin.Location = new System.Drawing.Point(397, 321);
            this.B_Begin.Name = "B_Begin";
            this.B_Begin.Size = new System.Drawing.Size(75, 34);
            this.B_Begin.TabIndex = 7;
            this.B_Begin.Text = "&Begin";
            this.B_Begin.UseVisualStyleBackColor = true;
            this.B_Begin.Click += new System.EventHandler(this.B_Begin_Click);
            // 
            // NUD_NoteCount
            // 
            this.NUD_NoteCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NUD_NoteCount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUD_NoteCount.Location = new System.Drawing.Point(16, 203);
            this.NUD_NoteCount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NUD_NoteCount.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUD_NoteCount.Name = "NUD_NoteCount";
            this.NUD_NoteCount.Size = new System.Drawing.Size(120, 24);
            this.NUD_NoteCount.TabIndex = 4;
            this.NUD_NoteCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // C_IsSharp
            // 
            this.C_IsSharp.AutoSize = true;
            this.C_IsSharp.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C_IsSharp.Location = new System.Drawing.Point(144, 127);
            this.C_IsSharp.Name = "C_IsSharp";
            this.C_IsSharp.Size = new System.Drawing.Size(150, 22);
            this.C_IsSharp.TabIndex = 8;
            this.C_IsSharp.Text = "Sharp Orientation?";
            this.C_IsSharp.UseVisualStyleBackColor = true;
            // 
            // SessionSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 367);
            this.Controls.Add(this.C_IsSharp);
            this.Controls.Add(this.NUD_NoteCount);
            this.Controls.Add(this.B_Begin);
            this.Controls.Add(this.L_SetupSession);
            this.Controls.Add(this.CB_Diff);
            this.Controls.Add(this.CB_Scales);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(500, 500);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 406);
            this.MinimumSize = new System.Drawing.Size(500, 406);
            this.Name = "SessionSetupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Song-Proof";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CloseProgram);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_NoteCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CB_Scales;
        private System.Windows.Forms.ComboBox CB_Diff;
        private System.Windows.Forms.Label L_SetupSession;
        private System.Windows.Forms.Button B_Begin;
        private System.Windows.Forms.NumericUpDown NUD_NoteCount;
        private System.Windows.Forms.CheckBox C_IsSharp;
    }
}