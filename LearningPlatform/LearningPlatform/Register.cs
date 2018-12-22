using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LearningPlatform
{
    public partial class Register : Form
    {

        Form1 open;

        public Register(Form1 _open)
        {
            InitializeComponent();
            open = _open;
        }

        private void register_form_Click(object sender, EventArgs e)
        {

        }

        private void ambiance_HeaderLabel1_Click(object sender, EventArgs e)
        {

        }

        private void register_button_Click(object sender, EventArgs e) // Checks if eerything is OKAY. Registers the teacher if so
        {
            if (email_text.Text.Length < 1 || pass_text.Text.Length < 1 || fName_text.Text.Length < 1 || lName_text.Text.Length < 1)
            {
                System.Windows.Forms.MessageBox.Show("Please don't leave anything blank.");
            }
            else
            {
                if ((email_text.Text + pass_text.Text + fName_text.Text + lName_text.Text).Contains("&") ||
                                (email_text.Text + pass_text.Text + fName_text.Text + lName_text.Text).Contains("|"))
                {
                    System.Windows.Forms.MessageBox.Show("You can not use the symbols & or |");
                }
                else
                {
                    if (!Database.AddTeacher(email_text.Text, pass_text.Text, fName_text.Text, lName_text.Text))
                        System.Windows.Forms.MessageBox.Show("That email is already taken!\nTry logging in instead?");
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Registered Successfully!");
                        leave();
                    }
                }
            }
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            leave();
        }

        private void leave() // Goes back to the main form
        {
            this.Hide();
            open.Show();
        }
    }
}
