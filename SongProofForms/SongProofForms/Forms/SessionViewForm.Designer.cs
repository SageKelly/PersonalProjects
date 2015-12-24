namespace SongProofForms
{
    partial class SessionViewForm
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
            this.LL_Scale = new System.Windows.Forms.Label();
            this.L_Scale = new System.Windows.Forms.Label();
            this.LL_Diff = new System.Windows.Forms.Label();
            this.L_Diff = new System.Windows.Forms.Label();
            this.LL_TestLength = new System.Windows.Forms.Label();
            this.L_TestLength = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.B_CloseForm = new System.Windows.Forms.Button();
            this.B_OpenSession = new System.Windows.Forms.Button();
            this.TB_TFN5 = new System.Windows.Forms.TextBox();
            this.TB_TFN4 = new System.Windows.Forms.TextBox();
            this.TB_TFN3 = new System.Windows.Forms.TextBox();
            this.TB_TFN2 = new System.Windows.Forms.TextBox();
            this.TB_TFN1 = new System.Windows.Forms.TextBox();
            this.LL_TopFiveNotes = new System.Windows.Forms.Label();
            this.LB_ScaleNotes = new System.Windows.Forms.ListBox();
            this.LL_AvgGuessTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LL_Scale
            // 
            this.LL_Scale.AutoSize = true;
            this.LL_Scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Scale.Location = new System.Drawing.Point(26, 7);
            this.LL_Scale.Name = "LL_Scale";
            this.LL_Scale.Size = new System.Drawing.Size(125, 24);
            this.LL_Scale.TabIndex = 0;
            this.LL_Scale.Text = "Scale Tested:";
            // 
            // L_Scale
            // 
            this.L_Scale.AutoSize = true;
            this.L_Scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Scale.Location = new System.Drawing.Point(157, 7);
            this.L_Scale.Name = "L_Scale";
            this.L_Scale.Size = new System.Drawing.Size(15, 24);
            this.L_Scale.TabIndex = 1;
            this.L_Scale.Text = " ";
            // 
            // LL_Diff
            // 
            this.LL_Diff.AutoSize = true;
            this.LL_Diff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Diff.Location = new System.Drawing.Point(26, 39);
            this.LL_Diff.Name = "LL_Diff";
            this.LL_Diff.Size = new System.Drawing.Size(82, 24);
            this.LL_Diff.TabIndex = 2;
            this.LL_Diff.Text = "Difficulty:";
            // 
            // L_Diff
            // 
            this.L_Diff.AutoSize = true;
            this.L_Diff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Diff.Location = new System.Drawing.Point(154, 39);
            this.L_Diff.Name = "L_Diff";
            this.L_Diff.Size = new System.Drawing.Size(0, 24);
            this.L_Diff.TabIndex = 3;
            // 
            // LL_TestLength
            // 
            this.LL_TestLength.AutoSize = true;
            this.LL_TestLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_TestLength.Location = new System.Drawing.Point(26, 75);
            this.LL_TestLength.Name = "LL_TestLength";
            this.LL_TestLength.Size = new System.Drawing.Size(114, 24);
            this.LL_TestLength.TabIndex = 4;
            this.LL_TestLength.Text = "Test Length:";
            // 
            // L_TestLength
            // 
            this.L_TestLength.AutoSize = true;
            this.L_TestLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_TestLength.Location = new System.Drawing.Point(154, 75);
            this.L_TestLength.Name = "L_TestLength";
            this.L_TestLength.Size = new System.Drawing.Size(15, 24);
            this.L_TestLength.TabIndex = 5;
            this.L_TestLength.Text = " ";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.TB_TFN5);
            this.splitContainer1.Panel2.Controls.Add(this.TB_TFN4);
            this.splitContainer1.Panel2.Controls.Add(this.TB_TFN3);
            this.splitContainer1.Panel2.Controls.Add(this.TB_TFN2);
            this.splitContainer1.Panel2.Controls.Add(this.TB_TFN1);
            this.splitContainer1.Panel2.Controls.Add(this.LL_TopFiveNotes);
            this.splitContainer1.Panel2.Controls.Add(this.LB_ScaleNotes);
            this.splitContainer1.Panel2.Controls.Add(this.LL_AvgGuessTime);
            this.splitContainer1.Size = new System.Drawing.Size(484, 417);
            this.splitContainer1.SplitterDistance = 114;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.B_CloseForm);
            this.splitContainer2.Panel1.Controls.Add(this.B_OpenSession);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.L_Scale);
            this.splitContainer2.Panel2.Controls.Add(this.LL_Scale);
            this.splitContainer2.Panel2.Controls.Add(this.LL_Diff);
            this.splitContainer2.Panel2.Controls.Add(this.L_Diff);
            this.splitContainer2.Panel2.Controls.Add(this.LL_TestLength);
            this.splitContainer2.Panel2.Controls.Add(this.L_TestLength);
            this.splitContainer2.Size = new System.Drawing.Size(484, 114);
            this.splitContainer2.SplitterDistance = 170;
            this.splitContainer2.TabIndex = 6;
            // 
            // B_CloseForm
            // 
            this.B_CloseForm.AutoSize = true;
            this.B_CloseForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_CloseForm.Location = new System.Drawing.Point(10, 50);
            this.B_CloseForm.Name = "B_CloseForm";
            this.B_CloseForm.Size = new System.Drawing.Size(140, 34);
            this.B_CloseForm.TabIndex = 0;
            this.B_CloseForm.Text = "Close";
            this.B_CloseForm.UseVisualStyleBackColor = true;
            this.B_CloseForm.Click += new System.EventHandler(this.B_CloseForm_Click);
            // 
            // B_OpenSession
            // 
            this.B_OpenSession.AutoSize = true;
            this.B_OpenSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_OpenSession.Location = new System.Drawing.Point(10, 10);
            this.B_OpenSession.Name = "B_OpenSession";
            this.B_OpenSession.Size = new System.Drawing.Size(140, 34);
            this.B_OpenSession.TabIndex = 0;
            this.B_OpenSession.Text = "Open Session";
            this.B_OpenSession.UseVisualStyleBackColor = true;
            this.B_OpenSession.Click += new System.EventHandler(this.B_OpenSession_Click);
            // 
            // TB_TFN5
            // 
            this.TB_TFN5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TFN5.Location = new System.Drawing.Point(335, 197);
            this.TB_TFN5.Name = "TB_TFN5";
            this.TB_TFN5.Size = new System.Drawing.Size(100, 29);
            this.TB_TFN5.TabIndex = 3;
            // 
            // TB_TFN4
            // 
            this.TB_TFN4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TFN4.Location = new System.Drawing.Point(335, 162);
            this.TB_TFN4.Name = "TB_TFN4";
            this.TB_TFN4.Size = new System.Drawing.Size(100, 29);
            this.TB_TFN4.TabIndex = 3;
            // 
            // TB_TFN3
            // 
            this.TB_TFN3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TFN3.Location = new System.Drawing.Point(335, 127);
            this.TB_TFN3.Name = "TB_TFN3";
            this.TB_TFN3.Size = new System.Drawing.Size(100, 29);
            this.TB_TFN3.TabIndex = 3;
            // 
            // TB_TFN2
            // 
            this.TB_TFN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TFN2.Location = new System.Drawing.Point(335, 92);
            this.TB_TFN2.Name = "TB_TFN2";
            this.TB_TFN2.Size = new System.Drawing.Size(100, 29);
            this.TB_TFN2.TabIndex = 3;
            // 
            // TB_TFN1
            // 
            this.TB_TFN1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_TFN1.Location = new System.Drawing.Point(335, 56);
            this.TB_TFN1.Name = "TB_TFN1";
            this.TB_TFN1.Size = new System.Drawing.Size(100, 29);
            this.TB_TFN1.TabIndex = 3;
            // 
            // LL_TopFiveNotes
            // 
            this.LL_TopFiveNotes.AutoSize = true;
            this.LL_TopFiveNotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_TopFiveNotes.Location = new System.Drawing.Point(331, 24);
            this.LL_TopFiveNotes.Name = "LL_TopFiveNotes";
            this.LL_TopFiveNotes.Size = new System.Drawing.Size(144, 24);
            this.LL_TopFiveNotes.TabIndex = 2;
            this.LL_TopFiveNotes.Text = "Top Five Notes:";
            // 
            // LB_ScaleNotes
            // 
            this.LB_ScaleNotes.FormattingEnabled = true;
            this.LB_ScaleNotes.Location = new System.Drawing.Point(16, 56);
            this.LB_ScaleNotes.Name = "LB_ScaleNotes";
            this.LB_ScaleNotes.Size = new System.Drawing.Size(256, 199);
            this.LB_ScaleNotes.TabIndex = 1;
            // 
            // LL_AvgGuessTime
            // 
            this.LL_AvgGuessTime.AutoSize = true;
            this.LL_AvgGuessTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_AvgGuessTime.Location = new System.Drawing.Point(12, 24);
            this.LL_AvgGuessTime.Name = "LL_AvgGuessTime";
            this.LL_AvgGuessTime.Size = new System.Drawing.Size(260, 24);
            this.LL_AvgGuessTime.TabIndex = 0;
            this.LL_AvgGuessTime.Text = "Avg Guessing Time Per Note:";
            // 
            // SessionViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 417);
            this.Controls.Add(this.splitContainer1);
            this.MaximumSize = new System.Drawing.Size(500, 456);
            this.MinimumSize = new System.Drawing.Size(500, 456);
            this.Name = "SessionViewForm";
            this.Text = "Session Review";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LL_Scale;
        private System.Windows.Forms.Label L_Scale;
        private System.Windows.Forms.Label LL_Diff;
        private System.Windows.Forms.Label L_Diff;
        private System.Windows.Forms.Label LL_TestLength;
        private System.Windows.Forms.Label L_TestLength;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label LL_AvgGuessTime;
        private System.Windows.Forms.TextBox TB_TFN5;
        private System.Windows.Forms.TextBox TB_TFN4;
        private System.Windows.Forms.TextBox TB_TFN3;
        private System.Windows.Forms.TextBox TB_TFN2;
        private System.Windows.Forms.TextBox TB_TFN1;
        private System.Windows.Forms.Label LL_TopFiveNotes;
        private System.Windows.Forms.ListBox LB_ScaleNotes;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button B_CloseForm;
        private System.Windows.Forms.Button B_OpenSession;
    }
}