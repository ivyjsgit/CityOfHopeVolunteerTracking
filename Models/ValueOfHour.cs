using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityOfHopeVolunteerTracking.Models
{
    public class ValueOfHour
    {
        [Key]
        public int ID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public float Value { get; set; }
    }
}