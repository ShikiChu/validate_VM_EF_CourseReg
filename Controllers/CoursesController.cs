using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseReg.Models.DataAccess;
using CourseReg.Models.ViewModels;
using System.Data;

namespace CourseReg.Controllers
{
    public class CoursesController : Controller
    {
        private readonly RegistrationDBContext _context;

        public CoursesController(RegistrationDBContext context)
        {
            _context = context;
        }

        // GET: Courses
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel searchViewModel) // bind with the model in Views, here ViewModels as the obj, bind with asp-for  
        {
            if (ModelState.IsValid) 
            {
                string searchString = searchViewModel.SearchString;
                List<Course> searchResults = _context.Courses.Include(c=>c.Students).Where(s =>
                s.CourseTitle.Contains(searchString)).ToList();
                if (searchResults.Count == 0)
                {
                    ModelState.AddModelError("SearchString", "No course found with specified search characters");
                }
                else
                {
                    ViewData["searchString"] = searchString; // store into ViewData, which will show in the index page
                    return View("Index", searchResults);  // return to the view, Index page and with the model obj.
                                                          // work same as in get Index(), return View( _context.Customers.Where(s =>s.FirstName.Contains(searchString) || s.LastName.Contains(searchString)).ToList();)
                }
            }
            return View(searchViewModel);   
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.Include(c=>c.Students).ToListAsync());
        }

        public async Task<IActionResult> EditRegistrations(string id) 
        {
            var selectCourse = await _context.Courses.Include(c=>c.Students).SingleOrDefaultAsync(c => c.CourseId.Equals(id));
            var allStudent = _context.Students.Include(s=>s.Courses).ToList();

            List<StudentSelection> studentSelectionsList = new List<StudentSelection>();
            foreach (Student theStudent in allStudent)
            {
                bool selected = false;
                StudentSelection studentSelection = new StudentSelection(theStudent, selected);
                studentSelectionsList.Add(studentSelection);
            }
            CourseViewModel courseView = new CourseViewModel(selectCourse, studentSelectionsList);

            return View(courseView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRegistrations (CourseViewModel courseViewModel) 
        {
            //max hour of course:
            //var maxReg = courseViewModel.TheCourse.MaxRegistrations;
            // record max hour :
            var maxReg = _context.Courses.SingleOrDefault(c => c.CourseId == courseViewModel.TheCourse.CourseId);
            int count = 0;
            foreach (var student in courseViewModel.StudentSelections)
            {
                if (student.Selected)
                {
                    count++;
                }
            }
            if (count > maxReg.MaxRegistrations) 
            {
                ModelState.AddModelError("StudentSelections","Your selection exceeds the course's max registration");
            }
            
            

            if (ModelState.IsValid) 
            {
                Course selectedcourse = await _context.Courses.Include(c => c.Students).SingleOrDefaultAsync(c => c.CourseId == courseViewModel.TheCourse.CourseId);
                selectedcourse.Students.Clear(); // clear all old records
                foreach (StudentSelection studentSelection in courseViewModel.StudentSelections)
                {
                    if (studentSelection.Selected)
                    {
                        var choice = _context.Students.SingleOrDefault(s => s.StudentNum == studentSelection.TheStudent.StudentNum);
                        selectedcourse.Students.Add(choice);
                    }
                }
                _context.Update(selectedcourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(courseViewModel);
        }


    }
}
