using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPlatform
{
   
    public class Teacher // Depreciated (No longer in use)
    {
        public string email;
        public string password;
        public string firstName;
        public string lastName;

        public Teacher(string e, string p, string f, string l) // When you make a teacher, you can pass all the info to store
        {
            email = e;
            password = p;
            firstName = f;
            lastName = l;
        }
        public Teacher(string info) // Takes all four arguments in 1 string
        {
            string[] allInfo = info.Split('&');
            email = allInfo[0];
            password = allInfo[1];
            firstName = allInfo[2];
            lastName = allInfo[3];
        }
        public string encoded() // stores all data in a single string
        {
            return email + "&" + password + "&" + firstName + "&" + lastName;
        }
    }
}
