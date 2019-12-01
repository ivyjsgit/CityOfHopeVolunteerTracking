

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Office.Interop.Excel;
using Syncfusion.XlsIO;
using System.IO;


namespace CoHO.Pages
{
    public class AdminIndexModel : PageModel
    {
        private readonly CoHO.Data.ApplicationDbContext _context;

        public AdminIndexModel(CoHO.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public VolunteerActivity VolunteerActivity { get; set; }
        public Volunteer Volunteer { get; set; }
        public Initiative Initiative { get; set; }

        public void OnGet()
        {

        }




        public FileStreamResult OnPostView()
        {
            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;

            //Save information to 2d array.
            int numInitiatives = initiatives.Count();
            double[,] initiativeMonth = new double[numInitiatives, 13];

            using ExcelEngine excelEngine = new ExcelEngine();
            //Initialize Application.
            IApplication application = excelEngine.Excel;

            //Set default version for application.
            application.DefaultVersion = ExcelVersion.Excel2013;

            //Create a new workbook.
            IWorkbook workbook = application.Workbooks.Create(1);

            //Accessing first worksheet in the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];

            //Setting first column width to 25.
            worksheet.SetColumnWidth(1, 25);

            //Setting all other column widths to 15.
            for (int i = 2; i < 20; i++)
            {
                worksheet.SetColumnWidth(i, 15);
            }

            //looping through initiatives
            for (int i = 0; i < numInitiatives + 1; i++)
            {
                
                if (i > 0)
                {
                    worksheet.Range[2 * (i + 1), 1].Text = initiatives.Single(m => m.InitiativeID == i).Description + "(non-staff) hours";
                    worksheet.Range[2 * (i + 1) + 1, 1].Text = initiatives.Single(m => m.InitiativeID == i).Description + "(non-staff) value";
                }
                else
                {
                    worksheet.Range[2, 1].Text = "Staff Hours";
                    worksheet.Range[3, 1].Text = "Staff Value";
                }
                // labels for the months and initiatives.

                //looping through the months.
                for (int j = 1; j < 13; j++)
                {
                    // Initializing the hours and values to keep track of current month's (j) total values and hours.
                    double hours = 0;
                    double value = 0;

                    //querying the activities of the current month (j) and initiative id (i).
                    System.Linq.IQueryable<CoHO.Models.VolunteerActivity> activities;

                    if (i == 0)
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j);
                    }
                    else
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j && m.InitiativeId == i);

                    }

                    foreach (var activity in activities)
                    {
                        // time for this one activity
                        double time = activity.ElapsedTime.Hours + activity.ElapsedTime.Minutes / 60;

                        //adding the current activities hours and values to their respective variables
                        if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID == 1 && i != 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }
                        else if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID == 2 && i == 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }

                    }
                    //initiativeMonth[i - 1, j - 1] = hours;

                    //adding the hours and values to the table.
                    worksheet.Range[2 * (i + 1), j + 1].Number = Math.Round(hours, 2);
                    worksheet.Range[2 * (i + 1) + 1, j + 1].Number = Math.Round(value, 2);
                }
                
                
            }
            String[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 1; i < 13; i++)
            {
                worksheet.Range[1, i + 1].Text = months[i - 1];
            }

          

            




            //Saving the Excel to the MemoryStream 
            MemoryStream stream = new MemoryStream();

            workbook.SaveAs(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the Excel file in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel")
            {
                FileDownloadName = "Hours/Values.xlsx"
            };

            return fileStreamResult;



        }
    }
}
