using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoHO.Data;
using CoHO.Models;
using Microsoft.EntityFrameworkCore;

namespace CoHO.Pages
{
    public class CreateModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public CreateModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerID", "UserName");
            ViewData["InitiativeId"] = new SelectList(_context.Initiative, "InitiativeID", "Description");

            return Page();
        }

        [BindProperty]
        public VolunteerActivity VolunteerActivity { get; set; }
        [BindProperty]
        public Volunteer Volunteers { get; set; }
        [BindProperty]
        public Initiative Initiative { get; set; }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for

        public VolunteerActivity GetLastActivity(Volunteer volunteer)
        {

            List<VolunteerActivity> VolunteerActivities = (from volunteeractivity in _context.VolunteerActivity where volunteeractivity.VolunteerId == volunteer.VolunteerID orderby volunteeractivity.EndTime select volunteeractivity).ToList();
            VolunteerActivities.Reverse();
            //Console.WriteLine("Our good one");

            //Console.WriteLine(VolunteerActivities[0]);
            try
            {
                return VolunteerActivities[0];

            }
            catch
            {
                return null;
            }
        }


        public async void DoClockout(Volunteer ourVolunteer, Boolean after)
        {
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);
            if (!after)
            {
                LastActivity.EndTime = DateTime.Now;

            }
            LastActivity.ClockedIn = false;

            _context.Attach(LastActivity).State = EntityState.Modified;

            await _context.SaveChangesAsync();



        }

        public async void HandleClockRequests(Volunteer ourVolunteer)
        {
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);

            // If the user clicks the button before they hit the 2 hour mark
            if (LastActivity != null)
            {
                if (LastActivity.ClockedIn)
                {
                    if (DateTime.Compare(LastActivity.EndTime, DateTime.Now) > 0)
                    {
                        DoClockout(ourVolunteer, false);

                    }
                    else
                    {
                        DoClockout(ourVolunteer, true);
                    }
                }
                else
                {
                    //Check if the last thing is clocked in
                    Clockin(ourVolunteer);
                }
            }
            else
            {
                Clockin(ourVolunteer);
            }

        }

        public async void Clockin(Volunteer ourVolunteer)
        {

            Console.WriteLine("Our volunteer still is ");
            Console.WriteLine(ourVolunteer.Email);
            VolunteerActivity.InitiativeId = Initiative.InitiativeID;
            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(2.0);
            VolunteerActivity.ClockedIn = true;
            VolunteerActivity.Volunteer = ourVolunteer;
            VolunteerActivity.Initiative = (from activity in _context.Initiative where activity.InitiativeID == Initiative.InitiativeID select activity).ToList()[0];



            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();
        }


        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostClockInClockOut()
        {


            //Console.WriteLine("Our initiative is ");
            //Console.WriteLine(Initiative.Description);
            Console.Write(Volunteers.UserName);


            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email == Volunteers.Email select volunteer).ToList()[0];
            Console.Write("Our Volunteer is....");
            Console.Write(ourVolunteer.Email);



            HandleClockRequests(ourVolunteer);
            return RedirectToPage("./Index");
        }

        //[HttpPost]
        public async Task<IActionResult> OnPostClockOut()
        {
            Console.WriteLine("Clocking in");
            //Move over the clock in code here
            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email == Volunteers.Email select volunteer).ToList()[0];
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);

            if (LastActivity != null)
            {
                if (LastActivity.ClockedIn)
                {
                    if (DateTime.Compare(LastActivity.EndTime, DateTime.Now) > 0)
                    {
                        DoClockout(ourVolunteer, false);
                    }
                    else
                    {
                        DoClockout(ourVolunteer, true);
                    }
                }
                else
                {
                    Clockin(ourVolunteer);
                }
            }



            return RedirectToPage("./Index");


        }
        public async Task<IActionResult> OnPostClockIn()
        {
            Console.WriteLine("Clocking out");
            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.Email == Volunteers.Email select volunteer).ToList()[0];
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);

            Clockin(ourVolunteer);
            return RedirectToPage("./Index");


            //Put the clockout code here.

        }


    }
}

