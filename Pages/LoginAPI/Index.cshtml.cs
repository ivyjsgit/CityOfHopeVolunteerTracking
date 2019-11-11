using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityOfHopeVolunteerTracking.Pages.LoginAPI
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost()
        {
            Message = "OnPost used";
        }

    }
}
