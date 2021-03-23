

using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Text;

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
        [BindProperty]
        public Volunteer OurVolunteer { get; set; }
        public Initiative Initiative { get; set; }
        public VolunteerType VolunteerTypes { get; set; }

        public string Email { get; set; }
        
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
            ViewData["Email"] = "";

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
            StringBuilder sb = new StringBuilder();

            //Accessing first worksheet in the workbook.
            String[,] worksheeArray = new String[20, 2];

            // Here we separately check through each of the three volunteer types.
            int i = 1;

            foreach (var volunteerType in volunteerTypes)
            {
                worksheeArray[i, 0] = volunteerType.Description;

                var volunteersOfType = volunteers.Where(m => m.VolunteerType.Description == volunteerType.Description);
                i++;
                foreach (var volunteer in volunteersOfType)
                {
                    double hours = 0.0;
                    worksheeArray[i, 0] = volunteer.FullName;

                    var activitiesRange = volunteerActivities.Where(m => m.Volunteer == volunteer && m.StartTime >= start && m.StartTime <= end);
                    foreach (var activity in activitiesRange)
                    {
                        hours += activity.ElapsedTime.Hours;
                        hours += (activity.ElapsedTime.Minutes / 60.0);
                    }
                    worksheeArray[i, 1] = Math.Round(hours, 2) + "";

                    i++;
                }

                // Setting totals of current volunteer type
                worksheeArray[i, 0] = volunteerType.Description + " Hours Total";
                float total = 0;
                for (int j = i - volunteersOfType.Count(); j <= i - 1; j++) 
                {
                    total += float.Parse(worksheeArray[j, 1]);
                }
                worksheeArray[i, 1] = total.ToString();
                i += 2;
            }

            StringBuilder currentSb = new StringBuilder();

            for (int k = 0; k < worksheeArray.GetLength(0); k++)
            {
                String row = "";
                for (int t = 0; t < worksheeArray.GetLength(1); t++)
                {
                    row += worksheeArray[k, t] + ",";
                }
                row += Environment.NewLine;
                currentSb.Append(row);
            }



            byte[] bytes = Encoding.ASCII.GetBytes(currentSb.ToString());

            FileContentResult fileResult = new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "Hours.csv"
            };

            RedirectToPage("./adminindex");
            return fileResult;
        }






        public IActionResult OnPostYearReport()
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

            List<StringBuilder> sblist = new List<StringBuilder>();
            for (int i = 0; i < numTypes + 1; i++){
                sblist.Add(new StringBuilder());
            }

            StringBuilder sb1 = sblist[0];
            string title = "Totals" + Environment.NewLine;
            sb1.Append(title);

            String[,] worksheet1data = new String[35,15];

            int rows = 0;
            //looping through initiatives
            for (int currentInitiative = 0; currentInitiative < numInitiatives + 1; currentInitiative++)
            {
                rows = 2 * (currentInitiative + 1) + 1;
                if (currentInitiative > 0)
                {
                    // non-staff values and hours. 
                    GenerateStaffNonStaffHours(worksheet1data, currentInitiative, initiatives);
                }
                else
                {
                    // staff values and hours.
                    worksheet1data[2,1] = "Staff Hours";
                    worksheet1data[3,1] = "Staff Value";

                }

                //looping through the months.
                for (int j = 1; j < 13; j++)
                {
                    // Initializing the hours and values to keep track of current month's (j) total values and hours.
                    double hours = 0;
                    double value = 0;

                    //querying the activities of the current month (j) and initiative id (i).
                    System.Linq.IQueryable<CoHO.Models.VolunteerActivity> activities;

                    // If l == 0 then we are in the first two rows that correspond to the staff hours/values.
                    if (currentInitiative == 0)
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j);
                    }
                    // If we are looking at hours for initiatives and non-staff.
                    else
                    {
                        activities = volunteerActivities.Where(m => m.StartTime.Month == j && m.InitiativeId == currentInitiative);

                    }

                    foreach (var activity in activities.Where(m => m.StartTime.Year == year))
                    {
                        // time for this one activity
                        double time = activity.ElapsedTime.Hours + (activity.ElapsedTime.Minutes / 60.0);

                        //adding the current activities hours and values to their respective variables
                        // First check if they're staff.
                        if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID != 2 && currentInitiative != 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }
                        else if (volunteers.Single(m => m.VolunteerID == activity.VolunteerId).VolunteerTypeID == 2 && currentInitiative == 0)
                        {
                            hours += time;
                            value += time * valuesOfHours.OrderBy(m => m.EffectiveDate).Last(m => m.EffectiveDate <= activity.StartTime).Value;
                        }

                    }

                    //adding the hours and values to the table.
                    worksheet1data[2 * (currentInitiative + 1), j + 1] = Math.Round(hours, 2).ToString();
                    worksheet1data[2 * (currentInitiative + 1) + 1, j + 1] = Math.Round(value, 2).ToString();
                }

                // Formulas for summing the data from the rows.
                int row = 2 * (currentInitiative + 1);

                float total = 0;
                float totalValue = 0;
                for (int i = 2; i < 14; i++) {
                    total += float.Parse(worksheet1data[row, i]);
                    totalValue += float.Parse(worksheet1data[row + 1, i]);
                }
                worksheet1data[row, 14]  = "" + total;
                worksheet1data[row + 1, 14] = "" + totalValue; //check this place for potental bug


            }

            // Formulas for summing the totals of all hours/values.
            worksheet1data[rows + 1, 1] = "Total Hours";
            worksheet1data[rows + 2, 1] = "Total Value";

            float totalHours = 0;
            float totalValues = 0;
            for (int i = 2; i < rows; i++)
            {
                if (i % 2 == 0)
                {
                    totalHours += float.Parse(worksheet1data[i, 14]);
                }
                else
                {
                    totalValues += float.Parse(worksheet1data[i, 14]);
                }
            }

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

            worksheet1data[rows + 1, 14] = "" + totalHours;
            worksheet1data[rows + 2, 14] = "" + totalValues;


            String[] months = { "January", "February", "March", "April", "May",
                "June", "July", "August", "September", "October", "November",
                "December" };
            for (int i = 1; i < 13; i++)
            {
                worksheet1data[1, i + 1] = months[i - 1];
            }
            worksheet1data[1, 14] = year + " Totals";

            // Call function to generate second table on first worksheet.
            SecondMainGraph(worksheet1data, rows + 5, months, numInitiatives);

            List<String[,]> worksSheets = new List<string[,]>();

            worksSheets.Add(worksheet1data);
            // Creating distinct worksheets for each volunteer type.
            int o = 1;
            foreach (var type in volunteerTypes)
            {
                String[,] currentWorkSheetData = new string[20, 25];
                VolunteerTypeWorksheet(currentWorkSheetData, type.Description, year, months);
                worksSheets.Add(currentWorkSheetData);
                o++;
            }

            for (int i = 0; i < sblist.Count(); i++) 
            {
                StringBuilder curreSb = sblist[i];
                String tableTitle = "";
                if (i == 1) {
                    tableTitle = "Volunteers" + Environment.NewLine;
                    curreSb.Append(tableTitle);
                }

                if (i == 2)
                {
                    tableTitle = "Staff" + Environment.NewLine;
                    curreSb.Append(tableTitle);
                }

                if (i == 3)
                {
                    tableTitle = "Board" + Environment.NewLine;
                    curreSb.Append(tableTitle);
                }

                String[,] currentWorkSheetArray = worksSheets[i];
                for (int k = 0; k < currentWorkSheetArray.GetLength(0); k++) 
                {
                    String row = "";
                    for (int t = 0; t < currentWorkSheetArray.GetLength(1); t++) 
                    {
                        row += currentWorkSheetArray[k, t] + ",";
                    }
                    row += Environment.NewLine;
                    curreSb.Append(row);
                }
            }

            StringBuilder totalsb = new StringBuilder();

            foreach (StringBuilder sb in sblist) 
            {
                totalsb.Append(sb.ToString());
                totalsb.Append(Environment.NewLine);
            }

            byte[] bytes = Encoding.ASCII.GetBytes(totalsb.ToString());

            FileContentResult fileResult = new FileContentResult(bytes, "text/csv")
            {
                FileDownloadName = "Hours/Values.csv"
            };

            return fileResult;
        }

        private static void GenerateStaffNonStaffHours(string[,] worksheet1data, int currentInitiative, DbSet<Initiative> initiatives)
        {
            worksheet1data[2 * (currentInitiative + 1), 1]=
                initiatives.SingleOrDefault(m => m.InitiativeID == currentInitiative).Description + " (non-staff) hours";

            worksheet1data[2 * (currentInitiative + 1) + 1, 1]=
                initiatives.SingleOrDefault(m => m.InitiativeID == currentInitiative).Description + " (non-staff) value";
        }


        public void SecondMainGraph(string[,] worksheet1data, int startRow, String[] months, int numInitiatives)
        {
            // Column Labels.
            AddColumnTitlesOverall(worksheet1data, startRow);

            for (int month = 0; month < 12; month++)
            {
                // This table is Made up of functions that rely on info from the main table.
                SumMainTable(worksheet1data, startRow, months, month, numInitiatives);
            }
        }

        private static void SumMainTable(string[,] worksheet1data, int startRow, string[] months, int curMonth, int numInitiatives)
        {
            worksheet1data[startRow + curMonth, 1] = months[curMonth];
            worksheet1data[startRow + curMonth, 2] = "" + worksheet1data[2, curMonth + 2];
            worksheet1data[startRow + curMonth, 3] = "" + worksheet1data[3, curMonth + 2];

            String volunteerHours = "=SUM(";
            String volunteerValue = "=SUM(";
            for (int j = 4; j < startRow - 5; j++)
            {
                if (j % 2 == 0)
                {
                    volunteerHours += "," + (char) (66 + curMonth) + j;
                }
                else
                {
                    volunteerValue += "," + (char) (66 + curMonth) + j;
                }
            }

            float volhourPerCell = 0;
            float volvaluePerCell = 0;
            float totalhourPerCell = 0;
            float totalValuePerCell = 0;

            for (int row = 5; row < 4+numInitiatives*2; row++) {
                if (row % 2 == 0)
                {
                    volhourPerCell += float.Parse(worksheet1data[row, curMonth + 2]);
                }
                else
                {
                    volvaluePerCell += float.Parse(worksheet1data[row, curMonth + 2]);
                }
            }
            
            totalhourPerCell += volhourPerCell + float.Parse(worksheet1data[2, curMonth + 2]);
            totalValuePerCell += volhourPerCell + float.Parse(worksheet1data[3, curMonth + 2]);

            worksheet1data[startRow + curMonth, 4] = "" + volhourPerCell;
            worksheet1data[startRow + curMonth, 5] = "" + volvaluePerCell;
            worksheet1data[startRow + curMonth, 6] = "" + totalhourPerCell;
            worksheet1data[startRow + curMonth, 7] = "" + totalValuePerCell;
        }

      

        private static void AddColumnTitlesOverall(string[,] worksheet1data, int startRow)
        {
            worksheet1data[startRow - 1, 2] = "Staff Hours";
            worksheet1data[startRow - 1, 3] = "Staff Value";
            worksheet1data[startRow - 1, 4] = "Volunteer Hours";
            worksheet1data[startRow - 1, 5] = "Volunteer Value";
            worksheet1data[startRow - 1, 6] = "Total Hours";
            worksheet1data[startRow - 1, 7] = "Total Value";
        }

        public void VolunteerTypeWorksheet(String[,] currentWorkSheetData, String volunteerType, int year, String[] months)
        {
            var volunteersOfType = _context.Volunteer.Where(m => m.VolunteerType.Description == volunteerType);
            var volunteerActivities = _context.VolunteerActivity;
            currentWorkSheetData[1, 1] = "Name";

            for (int j = 0; j < 12; j++)
            {
                if (j >= 0 && j <= 3)
                {
                    currentWorkSheetData[1, j + 2] = months[j];
                }
                else if (j <= 7)
                {
                    currentWorkSheetData[1, j + 3] = months[j];
                } else
                {
                    currentWorkSheetData[1, j + 4] = months[j];
                }

            }

            currentWorkSheetData[1, 6] = "Total Spring Hours";
            currentWorkSheetData[1, 11] = "Total Summer Hours";
            currentWorkSheetData[1, 16] = "Total Fall Hours";
            currentWorkSheetData[1, 17] = "Total for year";

            int i = 2;
            foreach (var volunteer in volunteersOfType)
            {
                var hoursForYear = volunteerActivities.Where(m => m.Volunteer == volunteer && m.StartTime.Year == year);
                currentWorkSheetData[i, 1] = volunteer.FullName;

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
                        currentWorkSheetData[i, j + 1] = Math.Round(hours, 2).ToString();
                    }
                    else if (j <= 8)
                    {
                        currentWorkSheetData[i, j + 2] = Math.Round(hours, 2).ToString();
                    } else
                    {
                        currentWorkSheetData[i, j + 3] = Math.Round(hours, 2).ToString();
                    }
                }

                float summerTotal = 0;
                float springTotal = 0;
                float fallTotal = 0;
                float overallTotal = 0;

                summerTotal = float.Parse(currentWorkSheetData[i, 2]) + float.Parse(currentWorkSheetData[i, 3])
                    + float.Parse(currentWorkSheetData[i, 4]) + float.Parse(currentWorkSheetData[i, 5]);

                springTotal = float.Parse(currentWorkSheetData[i, 7]) + float.Parse(currentWorkSheetData[i, 8])
                    + float.Parse(currentWorkSheetData[i, 9]) + float.Parse(currentWorkSheetData[i, 10]);

                fallTotal = float.Parse(currentWorkSheetData[i, 12]) + float.Parse(currentWorkSheetData[i, 13])
                    + float.Parse(currentWorkSheetData[i, 14]) + float.Parse(currentWorkSheetData[i, 15]);

                overallTotal = summerTotal + springTotal + fallTotal;
                currentWorkSheetData[i, 6] = "" + summerTotal;
                currentWorkSheetData[i, 11] = "" + springTotal;
                currentWorkSheetData[i, 16] = "" + fallTotal;
                currentWorkSheetData[i, 17] = "" + overallTotal;
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
            // value = Request.Form["stuff"];
            Console.WriteLine($"Our volunteers email is {OurVolunteer.Email}");
            try
            {
                Volunteer ourVolunteer =
                    (from volunteer in _context.Volunteer
                        where volunteer.Email.ToLower().Trim() == OurVolunteer.Email.ToLower().Trim()
                        select volunteer).ToList()[0];
                // Volunteer ourVolunteer = await _context.Volunteer
                //     .FirstOrDefaultAsync(m => m.Email.ToLower() == OurVolunteer.Email.ToLower());
           
                    return GenerateVolunteerWordDoc(ourVolunteer, volunteerActivities, initiatives);

                
            }
            catch
            {
                Console.WriteLine("Bad Volunteer");
                TempData["message"] = "VNF";

                return Redirect("./AdminIndex");
            }
   
          
        }

        private IActionResult GenerateVolunteerWordDoc(Volunteer ourVolunteer, DbSet<VolunteerActivity> volunteerActivities, DbSet<Initiative> initiatives)
        {
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

                    var StartAMPM = activity.StartTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
                    var EndAMPM = activity.EndTime.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);


                    int id = activity.InitiativeId;
                    info.Rows[row].Cells[0].AddParagraph().AppendText(Start.Split(' ')[0]);
                    info.Rows[row].Cells[1].AddParagraph()
                        .AppendText(initiatives.Single(m => m.InitiativeID == id).Description);
                    info.Rows[row].Cells[2].AddParagraph().AppendText(StartAMPM);
                    info.Rows[row].Cells[3].AddParagraph().AppendText(EndAMPM);
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
