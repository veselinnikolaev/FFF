using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public User() : base()
        {
        }
        public User(string userName) : base(userName)
        {
        }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
    }
}
