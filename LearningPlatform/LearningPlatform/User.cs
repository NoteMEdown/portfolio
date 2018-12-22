using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPlatform
{

    public class User // Replaced Student and Teacher class
    {
        public string username; // can be either an student ID or a teacher email
        public string password;
        public string firstName;
        public string lastName;
        public string classes;
        public bool isTeacher; // True or False value based on if the user is a teacher or not

        public User(string u, string p, string f, string l, string c, bool t) // When you make a student, you can pass all the info to store
        {
            username = u;
            password = p;
            firstName = f;
            lastName = l;
            classes = c;
            isTeacher = t;
        }
        public User(string info) // Takes all four arguments in 1 string
        {
            string[] allInfo = info.Split('&'); // splits the string and stores it in an array
            username = allInfo[0];
            password = allInfo[1];
            firstName = allInfo[2];
            lastName = allInfo[3];
            classes = allInfo[4];
            isTeacher = allInfo[5].Equals("True"); // Turns the string "True" or the string "False" into the actual bool value
        }
        public void AddClass(string _className)
        {
            classes += _className + "@";
        }

        public float CalculateGPA()
        {
            if (isTeacher || classes.Length == 0)
                return -1; // return -1 if trying to calculate the gpa of a teacher
            string[] allClasses = classes.Substring(0, classes.Length - 1).Split('@');
            int averagePerClass = 0;
            float totalGpa = 0;
            SchoolClass tempClass = null;
            float classCount = allClasses.Length;
            foreach (string className in allClasses)
            {
                tempClass = Database.classes[className];
                if (tempClass.grades.Count > 0)
                {
                    averagePerClass = tempClass.grades[this].GetAverage();
                    if (averagePerClass > 89)
                        totalGpa += 4.0f;
                    else if (averagePerClass > 79)
                        totalGpa += 3.0f;
                    else if (averagePerClass > 69)
                        totalGpa += 2.0f;
                    else if (averagePerClass > 59)
                        totalGpa += 1.0f;
                }
                else
                    classCount--;
            }
            return totalGpa / classCount;
        }
        public string Encoded() // stores all data in a single string
        {
            return username + "&" + password + "&" + firstName + "&" + lastName + "&" + classes + "&" + isTeacher.ToString();
        }
    }
}