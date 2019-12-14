using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using CoHO.Models;

namespace CoHO.Pages.VolunteerActivities
{
    public class IndexDetailModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexDetailModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VolunteerActivity> VolunteerActivity { get; set; }

        public async Task OnGetAsync()
        {
            VolunteerActivity = await _context.VolunteerActivity
                .OrderByDescending(v => v.StartTime)
                .Include(v => v.Volunteer)
                .Include(v => v.Volunteer.Race)
                .Include(v => v.Volunteer.EducationLevel)
                .Include(v => v.Volunteer.VolunteerType)
                .Include(v => v.Initiative).ToListAsync();



        }

    }
}
