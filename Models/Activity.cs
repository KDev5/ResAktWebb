using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResAktWebb.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual List<ActivityBooking> ActivityBookings {get; set;}


      //  public List<ActivityBooking> ActivityBookings{ get; set; } = new List<ActivityBooking>();
    }
}
