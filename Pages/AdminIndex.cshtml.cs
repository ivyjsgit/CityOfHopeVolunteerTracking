

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.XlsIO;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult OnGet()
        {
            //Initializing months, years, and days for date selector.
            String[] months = { "January", "February", "March", "April", "May",
                "June", "July", "August", "September", "October", "November",
                "December" };
            int[] years = new int[DateTime.Now.Year - 2010 + 1];
            for (int i = 2010; i <= DateTime.Now.Year; i++)
            {
                years[i - 2010] = i;
            }

            int[] days = new int[31];

            for (int i = 1; i <= 31; i++)
            {
                days[i - 1] = i;
            }

            ViewData["Months"] = new SelectList(months);
            ViewData["Years"] = new SelectList(years);
            ViewData["Days"] = new SelectList(days);

            //Querying the volunteers emails for the volunteer selector. 
            var ValidVolunteers = from v in _context.Volunteer
                                  where v.InActive == false
                                  orderby v.UserName // Sort by name.
                                  select v;
            ViewData["VolunteerId"] = new SelectList(ValidVolunteers, "VolunteerID", "UserName");


            return Page();
        }


        public FileStreamResult OnPostRange()
        {
            String startDate = Request.Form["start"];
            String endDate = Request.Form["end"];

            

            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;

            using ExcelEngine excelEngine = new ExcelEngine();
            //Initialize Application.
            IApplication application = excelEngine.Excel;

            //Set default version for application.
            application.DefaultVersion = ExcelVersion.Excel2013;

            //Create a new workbook.
            IWorkbook workbook = application.Workbooks.Create(1);

            //Accessing first worksheet in the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.EnableSheetCalculations();

            worksheet.Range[1, 1].Text = startDate;
            worksheet.Range[1, 2].Text = endDate;




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


        public FileStreamResult OnPostView()
        {
            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;


            //Number of initiatives currently.
            int numInitiatives = initiatives.Count();

            using ExcelEngine excelEngine = new ExcelEngine();
            //Initialize Application.
            IApplication application = excelEngine.Excel;

            //Set default version for application.
            application.DefaultVersion = ExcelVersion.Excel2013;

            //Create a new workbook.
            IWorkbook workbook = application.Workbooks.Create(1);

            //Accessing first worksheet in the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.EnableSheetCalculations();

            //Setting first column width to 25.
            worksheet.SetColumnWidth(1, 35);

            //Setting all other column widths to 15.
            for (int i = 2; i < 20; i++)
            {
                worksheet.SetColumnWidth(i, 15);
            }

            int rows = 0;
            //looping through initiatives
            for (int i = 0; i < numInitiatives + 1; i++)
            {
                rows = 2 * (i + 1) + 1;
                if (i > 0)
                {
                    // non-staff values and hours.
                    worksheet.Range[2 * (i + 1), 1].Text = initiatives.Single(m => m.InitiativeID == i).Description + " (non-staff) hours";
                    worksheet.Range[2 * (i + 1) + 1, 1].Text = initiatives.Single(m => m.InitiativeID == i).Description + " (non-staff) value";
                }
                else
                {
                    // staff values and hours.
                    worksheet.Range[2, 1].Text = "Staff Hours";
                    worksheet.Range[3, 1].Text = "Staff Value";

                }

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
                int row = 2 * (i + 1);
                worksheet.Range[row, 14].Formula = "=SUM(A" + row + ":M" + row + ")";
                worksheet.Range[row + 1, 14].Formula = "=SUM(A" + (row + 1) + ":M" + (row + 1) + ")";



            }

            worksheet.Range[rows + 1, 1].Text = "Total Hours";
            worksheet.Range[rows + 2, 1].Text = "Total Value";
            String hoursFormula = "=SUM(N2";
            String valueFormula = "=SUM(N3";
            for (int i = 4; i <= rows; i++)
            {
                if (i % 2 == 0)
                {
                    hoursFormula += ",N" + i;
                }
                else
                {
                    valueFormula += ",N" + i;
                }
            }
            worksheet.Range[rows + 1, 14].Formula = hoursFormula + ")";
            worksheet.Range[rows + 2, 14].Formula = valueFormula + ")";


            String[] months = { "January", "February", "March", "April", "May",
                "June", "July", "August", "September", "October", "November",
                "December" };
            for (int i = 1; i < 13; i++)
            {
                worksheet.Range[1, i + 1].Text = months[i - 1];
            }
            worksheet.Range[1, 14].Text = DateTime.Now.Year + " Totals";

            SecondMainGraph(worksheet, rows + 5, months);


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


        public void SecondMainGraph(IWorksheet worksheet, int startRow, String[] months)
        {
            worksheet.Range[startRow - 1, 2].Text = "Staff Hours";
            worksheet.Range[startRow - 1, 3].Text = "Staff Value";
            worksheet.Range[startRow - 1, 4].Text = "Volunteer Hours";
            worksheet.Range[startRow - 1, 5].Text = "Volunteer Value";
            worksheet.Range[startRow - 1, 6].Text = "Total Hours";
            worksheet.Range[startRow - 1, 7].Text = "Total Value";

            for (int i = 0; i < 12; i++)
            {
                worksheet.Range[startRow + i, 1].Text = months[i];
                worksheet.Range[startRow + i, 2].Formula = "=SUM(" + (char)(66 + i) + "2)";
                worksheet.Range[startRow + i, 3].Formula = "=SUM(" + (char)(66 + i) + "3)";
                String volunteerHours = "=SUM(";
                String volunteerValue = "=SUM(";
                for (int j = 4; j < startRow - 5; j++)
                {
                    if (j % 2 == 0)
                    {
                        volunteerHours += "," + (char)(66 + i) + j;
                    }
                    else
                    {
                        volunteerValue += "," + (char)(66 + i) + j;
                    }
                }
                worksheet.Range[startRow + i, 4].Formula = volunteerHours + ")";
                worksheet.Range[startRow + i, 5].Formula = volunteerValue + ")";
                worksheet.Range[startRow + i, 6].Formula = "=SUM(B" + (startRow + i) + "+D" + (startRow + i) + ")";
                worksheet.Range[startRow + i, 7].Formula = "=SUM(C" + (startRow + i) + "+E" + (startRow + i) + ")";

            }
        }
    }
}
