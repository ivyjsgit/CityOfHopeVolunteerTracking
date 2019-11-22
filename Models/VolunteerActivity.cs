using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class VolunteerActivity
    {
        [Key]
        public int VolunteerActivityID { get; set; }
        public int VolunteerId { get; set; }
        public int InitiativeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan ElapsedTime
        {
            get
            {
                return EndTime - StartTime;
            }
        }
        public Boolean ClockedIn { get; set; }

        public Volunteer Volunteer { get; set; }
        public Initiative Initiative { get; set; }
    }
}
