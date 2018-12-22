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
    public partial class CreateStudentForm : Form
    {
        Form1 openPage = null;

        public CreateStudentForm()
        {
            InitializeComponent();
            foreach (SchoolClass currentClass in Database.currentClasses)
            {
                ClassComboBox.Items.Add(currentClass.className);
            }
        }

        public void SetOpeningPage(Form1 _openPage)
        {
            openPage = _openPage;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CloseThis();
        }

        private void AddStudentButton_Click(object sender, EventArgs e)
        {
            #region debug
         //   MessageBox.Show("Selected class Index: " + ClassComboBox.SelectedIndex);
            #endregion
            if (StudentIDText.Text.Length == 0)
            {
                MessageBox.Show("Student ID can not be empty!");
                return;
            }
            if (ClassComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a class to add the student to.");
                return;
            }
            if(!Database.users.ContainsKey(StudentIDText.Text) && (FirstNameText.Text.Length == 0 ||
                LastNameText.Text.Length == 0 || StudentPassText.Text.Length == 0))
            {
                MessageBox.Show("Student does not exist, must provide name and password!");
                return;
            }
            if (Database.AddStudent(FirstNameText.Text, LastNameText.Text, StudentIDText.Text, StudentPassText.Text,
                 Database.currentClasses[ClassComboBox.SelectedIndex]))
            {
                MessageBox.Show("Student Successfully added to the class.");

                CloseThis();
            }
            else
                MessageBox.Show("That student already belongs to that class.");
            
        }

        private void CloseThis()
        {
            MainPage main = new MainPage();
            main.Show();
            main.SetOpeningPage(openPage);
            this.Close();
        }
    }
}
