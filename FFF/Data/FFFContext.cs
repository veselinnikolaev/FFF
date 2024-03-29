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
        public FFFContext(DbContextOptions<FFFContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeEvent>()
                .HasKey(ee => new { ee.EventId, ee.EmployeeId });

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reservations)
                .HasForeignKey(r => r.EventId);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Reservations)
                .WithOne(r => r.Event)
                .HasForeignKey(r => r.EventId);

            modelBuilder.Entity<EmployeeEvent>()
                .HasOne(ee => ee.Event)
                .WithMany(e => e.EmployeeEvents)
                .HasForeignKey(ee => ee.EventId);

            modelBuilder.Entity<EmployeeEvent>()
                .HasOne(ee => ee.Employee)
                .WithMany(emp => emp.EmployeeEvents)
                .HasForeignKey(ee => ee.EmployeeId);
        }
    }
}