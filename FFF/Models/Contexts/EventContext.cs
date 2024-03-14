using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models.Contexts
{
	public class EventContext : DbContext
	{
		public EventContext() : base("name=EventContext")
		{
		}
		public DbSet<Event> Events { get; set; }
	}
}
