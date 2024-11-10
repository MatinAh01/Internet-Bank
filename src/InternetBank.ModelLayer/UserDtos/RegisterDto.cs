using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using InternetBank.ModelLayer.CustomValidations;

namespace InternetBank.ModelLayer.UserDtos
{
    public class RegisterDto
    {
        [Required]
        [PersianTextOnly]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [PersianTextOnly]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [ValidIranianNationalCode]
        public string NationalCode { get; set; } = string.Empty;
        [Required]
        [Range(18, int.MaxValue, ErrorMessage = "You must be at least 18 years old.")]
        public int Age { get; set; }
        [Required]
        [ValidIranianMobileNumber]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(20, ErrorMessage = "Password must not exceed 20 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}