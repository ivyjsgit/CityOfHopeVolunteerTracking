using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class Volunteer
    {
        public int VolunteerID { get; set; }
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string First { get; set; }
        [Required]
        public string Last { get; set; }
        public string FullName {
            get
            {
                return First + " " + Last;
            }
        }
        [DataType(DataType.Date)]
        public DateTime? Birthday{ get; set; }
        public string Email { get; set; }
        public string Home { get; set; }
        public string Cell { get; set; }
        public int? RaceID { get; set; }
        [Required]
        public int VolunteerTypeID { get; set; }
        public int? EducationLevelID { get; set; }
        public int? DisabilityID { get; set; }
        public int? SkillID { get; set; }
        public Boolean LoggedIn { get; set; }
        public int? VolunteerActivityID { get; set; }
        public Boolean InActive { get; set; }
        public Boolean CommunityService { get; set; }
        public Boolean WorkersComp { get; set; }
        public Boolean Veteran { get; set; }
        public Boolean Admin { get; set; }
        
        public Race Race { get; set; }
        public VolunteerType VolunterrType { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public ICollection<Disability> Disabilities { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<VolunteerActivity> VolunteerActivities { get; set; }

    }
}
