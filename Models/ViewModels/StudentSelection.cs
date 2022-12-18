using System;
using System.Collections.Generic;
using CourseReg.Models.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace CourseReg.Models.ViewModels
{
    public class StudentSelection
    {
        [Display(Name = "Select")]
        public bool Selected { get; set; } = false;

        [Display(Name = "Student")]
        public Student TheStudent { get; set; }

        public StudentSelection() { }
        public StudentSelection(Student theStudent, bool selected = false)
        {
            Selected = selected;
            TheStudent = theStudent;
        }
    }
}
