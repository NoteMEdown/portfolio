namespace LearningPlatform
{
    partial class CreateClassPage
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
            this.CreateClassForm = new Ambiance.Ambiance_ThemeContainer();
            this.ClassIDLabel = new Ambiance.Ambiance_HeaderLabel();
            this.ClassIDText = new Ambiance.Ambiance_TextBox();
            this.ClassNameLabel = new Ambiance.Ambiance_HeaderLabel();
            this.ClassNameText = new Ambiance.Ambiance_TextBox();
            this.CreateClassButton = new Ambiance.Ambiance_Button_2();
            this.cancelButton = new Ambiance.Ambiance_Button_1();
            this.CreateClassForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateClassForm
            // 
            this.CreateClassForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.CreateClassForm.Controls.Add(this.ClassIDLabel);
            this.CreateClassForm.Controls.Add(this.ClassIDText);
            this.CreateClassForm.Controls.Add(this.ClassNameLabel);
            this.CreateClassForm.Controls.Add(this.ClassNameText);
            this.CreateClassForm.Controls.Add(this.CreateClassButton);
            this.CreateClassForm.Controls.Add(this.cancelButton);
            this.CreateClassForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreateClassForm.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CreateClassForm.Location = new System.Drawing.Point(0, 0);
            this.CreateClassForm.Name = "CreateClassForm";
            this.CreateClassForm.Padding = new System.Windows.Forms.Padding(20, 56, 20, 16);
            this.CreateClassForm.RoundCorners = true;
            this.CreateClassForm.Sizable = true;
            this.CreateClassForm.Size = new System.Drawing.Size(327, 230);
            this.CreateClassForm.SmartBounds = true;
            this.CreateClassForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.CreateClassForm.TabIndex = 0;
            this.CreateClassForm.Text = "Create Class";
            // 
            // ClassIDLabel
            // 
            this.ClassIDLabel.AutoSize = true;
            this.ClassIDLabel.BackColor = System.Drawing.Color.Transparent;
            this.ClassIDLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ClassIDLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.ClassIDLabel.Location = new System.Drawing.Point(27, 130);
            this.ClassIDLabel.Name = "ClassIDLabel";
            this.ClassIDLabel.Size = new System.Drawing.Size(68, 20);
            this.ClassIDLabel.TabIndex = 6;
            this.ClassIDLabel.Text = "Class ID:";
            // 
            // ClassIDText
            // 
            this.ClassIDText.BackColor = System.Drawing.Color.Transparent;
            this.ClassIDText.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ClassIDText.ForeColor = System.Drawing.Color.DimGray;
            this.ClassIDText.Location = new System.Drawing.Point(99, 126);
            this.ClassIDText.MaxLength = 32767;
            this.ClassIDText.Multiline = false;
            this.ClassIDText.Name = "ClassIDText";
            this.ClassIDText.ReadOnly = false;
            this.ClassIDText.Size = new System.Drawing.Size(201, 28);
            this.ClassIDText.TabIndex = 5;
            this.ClassIDText.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.ClassIDText.UseSystemPasswordChar = false;
            // 
            // ClassNameLabel
            // 
            this.ClassNameLabel.AutoSize = true;
            this.ClassNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.ClassNameLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.ClassNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(77)))));
            this.ClassNameLabel.Location = new System.Drawing.Point(28, 79);
            this.ClassNameLabel.Name = "ClassNameLabel";
            this.ClassNameLabel.Size = new System.Drawing.Size(94, 20);
            this.ClassNameLabel.TabIndex = 4;
            this.ClassNameLabel.Text = "Class Name:";
            // 
            // ClassNameText
            // 
            this.ClassNameText.BackColor = System.Drawing.Color.Transparent;
            this.ClassNameText.Font = new System.Drawing.Font("Tahoma", 11F);
            this.ClassNameText.ForeColor = System.Drawing.Color.DimGray;
            this.ClassNameText.Location = new System.Drawing.Point(123, 75);
            this.ClassNameText.MaxLength = 32767;
            this.ClassNameText.Multiline = false;
            this.ClassNameText.Name = "ClassNameText";
            this.ClassNameText.ReadOnly = false;
            this.ClassNameText.Size = new System.Drawing.Size(177, 28);
            this.ClassNameText.TabIndex = 2;
            this.ClassNameText.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.ClassNameText.UseSystemPasswordChar = false;
            // 
            // CreateClassButton
            // 
            this.CreateClassButton.BackColor = System.Drawing.Color.Transparent;
            this.CreateClassButton.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.CreateClassButton.Image = null;
            this.CreateClassButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CreateClassButton.Location = new System.Drawing.Point(32, 175);
            this.CreateClassButton.Name = "CreateClassButton";
            this.CreateClassButton.Size = new System.Drawing.Size(126, 30);
            this.CreateClassButton.TabIndex = 1;
            this.CreateClassButton.Text = "Create Class";
            this.CreateClassButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.CreateClassButton.Click += new System.EventHandler(this.CreateClassButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cancelButton.Image = null;
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.Location = new System.Drawing.Point(183, 175);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(117, 30);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.TextAlignment = System.Drawing.StringAlignment.Center;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // CreateClassPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 230);
            this.Controls.Add(this.CreateClassForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 65);
            this.Name = "CreateClassPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Class";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.CreateClassForm.ResumeLayout(false);
            this.CreateClassForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Ambiance.Ambiance_ThemeContainer CreateClassForm;
        private Ambiance.Ambiance_Button_2 CreateClassButton;
        private Ambiance.Ambiance_Button_1 cancelButton;
        private Ambiance.Ambiance_HeaderLabel ClassIDLabel;
        private Ambiance.Ambiance_TextBox ClassIDText;
        private Ambiance.Ambiance_HeaderLabel ClassNameLabel;
        private Ambiance.Ambiance_TextBox ClassNameText;
    }
}