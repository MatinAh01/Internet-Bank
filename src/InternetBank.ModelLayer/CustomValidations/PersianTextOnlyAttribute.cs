using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.CustomValidations
{
    public class PersianTextOnlyAttribute : ValidationAttribute
    {
        private static readonly Regex PersianRegex = new(@"^[\u0600-\u06FF\s]+$", RegexOptions.Compiled);

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string text)
            {
                if (PersianRegex.IsMatch(text))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("This field can only contain Persian characters.");
                }
            }

            return new ValidationResult("Invalid input format.");
        }
    }
}