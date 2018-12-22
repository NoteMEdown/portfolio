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
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            Database.CreateFiles();
        }

        private void ambiance_Button_21_Click(object sender, EventArgs e) // Teacher Login Button
        {
            if(Database.CheckTeacherLogin(Username_Text.Text, Password_Text.Text)) // If teachers email and password are correct, login
            {
                Database.SetTeacher(Username_Text.Text);
                login();
            }
            else
                System.Windows.Forms.MessageBox.Show("Incorrect email or password!");
        }

        private void ambiance_ThemeContainer1_Click(object sender, EventArgs e)
        {
            // Put Nothing here
        }

        private void StudentLogin_Click(object sender, EventArgs e)
        {
            if (Database.CheckStudentLogin(Username_Text.Text, Password_Text.Text)) // If students id and password are correct, login
            {
                Database.SetStudent(Username_Text.Text);
                login();
            }
            else
                System.Windows.Forms.MessageBox.Show("Incorrect ID or password!");
        }

        

        private void exit_button_Click_1(object sender, EventArgs e) // exit when exit button is clicked
        {
            Application.Exit();
        }

        private void RegisterButton_Click(object sender, EventArgs e) // Shows register form
        {
            Register reg = new Register(this);
            this.Hide();
            reg.Show();
        }

        private void login() // Switches form to the main page form
        {
            MainPage main = new MainPage();
            this.Hide();
            main.SetOpeningPage(this);
            main.Show();
        }
    }
}
