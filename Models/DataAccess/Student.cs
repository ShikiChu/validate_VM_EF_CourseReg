using System;
using System.Collections.Generic;

namespace CourseReg.Models.DataAccess
{
    public partial class Student
    {
        public Student()
        {
            Courses = new HashSet<Course>();
        }

        public string StudentNum { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
    }
}
