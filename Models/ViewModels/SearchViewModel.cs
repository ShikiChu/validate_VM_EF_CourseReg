using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CourseReg.Models.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage="You must enter at least 2 characters") ]
        [Display(Name = "Title Contains:")]
        public string? SearchString { get; set; }

        public SearchViewModel()
        {

        }

        public SearchViewModel(string searchString)
        {
            SearchString = searchString;
        }
    }
}
