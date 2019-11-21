using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class SkillSelection
    {
        public int SkillSelectionID {get;set;}
        public int SKillID { get; set; }
        public int VolunteerID { get; set; }

        public Skill Skill { get; set; }
        public Volunteer Volunteer { get; set; }
    }
}
