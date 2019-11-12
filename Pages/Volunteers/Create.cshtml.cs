using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CityOfHopeVolunteerTracking.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace CityOfHopeVolunteerTracking.Pages.Volunteers
{
    public class CreateModel : PageModel
    {
        private readonly CityOfHopeVolunteerTracking.Data.COHODatabaseContext _context;

        public CreateModel(CityOfHopeVolunteerTracking.Data.COHODatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Volunteer Volunteer { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Volunteer.Add(Volunteer);

            await _context.SaveChangesAsync();

            //Create Identity
            var user = new ApplicationUser() { UserName = Volunteer.UserName };
            var userStore = new UserStore<IdentityUser>();
           // var userManager = new UserManager<ApplicationUser>();


            //private UserManager<ApplicationUser> _userManager;
            //var result = userManager.CreateAsync(user, Volunteer.Password);






            return RedirectToPage("./Index");
        }
    }

    internal class UserStore<T>
    {
        public UserStore()
        {
        }
    }
}
