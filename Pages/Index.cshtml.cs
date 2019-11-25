using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoHO.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; }

        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public VolunteerActivity VolunteerActivity { get; set; }

        public Volunteer Volunteers { get; set; }


        public void OnPostClockin()
        {
            if (Volunteers.VolunteerID == null)
            {
                //
                //User does not exist (alert?)
            }
            else if (Request.Form["userid"] == Volunteers.VolunteerID)
            {
                if (Volunteers.ClockedIn == false)
                {
                    Volunteers.ClockedIn = true;
                    VolunteerActivity.ClockedIn = true;
                    VolunteerActivity.StartTime = DateTime.Now;
                }
                else if (Volunteers.ClockedIn) {
                    Volunteers.ClockedIn = false;
                    VolunteerActivity.ClockedIn = false;
                    VolunteerActivity.EndTime = DateTime.Now;
                }
                   

            }
        }

        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}

        public void OnGet()
        {

        }
    }
}