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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.TicketPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Employee>()
                .Property(e => e.BirthDate)
                .HasColumnType("date");

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reservations)
                .HasForeignKey(r => r.EventId)
                .IsRequired();

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Employees)
                .WithMany(e => e.Events);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithOne();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne();
        }

    }
}