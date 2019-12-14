using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoHO.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("admin").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin"
                };

                IdentityResult result = userManager.CreateAsync(user, "Super123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddClaimAsync(user, new Claim("super", "true")).Wait();
                }
            }
        }
    }
}
