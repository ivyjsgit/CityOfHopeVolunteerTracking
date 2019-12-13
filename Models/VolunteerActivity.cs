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
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime StartTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime EndTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h\\:mm}")]
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
