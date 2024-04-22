﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	[Table("Roles")]
	public class Role : IdentityRole
	{
		public ICollection<User> Users { get; } = new List<User>();
	}
}
