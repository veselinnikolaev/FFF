using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	public class Employee
	{
		[Key]
		public long Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string BirthDate { get; set; }
		[EnumDataType(typeof(Position))]
		public Position Position { get; set; }
		public string PhoneNumber { get; set; }
	}
}
