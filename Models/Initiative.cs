using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityOfHopeVolunteerTracking.Models
{
    public class Initiative
    {
        [Key]
        public int ID { get; set; }
        public string First { get; set; }
        public Boolean InActive { get; set; }





    }
}
