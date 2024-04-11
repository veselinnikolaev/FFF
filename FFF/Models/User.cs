﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	[Table("Users")]
	public class User
	{
		[Key]
		public long Id { get; set; }
		[Required(ErrorMessage = "Usename is required")]
		[StringLength(15, ErrorMessage = "Username cannot be longer than 15 characters")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Password must be minimum 8 symbols and has at least one number, upper case, lower case and a special character")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
		[NotMapped]
		public string ConfirmPassword { get; set; }
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage ="Email is incorrect")]
		public string Email { get; set; }
		public ICollection<Role> Roles { get; } = new List<Role>();
		public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
	}
}