using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }
        [Required]
        public string Description { get; set; }
        public Boolean InActive { get; set; }

        public ICollection<SkillSelection> SkillSelection { get; set; }

    }
}
