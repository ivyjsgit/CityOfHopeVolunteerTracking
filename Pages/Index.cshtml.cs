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
            ViewData["VolunteerId"] = new SelectList(_context.Volunteer, "VolunteerID", "Email");
            ViewData["InitiativeId"] = new SelectList(_context.Initiative, "InitiativeID", "Description");

            return Page();
        }

        [BindProperty]
        public VolunteerActivity VolunteerActivity { get; set; }
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
            if (!after){
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
            VolunteerActivity.InitiativeId = Initiative.InitiativeID;
            VolunteerActivity.StartTime = DateTime.Now;
            VolunteerActivity.EndTime = VolunteerActivity.StartTime.AddHours(2.0);
            VolunteerActivity.ClockedIn = true;




            _context.VolunteerActivity.Add(VolunteerActivity);
            await _context.SaveChangesAsync();
        }


        // more details see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{


        //    //Console.WriteLine("Our initiative is ");
        //    //Console.WriteLine(Initiative.Description);

        //    //Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.VolunteerID == VolunteerActivity.VolunteerId select volunteer).ToList()[0];

        //    //HandleClockRequests(ourVolunteer);
        //    //return RedirectToPage("./Index");
        //}

        //[HttpPost]
        public void OnPostClockOut()
        {
            Console.WriteLine("Clocking in");
            //Move over the clock in code here
            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.VolunteerID == VolunteerActivity.VolunteerId select volunteer).ToList()[0];
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





        }
        public void OnPostClockIn()
        {
            Console.WriteLine("Clocking out");
            Volunteer ourVolunteer = (from volunteer in _context.Volunteer where volunteer.VolunteerID == VolunteerActivity.VolunteerId select volunteer).ToList()[0];
            VolunteerActivity LastActivity = GetLastActivity(ourVolunteer);

            Clockin(ourVolunteer);

            //Put the clockout code here.

        }


    }
}

