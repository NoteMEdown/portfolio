namespace LearningPlatform
{
    partial class Form1
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
            this.background = new Ambiance.Ambiance_ThemeContainer();
            this.exit_button = new Ambiance.Ambiance_Button_1();
            this.pass_label = new Ambiance.Ambiance_Label();
            this.Password_Text = new Ambiance.Ambiance_TextBox();
            this.user_label = new Ambiance.Ambiance_Label();
            this.Username_Text = new Ambiance.Ambiance_TextBox();
            this.RegisterButton = new Ambiance.Ambiance_Button_2();
            this.TeacherLogin = new Ambiance.Ambiance_Button_2();
            this.StudentLogin = new Ambiance.Ambiance_Button_1();
            this.background.SuspendLayout();
            this.SuspendLayout();
            // 
            // background
            // 
            this.background.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.background.Controls.Add(this.exit_button);
            this.background.Controls.Add(this.pass_label);
            this.background.Controls.Add(this.Password_Text);
            this.background.Controls.Add(this.user_label);
            this.background.Controls.Add(this.Username_Text);
            this.background.Controls.Add(this.RegisterButton);
            this.background.Controls.Add(this.TeacherLogin);
            this.background.Controls.Add(this.StudentLogin);
            this.background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.background.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.background.Location = new System.Drawing.Point(0, 0);
            this.background.Name = "background";
            this.background.Padding = new System.Windows.Forms.Padding(20, 56, 20, 16);
            this.background.RoundCorners = true;
            this.background.Sizable = false;
            this.background.Size = new System.Drawing.Size(562, 309);
            this.background.SmartBounds = true;
            this.background.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.background.TabIndex = 0;
            this.background.Text = "Generic Learning Platform";
            this.background.Click += new System.EventHandler(this.ambiance_ThemeContainer1_Click);
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.Transparent;
            this.exit_button.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.exit_button.Image = null;
            this.exit_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exit_button.Location = new System.Drawing.Point(447, 260);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(92, 30);
            this.exit_button.TabIndex = 5;
            this.exit_button.Text = "Exit";
            this.exit_button.TextAlignment = System.Drawing.StringAlignment.Center;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click_1);
            // 
            // pass_label
            // 
            this.pass_label.AutoSize = true;
            this.pass_label.BackColor = System.Drawing.Color.Transparent;
            this.pass_label.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.pass_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.pass_label.Location = new System.Drawing.Point(89, 122);
            this.pass_label.Name = "pass_label";
            this.pass_label.Size = new System.Drawing.Size(73, 20);
            this.pass_label.TabIndex = 6;
            this.pass_label.Text = "Password:";
            // 
            // Password_Text
            // 
            this.Password_Text.BackColor = System.Drawing.Color.Transparent;
            this.Password_Text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.Password_Text.ForeColor = System.Drawing.Color.DimGray;
            this.Password_Text.Location = new System.Drawing.Point(168, 122);
            this.Password_Text.MaxLength = 32767;
            this.Password_Text.Multiline = false;
            this.Password_Text.Name = "Password_Text";
            this.Password_Text.ReadOnly = false;
            this.Password_Text.Size = new System.Drawing.Size(313, 28);
            this.Password_Text.TabIndex = 1;
            this.Password_Text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.Password_Text.UseSystemPasswordChar = true;
            // 
            // user_label
            // 
            this.user_label.AutoSize = true;
            this.user_label.BackColor = System.Drawing.Color.Transparent;
            this.user_label.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.user_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.user_label.Location = new System.Drawing.Point(76, 81);
            this.user_label.Name = "user_label";
            this.user_label.Size = new System.Drawing.Size(86, 20);
            this.user_label.TabIndex = 4;
            this.user_label.Text = "ID or Email:";
            // 
            // Username_Text
            // 
            this.Username_Text.BackColor = System.Drawing.Color.Transparent;
            this.Username_Text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.Username_Text.ForeColor = System.Drawing.Color.DimGray;
            this.Username_Text.Location = new System.Drawing.Point(168, 77);
            this.Username_Text.MaxLength = 32767;
            this.Username_Text.Multiline = false;
            this.Username_Text.Name = "Username_Text";
            this.Username_Text.ReadOnly = false;
            this.Username_Text.Size = new System.Drawing.Size(313, 28);
            this.Username_Text.TabIndex = 0;
            this.Username_Text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.Username_Text.UseSystemPasswordChar = false;
            // 
            // RegisterButton
            // 
            this.RegisterButton.BackColor = System.Drawing.Color.Transparent;
            this.RegisterButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.RegisterButton.Image = null;
            this.RegisterButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RegisterButton.Location = new System.Drawing.Point(184, 227);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(177, 30);
            this.RegisterButton.TabIndex = 4;
            this.RegisterButton.Text = "Register As Teacher";
            this.RegisterButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // TeacherLogin
            // 
            this.TeacherLogin.BackColor = System.Drawing.Color.Transparent;
            this.TeacherLogin.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.TeacherLogin.Image = null;
            this.TeacherLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TeacherLogin.Location = new System.Drawing.Point(76, 172);
            this.TeacherLogin.Name = "TeacherLogin";
            this.TeacherLogin.Size = new System.Drawing.Size(177, 30);
            this.TeacherLogin.TabIndex = 3;
            this.TeacherLogin.Text = "Login As Teacher";
            this.TeacherLogin.TextAlignment = System.Drawing.StringAlignment.Center;
            this.TeacherLogin.Click += new System.EventHandler(this.ambiance_Button_21_Click);
            // 
            // StudentLogin
            // 
            this.StudentLogin.BackColor = System.Drawing.Color.Transparent;
            this.StudentLogin.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.StudentLogin.Image = null;
            this.StudentLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StudentLogin.Location = new System.Drawing.Point(304, 172);
            this.StudentLogin.Name = "StudentLogin";
            this.StudentLogin.Size = new System.Drawing.Size(177, 30);
            this.StudentLogin.TabIndex = 2;
            this.StudentLogin.Text = "Login As Student";
            this.StudentLogin.TextAlignment = System.Drawing.StringAlignment.Center;
            this.StudentLogin.Click += new System.EventHandler(this.StudentLogin_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 309);
            this.Controls.Add(this.background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 65);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generic Learning Platform";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.background.ResumeLayout(false);
            this.background.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Ambiance.Ambiance_ThemeContainer background;
        private Ambiance.Ambiance_Button_2 TeacherLogin;
        private Ambiance.Ambiance_Button_1 StudentLogin;
        private Ambiance.Ambiance_Label pass_label;
        private Ambiance.Ambiance_TextBox Password_Text;
        private Ambiance.Ambiance_Label user_label;
        private Ambiance.Ambiance_TextBox Username_Text;
        private Ambiance.Ambiance_Button_2 RegisterButton;
        private Ambiance.Ambiance_Button_1 exit_button;
    }
}

