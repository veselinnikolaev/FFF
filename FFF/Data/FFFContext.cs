using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FFF.Models;
using System.Configuration;
using Microsoft.Extensions.Hosting;

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
            modelBuilder.Entity<Event>()
                .Property(e => e.TicketPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.BirthDate)
                .HasColumnType("date");

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

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Events)
                .WithMany(e => e.Employees)
                .UsingEntity<EmployeeEvent>(
                    x => x.HasOne<Event>(e => e.Event).WithMany(e => e.EmployeeEvents),
                    y => y.HasOne<Employee>(e => e.Employee).WithMany(e => e.EmployeeEvents));

            modelBuilder.Entity<Event>()
                .HasMany(emp => emp.Employees)
                .WithMany(emp => emp.Events)
                .UsingEntity<EmployeeEvent>(
                    x => x.HasOne<Employee>(e => e.Employee).WithMany(e => e.EmployeeEvents),
                    y => y.HasOne<Event>(e => e.Event).WithMany(e => e.EmployeeEvents));
        }
    }
}