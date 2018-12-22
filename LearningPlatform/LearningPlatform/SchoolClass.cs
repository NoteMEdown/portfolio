using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPlatform
{
    public class SchoolClass
    {
        public string className;
        public string classID;
        public User teacher; // stores the teacher
        public List<User> students; // Stores all students
        public Dictionary<User, Grade> grades; // lets you find a grade based on a user (REMINDER: Database class is storing current user!)

        public SchoolClass(string _className, string _classID, User _teacher) // When making a class, provide className, classID, and the Teacher
        {
            className = _className;
            classID = _classID;
            teacher = _teacher;
            students = new List<User>();
            grades = new Dictionary<User, Grade>();
        }

        public SchoolClass(string info) // This constructor should be used for loading a SchoolClass
        {
            // Info example "CS-3321&107298&jones@gmail.com&993251!60^45^99^43^88#993251!60^45^99^43^88"
            
            // Parse the class names.
            string[] allInfo = info.Split('&');
            className = allInfo[0];
            classID = allInfo[1];
            teacher = Database.users[allInfo[2]];
            
            // all students in allInfo[3]
            students = new List<User>();
            string[] studentInfo = allInfo[3].Split('#');
            string[] idAndGrades = null;
            User tempUser = null;
            grades = new Dictionary<User, Grade>();
            // Parse the students.
            foreach (string student in studentInfo)
            {
                try
                {
                    if (!student.Contains('!'))
                        return;
                    idAndGrades = student.Split('!');
                    tempUser = Database.users[idAndGrades[0]];
                    students.Add(tempUser);
                    grades.Add(tempUser, new Grade(idAndGrades[1]));
                }
                catch (Exception) { }
            }
        }

        public int[] getGrades(User _user) // returns an array of intergers that represent a user's grades
        {
            if(grades.ContainsKey(_user))
                return grades[_user].AllGrades();
            return new int[0]; // returns empty array if user was not found
        }

        public void AddStudent(User _user)
        {
            students.Add(_user);
            grades.Add(_user, new Grade());
        }
        public bool AddGrade(User _user, string _grade)
        {
            if (grades[_user].AddGrade(_grade))
                return true;
            return false;
        }
        public bool ChangeGrade(User _user, string _grade, int _index)
        {
            if (grades[_user].ChangeGrade(_grade, _index))
                return true;
            return false;
        }

        public string Encoded() 
        {
            string allText = "";

            allText += className + "&" + classID + "&" + teacher.username + "&";

            foreach (KeyValuePair<User, Grade> student in grades)
            {
                allText += student.Key.username + "!";
                foreach (int grade in student.Value.grades)
                {
                    allText += grade + "^";
                }
                allText += "#";
            }
            return allText;
        }
    }
}
