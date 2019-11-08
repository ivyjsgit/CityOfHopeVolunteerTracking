using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityOfHopeVolunteerTracking.Models
{
    public class VolunteerActivity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Initiative { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float ElapsedTime { get; set; }
        public Boolean ClockedIn { get; set; }
    }
}
