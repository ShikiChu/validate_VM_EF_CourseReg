using CourseReg.Models.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CourseReg.Models.ViewModels
{
    public class CourseViewModel
    {
        [Display(Name = "Courses")]
        public Course TheCourse { get; set; }

        [Display(Name = "Students")]
        public List<StudentSelection> StudentSelections { get; set; }

        public CourseViewModel() {
            TheCourse = new Course();
            StudentSelections = new List<StudentSelection>();
        }

        public CourseViewModel(Course course, List<StudentSelection> studentSelections)
        {
            TheCourse = course ?? throw new ArgumentNullException(nameof(course));
            StudentSelections = studentSelections ?? throw new ArgumentNullException(nameof(studentSelections));

            foreach(StudentSelection studentSelection in StudentSelections)
            {
                if (TheCourse.Students.Contains(studentSelection.TheStudent))
                {
                    studentSelection.Selected = true;
                }

            }
        }
    }
}
