using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityOfHopeVolunteerTracking.Models
{
    public class VolunteerActivity
    {
        public int actvityID { get; set; }
        public string initiative { get; set; }
        public string userName { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int elapsedTime { get; set; }
        public Boolean clockedIn { get; set; }
    }
}
