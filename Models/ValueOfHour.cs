using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHO.Models
{
    public class ValueOfHour
    {
        [Key]
        public int ID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public float Value { get; set; }
    }

}

