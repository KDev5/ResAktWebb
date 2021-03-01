using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class ActivityBooking
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int NumParticipants { get; set; }
 
        // FK
        
        [ForeignKey("Activity")]
		public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        
	}
}
