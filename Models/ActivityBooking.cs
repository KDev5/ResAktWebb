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
        public Activity Activity { get; set; }
        public int NumParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
