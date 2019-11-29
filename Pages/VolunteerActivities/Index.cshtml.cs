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
    public class IndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public IndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<VolunteerActivity> VolunteerActivity { get;set; }

        public async Task OnGetAsync()
        {
            VolunteerActivity = await _context.VolunteerActivity
                .Include(v => v.Volunteer).ToListAsync();
        }
        public static String GetActivityName(int ourID)
        {
            return "pls no crash";
            //return (from initiative in _context.Initiative where initiative.InitiativeID == ourID select initiative.Description).ToList()[0];

        }

    }
}
