using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFF.Areas.Identity;
using FFF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FFF.Data
{
    public class FFFContext : IdentityDbContext<User>
    {
        public FFFContext(DbContextOptions<FFFContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Event>()
				.Property(e => e.TicketPrice)
				.HasColumnType("decimal(18, 2)");

			builder.Entity<Employee>()
				.Property(e => e.BirthDate)
				.HasColumnType("date");

			builder.Entity<Reservation>()
				.HasOne(r => r.Event)
				.WithMany(e => e.Reservations)
				.HasForeignKey(r => r.EventId)
				.IsRequired();

			builder.Entity<Event>()
				.HasMany(e => e.Employees)
				.WithMany(e => e.Events);

            builder.Entity<User>()
				.HasMany(u => u.Reservations)
				.WithMany(r => r.Users);
        }
    }
}
