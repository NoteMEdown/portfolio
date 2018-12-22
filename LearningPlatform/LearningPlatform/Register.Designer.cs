namespace LearningPlatform
{
    partial class Register
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
            this.register_form = new Ambiance.Ambiance_ThemeContainer();
            this.cancel_button = new Ambiance.Ambiance_Button_1();
            this.fName_text = new Ambiance.Ambiance_TextBox();
            this.fName_label = new Ambiance.Ambiance_HeaderLabel();
            this.lName_text = new Ambiance.Ambiance_TextBox();
            this.pass_text = new Ambiance.Ambiance_TextBox();
            this.email_text = new Ambiance.Ambiance_TextBox();
            this.lName_label = new Ambiance.Ambiance_HeaderLabel();
            this.pass_label = new Ambiance.Ambiance_HeaderLabel();
            this.email_label = new Ambiance.Ambiance_HeaderLabel();
            this.register_button = new Ambiance.Ambiance_Button_2();
            this.register_form.SuspendLayout();
            this.SuspendLayout();
            // 
            // register_form
            // 
            this.register_form.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.register_form.Controls.Add(this.cancel_button);
            this.register_form.Controls.Add(this.fName_text);
            this.register_form.Controls.Add(this.fName_label);
            this.register_form.Controls.Add(this.lName_text);
            this.register_form.Controls.Add(this.pass_text);
            this.register_form.Controls.Add(this.email_text);
            this.register_form.Controls.Add(this.lName_label);
            this.register_form.Controls.Add(this.pass_label);
            this.register_form.Controls.Add(this.email_label);
            this.register_form.Controls.Add(this.register_button);
            this.register_form.Dock = System.Windows.Forms.DockStyle.Fill;
            this.register_form.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.register_form.Location = new System.Drawing.Point(0, 0);
            this.register_form.Name = "register_form";
            this.register_form.Padding = new System.Windows.Forms.Padding(20, 56, 20, 16);
            this.register_form.RoundCorners = true;
            this.register_form.Sizable = true;
            this.register_form.Size = new System.Drawing.Size(284, 400);
            this.register_form.SmartBounds = true;
            this.register_form.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.register_form.TabIndex = 0;
            this.register_form.Text = "Register";
            this.register_form.Click += new System.EventHandler(this.register_form_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.BackColor = System.Drawing.Color.Transparent;
            this.cancel_button.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cancel_button.Image = null;
            this.cancel_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancel_button.Location = new System.Drawing.Point(154, 340);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(70, 30);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // fName_text
            // 
            this.fName_text.BackColor = System.Drawing.Color.Transparent;
            this.fName_text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.fName_text.ForeColor = System.Drawing.Color.DimGray;
            this.fName_text.Location = new System.Drawing.Point(71, 224);
            this.fName_text.MaxLength = 32767;
            this.fName_text.Multiline = false;
            this.fName_text.Name = "fName_text";
            this.fName_text.ReadOnly = false;
            this.fName_text.Size = new System.Drawing.Size(135, 28);
            this.fName_text.TabIndex = 2;
            this.fName_text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.fName_text.UseSystemPasswordChar = false;
            // 
            // fName_label
            // 
            this.fName_label.AutoSize = true;
            this.fName_label.BackColor = System.Drawing.Color.Transparent;
            this.fName_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.fName_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.fName_label.Location = new System.Drawing.Point(94, 193);
            this.fName_label.Name = "fName_label";
            this.fName_label.Size = new System.Drawing.Size(86, 20);
            this.fName_label.TabIndex = 8;
            this.fName_label.Text = "First Name";
            this.fName_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lName_text
            // 
            this.lName_text.BackColor = System.Drawing.Color.Transparent;
            this.lName_text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.lName_text.ForeColor = System.Drawing.Color.DimGray;
            this.lName_text.Location = new System.Drawing.Point(71, 289);
            this.lName_text.MaxLength = 32767;
            this.lName_text.Multiline = false;
            this.lName_text.Name = "lName_text";
            this.lName_text.ReadOnly = false;
            this.lName_text.Size = new System.Drawing.Size(135, 28);
            this.lName_text.TabIndex = 3;
            this.lName_text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.lName_text.UseSystemPasswordChar = false;
            // 
            // pass_text
            // 
            this.pass_text.BackColor = System.Drawing.Color.Transparent;
            this.pass_text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.pass_text.ForeColor = System.Drawing.Color.DimGray;
            this.pass_text.Location = new System.Drawing.Point(71, 157);
            this.pass_text.MaxLength = 32767;
            this.pass_text.Multiline = false;
            this.pass_text.Name = "pass_text";
            this.pass_text.ReadOnly = false;
            this.pass_text.Size = new System.Drawing.Size(135, 28);
            this.pass_text.TabIndex = 1;
            this.pass_text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.pass_text.UseSystemPasswordChar = false;
            // 
            // email_text
            // 
            this.email_text.BackColor = System.Drawing.Color.Transparent;
            this.email_text.Font = new System.Drawing.Font("Tahoma", 11F);
            this.email_text.ForeColor = System.Drawing.Color.DimGray;
            this.email_text.Location = new System.Drawing.Point(71, 87);
            this.email_text.MaxLength = 32767;
            this.email_text.Multiline = false;
            this.email_text.Name = "email_text";
            this.email_text.ReadOnly = false;
            this.email_text.Size = new System.Drawing.Size(135, 28);
            this.email_text.TabIndex = 0;
            this.email_text.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.email_text.UseSystemPasswordChar = false;
            // 
            // lName_label
            // 
            this.lName_label.AutoSize = true;
            this.lName_label.BackColor = System.Drawing.Color.Transparent;
            this.lName_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lName_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.lName_label.Location = new System.Drawing.Point(94, 259);
            this.lName_label.Name = "lName_label";
            this.lName_label.Size = new System.Drawing.Size(84, 20);
            this.lName_label.TabIndex = 9;
            this.lName_label.Text = "Last Name";
            this.lName_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pass_label
            // 
            this.pass_label.AutoSize = true;
            this.pass_label.BackColor = System.Drawing.Color.Transparent;
            this.pass_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.pass_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.pass_label.Location = new System.Drawing.Point(99, 127);
            this.pass_label.Name = "pass_label";
            this.pass_label.Size = new System.Drawing.Size(76, 20);
            this.pass_label.TabIndex = 7;
            this.pass_label.Text = "Password";
            this.pass_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // email_label
            // 
            this.email_label.AutoSize = true;
            this.email_label.BackColor = System.Drawing.Color.Transparent;
            this.email_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.email_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.email_label.Location = new System.Drawing.Point(115, 56);
            this.email_label.Name = "email_label";
            this.email_label.Size = new System.Drawing.Size(47, 20);
            this.email_label.TabIndex = 6;
            this.email_label.Text = "Email";
            this.email_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.email_label.Click += new System.EventHandler(this.ambiance_HeaderLabel1_Click);
            // 
            // register_button
            // 
            this.register_button.BackColor = System.Drawing.Color.Transparent;
            this.register_button.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.register_button.Image = null;
            this.register_button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.register_button.Location = new System.Drawing.Point(53, 340);
            this.register_button.Name = "register_button";
            this.register_button.Size = new System.Drawing.Size(95, 30);
            this.register_button.TabIndex = 4;
            this.register_button.Text = "Register";
            this.register_button.TextAlignment = System.Drawing.StringAlignment.Center;
            this.register_button.Click += new System.EventHandler(this.register_button_Click);
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 400);
            this.Controls.Add(this.register_form);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 65);
            this.Name = "Register";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Register";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.register_form.ResumeLayout(false);
            this.register_form.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Ambiance.Ambiance_ThemeContainer register_form;
        private Ambiance.Ambiance_Button_2 register_button;
        private Ambiance.Ambiance_HeaderLabel lName_label;
        private Ambiance.Ambiance_HeaderLabel pass_label;
        private Ambiance.Ambiance_HeaderLabel email_label;
        private Ambiance.Ambiance_TextBox lName_text;
        private Ambiance.Ambiance_TextBox pass_text;
        private Ambiance.Ambiance_TextBox email_text;
        private Ambiance.Ambiance_TextBox fName_text;
        private Ambiance.Ambiance_HeaderLabel fName_label;
        private Ambiance.Ambiance_Button_1 cancel_button;
    }
}