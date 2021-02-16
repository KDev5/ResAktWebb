using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class ActivityBooking
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int NumParticipants { get; set; }
 
        // FK
		public int ActivityId { get; set; }
        public Activity Activity { get; set; }
	}
}
