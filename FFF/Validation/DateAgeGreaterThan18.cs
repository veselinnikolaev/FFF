using System;
using System.ComponentModel.DataAnnotations;

namespace FFF.Validation
{
    public class DateAgeGreaterThan18Attribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateOfBirth = (DateTime)value;

            // Calculate age
            int age = CalculateAge(dateOfBirth);

            // Check if age is greater than 18
            if (age < 18)
            {
                return new ValidationResult(ErrorMessage = "Employee must be older than 18"); ;
            }

            return ValidationResult.Success;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            // Subtract a year if the birthday hasn't occurred yet this year
            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
