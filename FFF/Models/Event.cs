﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	[Table("Events")]
	public class Event
	{
		[Key]
		public long Id { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[DataType(DataType.DateTime)]
		[Display(Name = "Date of Event")]
		public DateTime Date { get; set; }

		[Required(ErrorMessage = "First Name is required")]
		[StringLength(50, ErrorMessage = "Singer Name cannot be longer than 50 characters")]
		[Display(Name = "Singer Name")]
		public string SingerName { get; set; }

		[Required(ErrorMessage = "Ticket Price is required")]
		[RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid total")]
		[Range(1, 500, ErrorMessage = "Ticket Price must be between 1 and 500")]
		[Display(Name = "Ticket Price")]
		public decimal TicketPrice { get; set; }

		[Required(ErrorMessage = "Description is required")]
		[MaxLength(500, ErrorMessage = "Username cannot exceed 500 characters")]
		[DataType(DataType.Text)]
		public string Description { get; set; }
		
		[Required] //Foreign key Many to Many
        public ICollection<EventEmployee> EventEmployees { get; set; }

		[Required] //Foreign key One to Many
        public virtual ICollection<Reservation> Reservations { get; set; }
	}
}
