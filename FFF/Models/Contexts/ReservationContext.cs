using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models.Contexts
{
	public class ReservationContext : DbContext
	{
		public ReservationContext() : base("name=ReservationContext")
		{
		}
		public DbSet<Reservation> Reservations{ get; set; }
	}
}
