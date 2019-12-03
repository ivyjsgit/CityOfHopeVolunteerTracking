using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoHO.Pages
{
    public class IdentityIndexModel : PageModel
    {
        private readonly ILogger<IdentityIndexModel> _logger;

        public IdentityIndexModel(ILogger<IdentityIndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
