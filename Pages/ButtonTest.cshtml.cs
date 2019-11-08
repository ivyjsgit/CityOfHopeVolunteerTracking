using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityOfHopeVolunteerTracking.Pages
{
    public class ButtonTestModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = "Get used";
        }
        public void OnPost()
        {
            Message = "Post used";
        }
    }
}