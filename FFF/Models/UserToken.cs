using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
	public class UserToken
	{
        [Key]
		public int Id { get; set; }

        public string UserId { get; set; }  // Foreign key to ApplicationUser

        public string Token { get; set; }

        public string TokenType { get; set; }  // E.g., "EmailConfirmation"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property to ApplicationUser
        public virtual User User { get; set; }
	}
}
