using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class DisabilitySelection
    {
        public int DisabilitySelectionID { get; set; }
        public int DisabilityID { get; set; }
        public int VolunteerID { get; set; }

        public Disability Disability { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
