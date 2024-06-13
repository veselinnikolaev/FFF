using FFF.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FFF.Validation
{
    public class AlreadyScheduledDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (FFFContext)validationContext.GetService(typeof(FFFContext));

            // Convert the value to DateTime
            DateTime date = (DateTime)value;

            long id = Convert.ToInt64(validationContext.ObjectInstance.GetType().GetProperty("Id").GetValue(validationContext.ObjectInstance));
            // Check if there is any event scheduled on the provided date excluding the current event
            var isDateScheduled = dbContext.Events.Any(e => e.Date.Date == date.Date && e.Id != id);

            if (isDateScheduled)
            {
                return new ValidationResult("The date is already scheduled for another event.");
            }

            return ValidationResult.Success;
        }
    }
}
