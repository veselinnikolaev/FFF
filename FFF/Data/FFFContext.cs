using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FFF.Models;

namespace FFF.Data
{
    public class FFFContext : DbContext
    {
        public FFFContext (DbContextOptions<FFFContext> options)
            : base(options)
        {
        }

        public DbSet<FFF.Models.Reservation> Reservations { get; set; }
        public DbSet<FFF.Models.Event> Events { get; set; }
        public DbSet<FFF.Models.Employee> Employees { get; set; }
    }
}
