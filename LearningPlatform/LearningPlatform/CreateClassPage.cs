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
    public partial class CreateClassPage : Form
    {
        Form1 openPage = null;

        public CreateClassPage()
        {
            InitializeComponent();
        }

        public void SetOpeningPage(Form1 _openPage)
        {
            openPage = _openPage;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            CloseThis();
        }

        private void CreateClassButton_Click(object sender, EventArgs e)
        {
            if(ClassNameText.Text.Length == 0 || ClassIDText.Text.Length == 0)
            {
                MessageBox.Show("You can not leave the Class Name or Class ID empty");
                return;
            }

            if(Database.AddSchoolClass(ClassNameText.Text, ClassIDText.Text))
            {
                MessageBox.Show("Class Successfully Created.");
                CloseThis();
            }
            else
                MessageBox.Show("That Class already exists.");
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
