using Microsoft.AspNetCore.Identity;
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
		[EnumDataType(typeof(Authorities))]
		[DataType(DataType.Text)]
		public Authorities Authority { get; set; }
		public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
	}
}
