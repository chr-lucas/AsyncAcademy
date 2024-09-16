using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace AsyncAcademy.Utils
{
    public class ValidiateBirthdayAttribute : ValidationAttribute
    {
        public ValidiateBirthdayAttribute()
        {
            const string defaultErrorMessage = "Error with Birthday";
            ErrorMessage ??= defaultErrorMessage;
        }

        public string GetErrorMessage() => "You must be older than 16 to register.";
        public string GetTimeTravelMessage() => "Selected Birthday is in the future. Time travel not allowed.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Birthday is Required.");
            }

            DateTime dob = (DateTime)value;
            DateTime today = DateTime.Today.Date;

            if (dob > today)
            {
                return new ValidationResult(GetTimeTravelMessage());
            }

            if (today >= dob.AddYears(16))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}
