using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	public class Event
	{
		public long Id { get; set; }
		public string Date { get; set; }
		public string SingerName { get; set; }
		public decimal TicketPrice { get; set; }
		public string Description { get; set; }
	}
}
