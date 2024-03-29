﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	[Table("Employees")]
	public class Employee
	{
		[Key]
		public long Id { get; set; }
		
		[Required(ErrorMessage = "First Name is required")]
		[StringLength(20, ErrorMessage = "CustomerName cannot be longer than 20 characters")]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		
		[Required(ErrorMessage = "Last Name is required")]
		[StringLength(20, ErrorMessage = "CustomerName cannot be longer than 20 characters")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		
		[Required(ErrorMessage = "Date of Birth is required")]
		[DataType(DataType.DateTime)]
		[Display(Name = "Date of Birth")]
		public DateTime BirthDate { get; set; }
		
		[Required(ErrorMessage = "Position is required")]
		[EnumDataType(typeof(Positions))]
		public Positions Position { get; set; }
		
		[Required(ErrorMessage = "Phone Number is required")]
		[Phone(ErrorMessage = "Invalid Phone Number")]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		
		[Required(ErrorMessage = "Email Address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required] //Foreign key Many to Many
        public ICollection<EventEmployee> EventEmployees { get; set; }
	}
}
