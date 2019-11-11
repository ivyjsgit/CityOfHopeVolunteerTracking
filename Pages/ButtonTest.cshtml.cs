using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityOfHopeVolunteerTracking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityOfHopeVolunteerTracking.Pages
{
    public class ButtonTestModel : PageModel
    {
    public string Message { get; set; }

    private readonly CityOfHopeVolunteerTracking.Data.COHODatabaseContext _context;

        public ButtonTestModel(CityOfHopeVolunteerTracking.Data.COHODatabaseContext context)
        {
            _context = context;
        }

        public Volunteer Volunteer { get; set; }

        public void OnGet()
        {
            Message = "OnGet used";
        }

        public void OnPost()
        {
            Message = "OnPost used";
        }

        public void OnPostView()
        {
            Message = "You called the OnPostView Method";
        }

        public void OnPostRecord(int? id)
        {
            if (id == null)
            {
                Message = "Invalid Record ID";
            }

            Volunteer = _context.Volunteer.FirstOrDefault(m => m.ID == id);

            if (Volunteer == null)
            {
                Message = "Record not Found";
            }
            else
            {
                Message = Volunteer.First + " " + Volunteer.Last;
            }
        }
    }
}