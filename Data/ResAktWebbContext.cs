using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResAktWebb.Models;

namespace ResAktWebb.Data
{
    public class ResAktWebbContext : DbContext
    {
        public ResAktWebbContext (DbContextOptions<ResAktWebbContext> options)
            : base(options)
        {
        }

        public DbSet<ResAktWebb.Models.Reservation> Reservation { get; set; }

        public DbSet<ResAktWebb.Models.Menu> Menu { get; set; }
    }
}
