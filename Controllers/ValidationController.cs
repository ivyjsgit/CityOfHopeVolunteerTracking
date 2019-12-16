using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoHO.Controllers
{
    public class ValidationController : Controller
    { 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CoHO.Data.ApplicationDbContext _context;

        public ValidationController(UserManager<IdentityUser> userManager,
    CoHO.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailValid(Volunteer Volunteer)
        {
            var user = await _userManager.FindByEmailAsync(Volunteer.Email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"{Volunteer.Email} is already in use.");
            }
        }
    }
}