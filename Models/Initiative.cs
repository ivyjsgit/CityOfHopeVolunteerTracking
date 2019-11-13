using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class Initiative
    {
        [Key]
        public int InitiativeID { get; set; }
        public string First { get; set; }
        public Boolean InActive { get; set; }

    }
}
