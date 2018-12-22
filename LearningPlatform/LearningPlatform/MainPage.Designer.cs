namespace LearningPlatform
{
    partial class MainPage
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.ambiance_ThemeContainer1 = new Ambiance.Ambiance_ThemeContainer();
            this.ExitButton = new Ambiance.Ambiance_Button_2();
            this.Logout = new Ambiance.Ambiance_Button_1();
            this.addStudentButton = new Ambiance.Ambiance_Button_1();
            this.createClassButton = new Ambiance.Ambiance_Button_2();
            this.classTabs = new Ambiance.Ambiance_TabControl();
            this.ambiance_ThemeContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // ambiance_ThemeContainer1
            // 
            this.ambiance_ThemeContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.ambiance_ThemeContainer1.Controls.Add(this.ExitButton);
            this.ambiance_ThemeContainer1.Controls.Add(this.Logout);
            this.ambiance_ThemeContainer1.Controls.Add(this.addStudentButton);
            this.ambiance_ThemeContainer1.Controls.Add(this.createClassButton);
            this.ambiance_ThemeContainer1.Controls.Add(this.classTabs);
            this.ambiance_ThemeContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ambiance_ThemeContainer1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ambiance_ThemeContainer1.Location = new System.Drawing.Point(0, 0);
            this.ambiance_ThemeContainer1.Name = "ambiance_ThemeContainer1";
            this.ambiance_ThemeContainer1.Padding = new System.Windows.Forms.Padding(20, 56, 20, 16);
            this.ambiance_ThemeContainer1.RoundCorners = true;
            this.ambiance_ThemeContainer1.Sizable = true;
            this.ambiance_ThemeContainer1.Size = new System.Drawing.Size(967, 565);
            this.ambiance_ThemeContainer1.SmartBounds = true;
            this.ambiance_ThemeContainer1.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.ambiance_ThemeContainer1.TabIndex = 0;
            this.ambiance_ThemeContainer1.Text = "Generic Learning Platform";
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ExitButton.Image = null;
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExitButton.Location = new System.Drawing.Point(763, 505);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(177, 30);
            this.ExitButton.TabIndex = 8;
            this.ExitButton.Text = "Exit";
            this.ExitButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // Logout
            // 
            this.Logout.BackColor = System.Drawing.Color.Transparent;
            this.Logout.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Logout.Image = null;
            this.Logout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Logout.Location = new System.Drawing.Point(27, 505);
            this.Logout.Name = "Logout";
            this.Logout.Size = new System.Drawing.Size(177, 30);
            this.Logout.TabIndex = 7;
            this.Logout.Text = "Logout";
            this.Logout.TextAlignment = System.Drawing.StringAlignment.Center;
            this.Logout.Click += new System.EventHandler(this.Logout_Click);
            // 
            // addStudentButton
            // 
            this.addStudentButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.addStudentButton.BackColor = System.Drawing.Color.Transparent;
            this.addStudentButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.addStudentButton.Image = null;
            this.addStudentButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addStudentButton.Location = new System.Drawing.Point(520, 505);
            this.addStudentButton.Name = "addStudentButton";
            this.addStudentButton.Size = new System.Drawing.Size(177, 30);
            this.addStudentButton.TabIndex = 6;
            this.addStudentButton.Text = "Add Student";
            this.addStudentButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.addStudentButton.Click += new System.EventHandler(this.addStudentButton_Click);
            // 
            // createClassButton
            // 
            this.createClassButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.createClassButton.BackColor = System.Drawing.Color.Transparent;
            this.createClassButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.createClassButton.Image = null;
            this.createClassButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.createClassButton.Location = new System.Drawing.Point(274, 505);
            this.createClassButton.Name = "createClassButton";
            this.createClassButton.Size = new System.Drawing.Size(177, 30);
            this.createClassButton.TabIndex = 5;
            this.createClassButton.Text = "Create Class";
            this.createClassButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.createClassButton.Click += new System.EventHandler(this.createClassButton_Click);
            // 
            // classTabs
            // 
            this.classTabs.ItemSize = new System.Drawing.Size(80, 24);
            this.classTabs.Location = new System.Drawing.Point(23, 59);
            this.classTabs.Name = "classTabs";
            this.classTabs.SelectedIndex = 0;
            this.classTabs.Size = new System.Drawing.Size(921, 427);
            this.classTabs.TabIndex = 0;
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 565);
            this.Controls.Add(this.ambiance_ThemeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 65);
            this.Name = "MainPage";
            this.Text = "Generic Learning Platform";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ambiance_ThemeContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Ambiance.Ambiance_ThemeContainer ambiance_ThemeContainer1;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        public Ambiance.Ambiance_TabControl classTabs;
        private Ambiance.Ambiance_Button_1 addStudentButton;
        private Ambiance.Ambiance_Button_2 createClassButton;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private Ambiance.Ambiance_Button_2 ExitButton;
        private Ambiance.Ambiance_Button_1 Logout;
    }
}