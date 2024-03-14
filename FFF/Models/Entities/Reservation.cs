using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	public class Reservation
	{
		[Key]
		public long Id { get; set; }
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
		public string Date { get; set; }
		public string Note { get; set; }
	}
}
