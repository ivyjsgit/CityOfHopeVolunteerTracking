using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CoHO.Pages
{
    public class IndexModel : PageModel
    {
        Boolean clockin = false;
        DateTime clockin_time = DateTime.Now;
        DateTime clockout_time = DateTime.Now;
        int temp_user_id = 1;

        protected void button1_click(object sender, EventArgs e)
        {
            if (temp_user_id == 1) {
                if (clockin == false)
                {
                    this.clockin = true;
                    this.clockin_time = DateTime.Now;


                }
                else if (clockin == true)
                {
                    this.clockin = false;
                    this.clockout_time = DateTime.Now;
                }
            }
        }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}