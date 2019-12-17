

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
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;

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
        public VolunteerType VolunteerTypes { get; set; }

        public string value { get; set; }
        
        public IActionResult OnGet()
        {
            //Initializing months, years, and days for date selector.
            String[] months = { "January", "February", "March", "April", "May",
                "June", "July", "August", "September", "October", "November",
                "December" };
            int[] years = new int[DateTime.Now.Year - 2019 + 1];
            for (int i = 2019; i <= DateTime.Now.Year; i++)
            {
                years[i - 2019] = i;
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


        public async Task<IActionResult> OnPostRangeAsync()
        {


            // month dictionary to convert the string input into the corresponding int for the month.
            Dictionary<String, Int32> months = new Dictionary<String, Int32>
            {
                { "January", 1 },
                { "February", 2 },
                { "March", 3 },
                { "April", 4 },
                { "May", 5 },
                { "June", 6 },
                { "July", 7 },
                { "August", 8 },
                { "September", 9 },
                { "October", 10 },
                { "November", 11 },
                { "December", 12 }
            };

            // Assigning input dates to their corresponding varialbles
            int startDay = Int32.Parse(Request.Form["start-day"]);
            int startMonth = months.GetValueOrDefault(Request.Form["start-month"]);
            int startYear = Int32.Parse(Request.Form["start-year"]);
            int endDay = Int32.Parse(Request.Form["end-day"]);
            int endMonth = months.GetValueOrDefault(Request.Form["end-month"]);
            int endYear = Int32.Parse(Request.Form["end-year"]);

            // Converting info to datetime objects.
            DateTime start = new DateTime(startYear, startMonth, startDay);
            DateTime end = new DateTime(endYear, endMonth, endDay);

            // Checking if the datetime is valid.
            if (start > end)
            {
                //Send a toast to the user saying that the selection is invalid.
                TempData["message"] = "NO";
                System.Threading.Thread.Sleep(500);
                return RedirectToPage("./adminindex");

            }

            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;
            var volunteerTypes = _context.VolunteerType;

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

            worksheet.SetColumnWidth(1, 20);

            // Here we separately check through each of the three volunteer types.
            int i = 1;

            foreach (var volunteerType in volunteerTypes)
            {
                worksheet.Range[i, 1].Text = volunteerType.Description;
                var volunteersOfType = volunteers.Where(m => m.VolunteerType.Description == volunteerType.Description);
                i++;
                foreach (var volunteer in volunteersOfType)
                {
                    double hours = 0.0;
                    worksheet.Range[i, 1].Text = volunteer.FullName;
                    var activitiesRange = volunteerActivities.Where(m => m.Volunteer == volunteer && m.StartTime >= start && m.StartTime <= end);
                    foreach (var activity in activitiesRange)
                    {
                        hours += activity.ElapsedTime.Hours;
                        hours += (activity.ElapsedTime.Minutes / 60.0);
                    }
                    worksheet.Range[i, 2].Number = Math.Round(hours, 2);
                    i++;
                }

                worksheet.Range[i, 1].Text = volunteerType.Description + " Hours Total";
                worksheet.Range[i, 2].Formula = "=SUM(B" + (i - volunteersOfType.Count() + ":B" + (i - 1) + ")");
                i += 2;
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

            RedirectToPage("./adminindex");
            return fileStreamResult;
        }






        public FileStreamResult OnPostView()
        {
            //Initialize database variables.
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var valuesOfHours = _context.ValueOfHour;
            var initiatives = _context.Initiative;
            var volunteerTypes = _context.VolunteerType;
            int numTypes = volunteerTypes.Count();
            int year = Int32.Parse(Request.Form["report-year"]);


            //Number of initiatives currently.
            int numInitiatives = initiatives.Count();

            using ExcelEngine excelEngine = new ExcelEngine();
            //Initialize Application.
            IApplication application = excelEngine.Excel;

            //Set default version for application.
            application.DefaultVersion = ExcelVersion.Excel2013;

            //Create a new workbook.
            IWorkbook workbook = application.Workbooks.Create(numTypes + 1);

            //Accessing first worksheet in the workbook.
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Totals";




            worksheet.EnableSheetCalculations();



            //Setting first column width to 25.
            worksheet.SetColumnWidth(1, 35);

            //Setting all other column widths to 15.
            for (int j = 2; j < 20; j++)
            {
                worksheet.SetColumnWidth(j, 15);
            }

            int rows = 0;
            //looping through initiatives
            for (int l = 0; l < numInitiatives + 1; l++)
            {
                rows = 2 * (l + 1) + 1;
                if (l > 0)
                {
                    // non-staff values and hours.
                    worksheet.Range[2 * (l + 1), 1].Text = initiatives.Single(m => m.InitiativeID == l).Description + " (non-staff) hours";
                    worksheet.Range[2 * (l + 1) + 1, 1].Text = initiatives.Single(m => m.InitiativeID == l).Description + " (non-staff) value";
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

                    if (l == 0)
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j);
                    }
                    else
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j && m.InitiativeId == l);

                    }

                    foreach (var activity in activities.Where(m => m.StartTime.Year == year))
                    {
                        // time for this one activity
                        double time = activity.ElapsedTime.Hours + (activity.ElapsedTime.Minutes / 60.0);

                        //adding the current activities hours and values to their respective variables
                        if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID != 2 && l != 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }
                        else if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID == 2 && l == 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }

                    }
                    //initiativeMonth[i - 1, j - 1] = hours;

                    //adding the hours and values to the table.
                    worksheet.Range[2 * (l + 1), j + 1].Number = Math.Round(hours, 2);
                    worksheet.Range[2 * (l + 1) + 1, j + 1].Number = Math.Round(value, 2);
                }
                int row = 2 * (l + 1);
                worksheet.Range[row, 14].Formula = "=SUM(A" + row + ":M" + row + ")";
                worksheet.Range[row + 1, 14].Formula = "=SUM(A" + (row + 1) + ":M" + (row + 1) + ")";



            }

            worksheet.Range[rows + 1, 1].Text = "Total Hours";
            worksheet.Range[rows + 2, 1].Text = "Total Value";
            String hoursFormula = "=SUM(N2";
            String valueFormula = "=SUM(N3";
            for (int m = 4; m <= rows; m++)
            {
                if (m % 2 == 0)
                {
                    hoursFormula += ",N" + m;
                }
                else
                {
                    valueFormula += ",N" + m;
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
            worksheet.Range[1, 14].Text = year + " Totals";

            SecondMainGraph(worksheet, rows + 5, months);

            int o = 1;
            foreach (var type in volunteerTypes)
            {
                VolunteerTypeWorksheet(workbook.Worksheets[o], type.Description, year, months);
                workbook.Worksheets[o].Name = type.Description;
                o++;
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

        public void VolunteerTypeWorksheet(IWorksheet worksheet, String volunteerType, int year, String[] months)
        {
            worksheet.SetColumnWidth(1, 25);
            worksheet.SetColumnWidth(6, 20);
            worksheet.SetColumnWidth(11, 20);
            worksheet.SetColumnWidth(16, 20);
            worksheet.SetColumnWidth(17, 20);


            var volunteersOfType = _context.Volunteer.Where(m => m.VolunteerType.Description == volunteerType);
            var volunteerActivities = _context.VolunteerActivity;
            worksheet.Range[1, 1].Text = "Name";
            for (int j = 0; j < 12; j++)
            {
                if (j >= 0 && j <= 3)
                {
                    worksheet.Range[1, j + 2].Text = months[j];
                }
                else if (j <= 7)
                {
                    worksheet.Range[1, j + 3].Text = months[j];
                } else
                {
                    worksheet.Range[1, j + 4].Text = months[j];
                }

            }
            worksheet.Range[1, 6].Text = "Total Spring Hours";
            worksheet.Range[1, 11].Text = "Total Summer Hours";
            worksheet.Range[1, 16].Text = "Total Fall Hours";
            worksheet.Range[1, 17].Text = "Total for year";

            int i = 2;
            foreach (var volunteer in volunteersOfType)
            {
                var hoursForYear = volunteerActivities.Where(m => m.Volunteer == volunteer && m.StartTime.Year == year);
                worksheet.Range[i, 1].Text = volunteer.FullName;

                for (int j = 1; j < 13; j++)
                {
                    double hours = 0.0;
                    var allHours = hoursForYear.Where(m => m.StartTime.Month == j);
                    foreach (var activity in allHours)
                    {
                        hours += activity.ElapsedTime.Hours;
                        hours += (activity.ElapsedTime.Minutes / 60.0);
                    }
                    if (j >= 1 && j <= 4)
                    {
                        worksheet.Range[i, j + 1].Number = Math.Round(hours, 2);
                    }
                    else if (j <= 8)
                    {
                        worksheet.Range[i, j + 2].Number = Math.Round(hours, 2);
                    } else
                    {
                        worksheet.Range[i, j + 3].Number = Math.Round(hours, 2);
                    }
                }

                worksheet.Range[i, 6].Formula = "=SUM(B" + i + ":E" + i + ")";
                worksheet.Range[i, 11].Formula = "=SUM(G" + i + ":J" + i + ")";
                worksheet.Range[i, 16].Formula = "=SUM(L" + i + ":O" + i + ")";
                worksheet.Range[i, 17].Formula = "=SUM(F" + i + ",K" + i + ",P" + i + ")";
                i++;
            }
        }


        public async Task<IActionResult> OnPostVolunteer()
        {
            //Initializing database variables
            var volunteers = _context.Volunteer;
            var volunteerActivities = _context.VolunteerActivity;
            var initiatives = _context.Initiative;

            //Pulling currently logged in user
            value = Request.Form["stuff"];

            Volunteer ourVolunteer = await _context.Volunteer
                .FirstOrDefaultAsync(m => m.VolunteerID == Convert.ToInt32(value));
   

        //Creating and formatting document
        WordDocument report = new WordDocument();
        IWSection section = report.AddSection();
        section.PageSetup.Margins.All = 50f;
        IWParagraph name = section.AddParagraph();
        name.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
        name.ParagraphFormat.AfterSpacing = 18f;
        IWTextRange nameText = name.AppendText(ourVolunteer.FullName);

        //Creating and formatting table
        IWTable info = section.AddTable();
        info.ResetCells(1, 5);
        info.Rows[0].Cells[0].AddParagraph().AppendText("Date");
        info.Rows[0].Cells[1].AddParagraph().AppendText("Initiative");
        info.Rows[0].Cells[2].AddParagraph().AppendText("Start Time");
        info.Rows[0].Cells[3].AddParagraph().AppendText("End Time");
        info.Rows[0].Cells[4].AddParagraph().AppendText("Elapsed Time");
        info.Rows[0].Height = 20;

        //Looping through initiatives and filling the table
        int row = 0;
        foreach (CoHO.Models.VolunteerActivity activity in volunteerActivities.ToArray())
        {
            if (activity.VolunteerId == ourVolunteer.VolunteerID)
            {
                info.AddRow();
                row += 1;
                info.Rows[row].Height = 20;
                string Start = activity.StartTime.ToString();
                string End = activity.EndTime.ToString();
                int id = activity.InitiativeId;
                info.Rows[row].Cells[0].AddParagraph().AppendText(Start.Split(' ')[0]);
                info.Rows[row].Cells[1].AddParagraph().AppendText(initiatives.Single(m => m.InitiativeID == id).Description);
                info.Rows[row].Cells[2].AddParagraph().AppendText(Start.Split(' ')[1]);
                info.Rows[row].Cells[3].AddParagraph().AppendText(End.Split(' ')[1]);
                info.Rows[row].Cells[4].AddParagraph().AppendText(activity.ElapsedTime.ToString().Split('.')[0]);
            }
        }

        //Assigning font
        nameText.CharacterFormat.FontName = "Times New Roman";
        nameText.CharacterFormat.FontSize = 14;

        //Saving document and downloading to user's computer
        MemoryStream stream = new MemoryStream();
        report.Save(stream, FormatType.Docx);
        report.Close();
        stream.Position = 0;
        return File(stream, "application/msword", "UserReport.docx");

        } 
    }

}
