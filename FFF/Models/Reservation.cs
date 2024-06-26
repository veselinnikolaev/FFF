﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	[Table("Reservations")]
	public class Reservation
	{
        public Reservation()
        {
        }

        [Key]
		public long Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Phone Number is required")]
		[Phone(ErrorMessage = "Ivalid Phone Number")]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }

		[MaxLength(500, ErrorMessage = "Username cannot exceed 500 characters")]
		[DataType(DataType.Text)]
		public string Note { get; set; }
		
		[Required(ErrorMessage = "Event selection is required")]
		[ForeignKey(nameof(Event))] //Foreign key One to One for the event
		[Display(Name = "Event")]
        public long EventId { get; set; }

		// Many to One
        public Event Event { get; set; }

		public ICollection<User> Users { get; } = new List<User>();
	}
}
