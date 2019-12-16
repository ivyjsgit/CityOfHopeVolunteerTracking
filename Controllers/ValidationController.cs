using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailValid(string email)
        {
          var user = await _userManager.FindByEmailAsync(email);
         if (user == null)
            {
                var i = 1;
                return Json(true);
            } else
            {
                var i = 2;
                //return Json(false);
                return Json($"{email} is already in use.");
            }
        }
    }
}