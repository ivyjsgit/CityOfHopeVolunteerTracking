using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class Initiative
    {
        [Key]
        public int InitiativeID { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Inactive")]
        public Boolean InActive { get; set; }

        public ICollection<VolunteerActivity> VolunteerActivity { get; set; }

    }
}
