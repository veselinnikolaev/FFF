using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Models
{
    [Table("EventsEmployees")]
	public class EventEmployee
	{
        [Key]
        [ForeignKey("Event")]
        public long EventId { get; set; }
        public Event Event { get; set; }

        [Key]
        [ForeignKey("Employee")]
        public long EmployeeId { get; set; }
        public Employee Employee { get; set; }
	}
}
