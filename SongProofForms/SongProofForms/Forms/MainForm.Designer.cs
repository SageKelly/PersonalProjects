namespace SongProofForms
{
    partial class MainForm
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
            this.B_Session_Start = new System.Windows.Forms.Button();
            this.B_Settings = new System.Windows.Forms.Button();
            this.B_Session_Data = new System.Windows.Forms.Button();
            this.B_Exit = new System.Windows.Forms.Button();
            this.L_Title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // B_Session_Start
            // 
            this.B_Session_Start.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.B_Session_Start.AutoSize = true;
            this.B_Session_Start.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_Session_Start.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Session_Start.Location = new System.Drawing.Point(165, 152);
            this.B_Session_Start.Name = "B_Session_Start";
            this.B_Session_Start.Size = new System.Drawing.Size(128, 34);
            this.B_Session_Start.TabIndex = 7;
            this.B_Session_Start.Text = "Start Session";
            this.B_Session_Start.UseVisualStyleBackColor = true;
            this.B_Session_Start.Click += new System.EventHandler(this.B_Session_Start_Click);
            // 
            // B_Settings
            // 
            this.B_Settings.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.B_Settings.AutoSize = true;
            this.B_Settings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Settings.Location = new System.Drawing.Point(188, 232);
            this.B_Settings.Name = "B_Settings";
            this.B_Settings.Size = new System.Drawing.Size(86, 34);
            this.B_Settings.TabIndex = 8;
            this.B_Settings.Text = "Settings";
            this.B_Settings.UseVisualStyleBackColor = true;
            // 
            // B_Session_Data
            // 
            this.B_Session_Data.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.B_Session_Data.AutoSize = true;
            this.B_Session_Data.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_Session_Data.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Session_Data.Location = new System.Drawing.Point(140, 192);
            this.B_Session_Data.Name = "B_Session_Data";
            this.B_Session_Data.Size = new System.Drawing.Size(176, 34);
            this.B_Session_Data.TabIndex = 9;
            this.B_Session_Data.Text = "View Session Data";
            this.B_Session_Data.UseVisualStyleBackColor = true;
            this.B_Session_Data.Click += new System.EventHandler(this.B_Session_Data_Click);
            // 
            // B_Exit
            // 
            this.B_Exit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.B_Exit.AutoSize = true;
            this.B_Exit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.B_Exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B_Exit.Location = new System.Drawing.Point(202, 272);
            this.B_Exit.Name = "B_Exit";
            this.B_Exit.Size = new System.Drawing.Size(51, 34);
            this.B_Exit.TabIndex = 10;
            this.B_Exit.Text = "Exit";
            this.B_Exit.UseVisualStyleBackColor = true;
            this.B_Exit.Click += new System.EventHandler(this.B_Exit_Click);
            // 
            // L_Title
            // 
            this.L_Title.AutoSize = true;
            this.L_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Title.Location = new System.Drawing.Point(66, 38);
            this.L_Title.MaximumSize = new System.Drawing.Size(358, 73);
            this.L_Title.MinimumSize = new System.Drawing.Size(358, 73);
            this.L_Title.Name = "L_Title";
            this.L_Title.Size = new System.Drawing.Size(358, 73);
            this.L_Title.TabIndex = 11;
            this.L_Title.Text = "Song-Proof";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 367);
            this.Controls.Add(this.L_Title);
            this.Controls.Add(this.B_Exit);
            this.Controls.Add(this.B_Session_Data);
            this.Controls.Add(this.B_Settings);
            this.Controls.Add(this.B_Session_Start);
            this.Location = new System.Drawing.Point(500, 500);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 406);
            this.MinimumSize = new System.Drawing.Size(500, 406);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song-Proof";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Session_Start;
        private System.Windows.Forms.Button B_Settings;
        private System.Windows.Forms.Button B_Session_Data;
        private System.Windows.Forms.Button B_Exit;
        protected System.Windows.Forms.Label L_Title;

    }
}

