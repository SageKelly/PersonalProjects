namespace SongProofForms
{
    partial class OpenSessionForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LL_ChooseSession = new System.Windows.Forms.Label();
            this.LB_Sessions = new System.Windows.Forms.ListBox();
            this.LL_Date = new System.Windows.Forms.Label();
            this.LL_Time = new System.Windows.Forms.Label();
            this.LL_Scale = new System.Windows.Forms.Label();
            this.LL_Diff = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.L_Date = new System.Windows.Forms.Label();
            this.L_Time = new System.Windows.Forms.Label();
            this.L_Scale = new System.Windows.Forms.Label();
            this.L_Diff = new System.Windows.Forms.Label();
            this.L_Open = new System.Windows.Forms.Button();
            this.L_Cancel = new System.Windows.Forms.Button();
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
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LB_Sessions);
            this.splitContainer1.Panel1.Controls.Add(this.LL_ChooseSession);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.L_Date);
            this.splitContainer1.Panel2.Controls.Add(this.LL_Date);
            this.splitContainer1.Panel2.Controls.Add(this.L_Time);
            this.splitContainer1.Panel2.Controls.Add(this.LL_Time);
            this.splitContainer1.Panel2.Controls.Add(this.L_Scale);
            this.splitContainer1.Panel2.Controls.Add(this.LL_Scale);
            this.splitContainer1.Panel2.Controls.Add(this.L_Diff);
            this.splitContainer1.Panel2.Controls.Add(this.LL_Diff);
            this.splitContainer1.Size = new System.Drawing.Size(484, 368);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 0;
            // 
            // LL_ChooseSession
            // 
            this.LL_ChooseSession.AutoSize = true;
            this.LL_ChooseSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_ChooseSession.Location = new System.Drawing.Point(12, 9);
            this.LL_ChooseSession.Name = "LL_ChooseSession";
            this.LL_ChooseSession.Size = new System.Drawing.Size(149, 24);
            this.LL_ChooseSession.TabIndex = 0;
            this.LL_ChooseSession.Text = "Select a Session";
            // 
            // LB_Sessions
            // 
            this.LB_Sessions.FormattingEnabled = true;
            this.LB_Sessions.Location = new System.Drawing.Point(-2, 34);
            this.LB_Sessions.Name = "LB_Sessions";
            this.LB_Sessions.Size = new System.Drawing.Size(246, 381);
            this.LB_Sessions.TabIndex = 1;
            // 
            // LL_Date
            // 
            this.LL_Date.AutoSize = true;
            this.LL_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Date.Location = new System.Drawing.Point(0, 9);
            this.LL_Date.Name = "LL_Date";
            this.LL_Date.Size = new System.Drawing.Size(53, 24);
            this.LL_Date.TabIndex = 0;
            this.LL_Date.Text = "Date:";
            // 
            // LL_Time
            // 
            this.LL_Time.AutoSize = true;
            this.LL_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Time.Location = new System.Drawing.Point(0, 33);
            this.LL_Time.Name = "LL_Time";
            this.LL_Time.Size = new System.Drawing.Size(58, 24);
            this.LL_Time.TabIndex = 0;
            this.LL_Time.Text = "Time:";
            // 
            // LL_Scale
            // 
            this.LL_Scale.AutoSize = true;
            this.LL_Scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Scale.Location = new System.Drawing.Point(0, 57);
            this.LL_Scale.Name = "LL_Scale";
            this.LL_Scale.Size = new System.Drawing.Size(62, 24);
            this.LL_Scale.TabIndex = 0;
            this.LL_Scale.Text = "Scale:";
            // 
            // LL_Diff
            // 
            this.LL_Diff.AutoSize = true;
            this.LL_Diff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LL_Diff.Location = new System.Drawing.Point(0, 81);
            this.LL_Diff.Name = "LL_Diff";
            this.LL_Diff.Size = new System.Drawing.Size(82, 24);
            this.LL_Diff.TabIndex = 0;
            this.LL_Diff.Text = "Difficulty:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.L_Cancel);
            this.splitContainer2.Panel2.Controls.Add(this.L_Open);
            this.splitContainer2.Size = new System.Drawing.Size(484, 417);
            this.splitContainer2.SplitterDistance = 368;
            this.splitContainer2.TabIndex = 1;
            // 
            // L_Date
            // 
            this.L_Date.AutoSize = true;
            this.L_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Date.Location = new System.Drawing.Point(85, 11);
            this.L_Date.Name = "L_Date";
            this.L_Date.Size = new System.Drawing.Size(15, 24);
            this.L_Date.TabIndex = 0;
            this.L_Date.Text = " ";
            // 
            // L_Time
            // 
            this.L_Time.AutoSize = true;
            this.L_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Time.Location = new System.Drawing.Point(85, 33);
            this.L_Time.Name = "L_Time";
            this.L_Time.Size = new System.Drawing.Size(15, 24);
            this.L_Time.TabIndex = 0;
            this.L_Time.Text = " ";
            // 
            // L_Scale
            // 
            this.L_Scale.AutoSize = true;
            this.L_Scale.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Scale.Location = new System.Drawing.Point(85, 57);
            this.L_Scale.Name = "L_Scale";
            this.L_Scale.Size = new System.Drawing.Size(15, 24);
            this.L_Scale.TabIndex = 0;
            this.L_Scale.Text = " ";
            // 
            // L_Diff
            // 
            this.L_Diff.AutoSize = true;
            this.L_Diff.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Diff.Location = new System.Drawing.Point(85, 81);
            this.L_Diff.Name = "L_Diff";
            this.L_Diff.Size = new System.Drawing.Size(15, 24);
            this.L_Diff.TabIndex = 0;
            this.L_Diff.Text = " ";
            // 
            // L_Open
            // 
            this.L_Open.AutoSize = true;
            this.L_Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Open.Location = new System.Drawing.Point(300, 6);
            this.L_Open.Name = "L_Open";
            this.L_Open.Size = new System.Drawing.Size(82, 34);
            this.L_Open.TabIndex = 0;
            this.L_Open.Text = "Open";
            this.L_Open.UseVisualStyleBackColor = true;
            // 
            // L_Cancel
            // 
            this.L_Cancel.AutoSize = true;
            this.L_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Cancel.Location = new System.Drawing.Point(388, 6);
            this.L_Cancel.Name = "L_Cancel";
            this.L_Cancel.Size = new System.Drawing.Size(82, 34);
            this.L_Cancel.TabIndex = 0;
            this.L_Cancel.Text = "Cancel";
            this.L_Cancel.UseVisualStyleBackColor = true;
            // 
            // OpenSessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 417);
            this.Controls.Add(this.splitContainer2);
            this.MaximumSize = new System.Drawing.Size(500, 456);
            this.MinimumSize = new System.Drawing.Size(500, 456);
            this.Name = "OpenSessionForm";
            this.Text = "Open Session";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox LB_Sessions;
        private System.Windows.Forms.Label LL_ChooseSession;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label LL_Diff;
        private System.Windows.Forms.Label LL_Scale;
        private System.Windows.Forms.Label LL_Time;
        private System.Windows.Forms.Label LL_Date;
        private System.Windows.Forms.Label L_Date;
        private System.Windows.Forms.Label L_Time;
        private System.Windows.Forms.Label L_Scale;
        private System.Windows.Forms.Label L_Diff;
        private System.Windows.Forms.Button L_Cancel;
        private System.Windows.Forms.Button L_Open;
    }
}