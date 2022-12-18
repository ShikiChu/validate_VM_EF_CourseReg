using System;
using System.Collections.Generic;

namespace CourseReg.Models.DataAccess
{
    public partial class Course
    {
        public Course()
        {
            Students = new HashSet<Student>();
        }

        public string CourseId { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string? Description { get; set; }
        public int? HoursPerWeek { get; set; }
        public decimal? FeeBase { get; set; }
        public int? MaxRegistrations { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
