using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPlatform
{

    public class Student // Depreciated (No longer in use)
    {
        public string iD;
        public string password;
        public string firstName;
        public string lastName;
        public string classes;

        public Student(string i, string p, string f, string l, string c) // When you make a student, you can pass all the info to store
        {
            iD = i;
            password = p;
            firstName = f;
            lastName = l;
            classes = c;
        }
        public Student(string info) // Takes all four arguments in 1 string
        {
            string[] allInfo = info.Split('&'); // splits the string and stores it in an array
            iD = allInfo[0];
            password = allInfo[1];
            firstName = allInfo[2];
            lastName = allInfo[3];
            classes = allInfo[4];
        }
        public string encoded() // stores all data in a single string
        {
            return iD + "&" + password + "&" + firstName + "&" + lastName;
        }
    }
}