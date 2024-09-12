using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace AsyncAcademy.Utils
{
    public class ValidiateBirthdayAttribute : ValidationAttribute
    {
        public ValidiateBirthdayAttribute()
        {
            const string defaultErrorMessage = "Error with Date of Birth";
            ErrorMessage ??= defaultErrorMessage;
        }

        public string GetErrorMessage() => "You must be older than 16 to register.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Birth Date is Required.");
            }
            DateTime dob = (DateTime)value;
            DateTime today = DateTime.Today.Date;

            if (today >= dob.AddYears(16))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(GetErrorMessage());
        }
    }
}
