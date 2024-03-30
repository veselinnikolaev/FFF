using System.ComponentModel.DataAnnotations;
using System;

namespace FFF.Validation
{
    public class DateGreaterThanNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime)value;

            if (currentValue < DateTime.Now)
            {
                return new ValidationResult(ErrorMessage = "Date must be greater than current date.");
            }

            return ValidationResult.Success;
        }
    }

}
