using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class EducationLevel
    {
        [Key]
        public int EducationLevelID { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Inactive")]
        public Boolean InActive { get; set; }

        public ICollection<Volunteer> Volunteer { get; set; }
    }
}
