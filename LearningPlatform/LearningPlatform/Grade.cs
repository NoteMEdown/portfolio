using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPlatform
{
    public class Grade // Currently stores grades as integers, can change to float, must round to 2 decimals if that change is made
    {
        public List<int> grades;
        public Grade()
        {
            grades = new List<int>(); // create empty list
        }
        public Grade(string _grades)
        {
            grades = new List<int>();
            if (_grades.Length == 0)
                return;
            _grades = _grades.Substring(0, _grades.Length - 1);
            string[] tempGrades = _grades.Split('^'); // takes a single string with all grades, turns it into an array of strings
            foreach (string grade in tempGrades) // goes through the array
            {
                try
                {
                    grades.Add(Int32.Parse(grade)); // Turns the string into an int and adds it to the grade List
                }
                catch (Exception) { }
                
            }
        }
        
        public void AddGrade(int _grade) // Adds a grade to list
        {
            grades.Add(_grade);
        }
        public bool AddGrade(string _grade) // Changes grade from string to int, then adds to the list
        {
            try
            {
                grades.Add(Int32.Parse(_grade));
            }
            catch (Exception) { return false; }
            return true;
        }
        public bool ChangeGrade(string _grade, int _index)
        {
            try
            {
                int _gradeInt = Int32.Parse(_grade);
                grades[_index] = _gradeInt;
            }
            catch (Exception) { return false; }
            return true;
        }

        public int[] AllGrades()
        {
            return grades.ToArray(); // returns the list as an array for easy reading and use
        }

        public int GetAverage() // returns a rounded version of the grade average
        {
            if (grades.Count == 0)
                return 100;
            return (int)Math.Round(grades.Average()); // gets average, rounds it, then turns it back into an integer
        }

        public string Encode()
        {
            string gradeString = "";
            foreach (int grade in grades)
            {
                gradeString += grade.ToString() + "^"; // Adds every grade to a string and seperates it by the symbol ^
            }
            return gradeString;
        }
    }
}
