using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ambiance;

namespace LearningPlatform
{
    public partial class MainPage : Form
    {

        List<TabPage> allTabs; // Stores all tabs
        Form1 openPage = null;

        public MainPage() // Constructor function, gets called when the user logins successfully
        {
            InitializeComponent(); // Makes the form
            allTabs = new List<TabPage>(); // Creates empty list
            Database.SetCurrentClasses(); // Sets up the current classes the student or teacher is in
            UpdateUI(); // Make the UI unique for each student or teacher
        }

        public void SetOpeningPage(Form1 _openPage)
        {
            openPage = _openPage;
        }

        public void UpdateUI()
        {
            ChangeForTeacher();
            AddTabs();
        }

        public void ChangeForTeacher()
        {
            if (Database.isTeacher)
            {
                createClassButton.Enabled = true;
                createClassButton.Visible = true;
                addStudentButton.Enabled = true;
                addStudentButton.Visible = true;
            }
            else
            {
                createClassButton.Enabled = false;
                createClassButton.Visible = false;
                addStudentButton.Enabled = false;
                addStudentButton.Visible = false;
            }
        }

        public void AddTabs()
        {
            int count = 0;

            #region testing
            //SchoolClass tempClass = new SchoolClass("CS_2410", "930873", Database.currentUser);
            //Database.currentClasses.Add(tempClass);
            //Database.classes.Add(tempClass.classID, tempClass);

            //tempClass = new SchoolClass("MATH_3520", "171120", Database.currentUser);
            //Database.currentClasses.Add(tempClass);
            //Database.classes.Add(tempClass.classID, tempClass);

            //for (int i = 0; i < 5; i++)
            //{
            //    Database.currentClasses[0].students.Add(new User("none", "none", "First"+i, "Last"+i, "", false));
            //    Database.currentClasses[1].students.Add(new User("none", "none", "First2" + i, "Last2" + i, "", false));
            //}
            #endregion

            foreach (SchoolClass classTab in Database.currentClasses) // WIP: Adding tabs equal to number of classes
            {
                AddTab(classTab, count); // Add tab, only info needed is a SchoolClass and the index
                count++; // Increase index after every tab
            }
        }


        public void AddTab(SchoolClass classTab, int index) // This method adds a tab to the main page based on the info of a SchoolClass
        {
            TabPage tab = new System.Windows.Forms.TabPage // Create a new tab with className
            {
                Text = classTab.className,
                Name = classTab.classID,
                Size = new System.Drawing.Size(256, 214),
                TabIndex = index
            };

            allTabs.Add(tab); // Store all tabs in a list
            classTabs.Controls.Add(tab); // Add tab to display
            Ambiance_HeaderLabel teacherLabel = new Ambiance_HeaderLabel
            {
                AutoSize = true,
                Location = new System.Drawing.Point(16, 7),
                Name = "teacherLabel",
             //   Font = new Font("Segoe UI", 12.0f, FontStyle.Bold),
                Size = new System.Drawing.Size(43, 90),
                Text = ("Teacher : " + Database.currentClasses[index].teacher.firstName + " " + Database.currentClasses[index].teacher.lastName)
            };
            Ambiance_Label classIDLabel = new Ambiance_Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(16, 27),
                Name = "classIDLabel",
            //    Font = new Font("Segoe UI", 12.0f, FontStyle.Regular),
                Size = new System.Drawing.Size(43, 13),
                Text = ("Class ID  : " + Database.currentClasses[index].classID)
            };
            
            tab.Controls.Add(teacherLabel);
            tab.Controls.Add(classIDLabel);

            if (Database.isTeacher) // If user is a teacher, add these elements to tabs
            {
                Ambiance_ComboBox studentBox = new Ambiance_ComboBox
                {
                    Location = new System.Drawing.Point(700, 10),
                    Name = "studentBox",
                    Size = new System.Drawing.Size(204, 26)
                };
                Ambiance_HeaderLabel studentLabel = new Ambiance_HeaderLabel
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(565, 13),
                    Name = "studentLabel",
                    //    Font = new Font("Segoe UI", 12.0f, FontStyle.Regular),
                    Size = new System.Drawing.Size(129, 20),
                    Text = ("Current Student :")
                };
                Ambiance_HeaderLabel selectStudentLabel = new Ambiance_HeaderLabel
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(72, 180),
                    Name = "selectStudentLabel",
                    Font = new Font("Segoe UI", 32.0f, FontStyle.Bold),
                    Size = new System.Drawing.Size(773, 59),
                    Text = ("Select a student to view their grades.")
                };
                foreach (User userInClass in classTab.students)
                {
                    studentBox.Items.Add(userInClass.firstName + " " + userInClass.lastName);
                }
                studentBox.SelectedIndexChanged += new EventHandler(DynamicComboBox_OnChange);

                tab.Controls.Add(studentLabel);
                tab.Controls.Add(studentBox);
                tab.Controls.Add(selectStudentLabel);
            }
            else // If the user is a student, add these elements
            {
                ShowInfo(tab, classTab, Database.currentUser);
            }
            
        }

        private void DynamicComboBox_OnChange(object sender, EventArgs e) // Handles when the student combobox is changed
        {
            TabPage tab = (TabPage)((ComboBox)sender).Parent; // Gets the current Tab that we are on
            SchoolClass currentClass = Database.classes[((ComboBox)sender).Parent.Name]; // Gets the current class we are on
            User currentUser = currentClass.students[((ComboBox)sender).SelectedIndex]; // Gets the user that was selected
            ShowInfo(tab, currentClass, currentUser);
        }

        private void DynamicAddGrade_Click(object sender, EventArgs e)
        {
            TabPage tab = (TabPage)((Ambiance_Button_1)sender).Parent;
            SchoolClass currentClass = Database.classes[tab.Name];
            User currentUser = currentClass.students[((ComboBox)tab.Controls.Find("studentBox", false)[0]).SelectedIndex];
            Ambiance_Panel currentPanel = (Ambiance_Panel)tab.Controls.Find("studentGradesPanel", false)[0];
            AddGrade(tab, currentClass, currentUser, currentPanel);
        }

        private void DynamicChangeGrade_Click(object sender, EventArgs e)
        {
            TabPage tab = (TabPage)((Ambiance_Button_2)sender).Parent;
            SchoolClass currentClass = Database.classes[tab.Name];
            User currentUser = currentClass.students[((ComboBox)tab.Controls.Find("studentBox", false)[0]).SelectedIndex];
            Ambiance_Panel currentPanel = (Ambiance_Panel)tab.Controls.Find("studentGradesPanel", false)[0];
            ChangeGrade(tab, currentClass, currentUser, currentPanel);
        }

        private void ShowInfo(TabPage tab, SchoolClass currentClass, User currentUser) // Shows grades for particular student
        {
            if(tab.Controls.ContainsKey("selectStudentLabel")) // If tab has selectStudentLabel...
                tab.Controls.Remove(tab.Controls.Find("selectStudentLabel", false)[0]); // Delete it
            if (tab.Controls.ContainsKey("studentNameLabel")) // If the tab has studentNameLabel
                tab.Controls.Find("studentNameLabel", false)[0].Text = "Student Name : " + currentUser.firstName + " " + currentUser.lastName; // Change text
            else // otherwise..
            {
                Ambiance_Label studentNameLabel = new Ambiance_Label // Create it
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(326, 16),
                    Name = "studentNameLabel",
                    Size = new System.Drawing.Size(185, 20),
                    Text = ("Student Name : " + currentUser.firstName + " " + currentUser.lastName)
                };
                tab.Controls.Add(studentNameLabel);
            }

            if (tab.Controls.ContainsKey("studentIDLabel")) // If the tab has studentIDLabel
                tab.Controls.Find("studentIDLabel", false)[0].Text = "Student ID : " + currentUser.username; // Change text
            else // otherwise...
            {
                Ambiance_Label studentIDLabel = new Ambiance_Label // Create it
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(351, 56),
                    Name = "studentIDLabel",
                    Size = new System.Drawing.Size(138, 20),
                    Text = ("Student ID : " + currentUser.username)
                };
                tab.Controls.Add(studentIDLabel);
            }

            if (tab.Controls.ContainsKey("studentGPALabel")) // If the tab has studentGPALabel
                tab.Controls.Find("studentGPALabel", false)[0].Text = "GPA : " + String.Format("{0:0.00}", currentUser.CalculateGPA()); // Change text
            else // otherwise...
            {
                Ambiance_Label studentGPALabel = new Ambiance_Label // Create it
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(395, 95),
                    Name = "studentGPALabel",
                    Size = new System.Drawing.Size(74, 20),
                    Text = ("GPA : " + String.Format("{0:0.00}", currentUser.CalculateGPA()))
                };
                tab.Controls.Add(studentGPALabel);
            }

            if(!tab.Controls.ContainsKey("gradeButton") && Database.isTeacher)  // If the tab has studentGPALabel
            {
                Ambiance_Button_1 gradeButton = new Ambiance_Button_1 // Create it
                {
                    Location = new System.Drawing.Point(566, 307),
                    Name = "gradeButton",
                    Size = new System.Drawing.Size(139, 30),
                    Text = ("Add Test Score")
                };
                Ambiance_Button_2 gradeButton1 = new Ambiance_Button_2 // Create it
                {
                    Location = new System.Drawing.Point(566, 346),
                    Name = "gradeButton1",
                    Size = new System.Drawing.Size(139, 30),
                    Text = ("Commit Changes")
                };
                gradeButton.Click += new EventHandler(DynamicAddGrade_Click);
                gradeButton1.Click += new EventHandler(DynamicChangeGrade_Click);
                tab.Controls.Add(gradeButton);
                tab.Controls.Add(gradeButton1);
            }

            if (tab.Controls.ContainsKey("studentGradesPanel")) // If the tab has studentGradesPanel
                tab.Controls.Remove(tab.Controls.Find("studentGradesPanel", false)[0]);// Remove it

            Ambiance_Panel studentGradesPanel = new Ambiance_Panel // Make a new one
            {
                Location = new System.Drawing.Point(721, 56),
                Name = "studentGradesPanel",
                Size = new System.Drawing.Size(172, 320),
                AutoScroll = true
            };
            studentGradesPanel.VerticalScroll.Visible = true;
            
            List<int> currentGrades = currentClass.grades[currentUser].grades;
            #region testing
            //currentGrades.Add(12);
            //currentGrades.Add(55);
            //currentGrades.Add(99);
            //currentGrades.Add(12);
            //currentGrades.Add(55);
            //currentGrades.Add(99);
            //currentGrades.Add(12);
            //currentGrades.Add(55);
            //currentGrades.Add(99);
            #endregion
            for (int i = 0; i < currentGrades.Count; i++)
            {
                studentGradesPanel.Controls.Add(new Ambiance_Label
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(12, 48 * i + 12),
                    Name = "label" + i,
                    Size = new System.Drawing.Size(54, 20),
                    Text = ("Test " + i + " : ")
                });
                studentGradesPanel.Controls.Add(new Ambiance_Separator
                {
                    Location = new System.Drawing.Point(4, 48 * i + 42),
                    Name = "separator" + i,
                    Size = new System.Drawing.Size(139, 10)
                });
                if (Database.currentUser.isTeacher)
                {
                    studentGradesPanel.Controls.Add(new Ambiance_TextBox
                    {
                        Location = new System.Drawing.Point(73, 48 * i + 9),
                        Name = "grade",
                        Size = new System.Drawing.Size(69, 28),
                        Text = (currentGrades[i].ToString()),
                        MaxLength = 3,
                        TextAlignment = HorizontalAlignment.Center
                    });
                }
                else
                {
                    studentGradesPanel.Controls.Add(new Ambiance_Label
                    {
                        Location = new System.Drawing.Point(73, 48 * i + 12),
                        Name = "grade",
                        Size = new System.Drawing.Size(69, 28),
                        Text = (currentGrades[i].ToString())
                    });
                }
            }
            tab.Controls.Add(studentGradesPanel);
        }
        
        private void AddGrade(TabPage tab, SchoolClass currentClass, User currentUser, Ambiance_Panel currentPanel)
        {
            List<int> currentGrades = currentClass.grades[currentUser].grades;
            int i = currentGrades.Count;
            currentGrades.Add(0);
            currentPanel.Controls.Add(new Ambiance_Label
            {
                AutoSize = true,
                Location = new System.Drawing.Point(12, 48 * i + 12),
                Name = "label" + i,
                Size = new System.Drawing.Size(54, 20),
                Text = ("Test " + i + " : ")
            });
            currentPanel.Controls.Add(new Ambiance_Separator
            {
                Location = new System.Drawing.Point(4, 48 * i + 42),
                Name = "separator" + i,
                Size = new System.Drawing.Size(139, 10)
            });
            currentPanel.Controls.Add(new Ambiance_TextBox
            {
                Location = new System.Drawing.Point(73, 48 * i + 9),
                Name = "grade",
                Size = new System.Drawing.Size(69, 28),
                Text = ("0"),
                MaxLength = 3,
                TextAlignment = HorizontalAlignment.Center
            });
        }

        private void ChangeGrade(TabPage tab, SchoolClass currentClass, User currentUser, Ambiance_Panel currentPanel)
        {
            List<int> currentGrades = currentClass.grades[currentUser].grades;
            bool errorShown = false;
            Control[] gradeBoxs = currentPanel.Controls.Find("grade", false);
            for (int i = 0; i < currentGrades.Count; i++)
            {
                try
                {
                    if (Int32.Parse(gradeBoxs[i].Text) <= 100 && Int32.Parse(((Ambiance_TextBox)gradeBoxs[i]).Text) >= 0)
                        currentGrades[i] = Int32.Parse(((Ambiance_TextBox)gradeBoxs[i]).Text);
                    else
                        if (!errorShown)
                    {
                        MessageBox.Show("Grades must be 0-100");
                        errorShown = false;
                    }
                }
                catch (Exception)
                {
                    if (!errorShown)
                    {
                        MessageBox.Show("Grades can be numbers only!");
                        errorShown = true;
                    }
                }
            }
            if (!errorShown)
            {
                MessageBox.Show("Grades Successfully Updated");
            }
            Database.SaveClass(currentClass);
            tab.Controls.Find("studentGPALabel", false)[0].Text = "GPA : " + String.Format("{0:0.00}", currentUser.CalculateGPA());
        }

        private void createClassButton_Click(object sender, EventArgs e)
        {
            CreateClassPage ccp = new CreateClassPage();
            ccp.Show();
            ccp.SetOpeningPage(openPage);
            this.Close();
        }

        private void addStudentButton_Click(object sender, EventArgs e)
        {
            CreateStudentForm csf = new CreateStudentForm();
            csf.Show();
            csf.SetOpeningPage(openPage);
            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exits when exit button is clicked
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            openPage.Show();
            this.Close();
        }
    }   
}
