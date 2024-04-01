using FFF.Data;
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

            // Check if there is any event scheduled on the provided date
            var isDateScheduled = dbContext.Events.Any(e => e.Date.Date == date.Date);

            if (isDateScheduled)
            {
                return new ValidationResult("The date is already scheduled for another event.");
            }

            return ValidationResult.Success;
        }
    }
}
