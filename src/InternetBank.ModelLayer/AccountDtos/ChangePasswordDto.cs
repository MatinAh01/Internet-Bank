using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.AccountDtos
{
    public class ChangePasswordDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        [MinLength(6, ErrorMessage = "Password must be 6 digit number.")]
        [MaxLength(6, ErrorMessage = "Password must be 6 digit number.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Password must be a 6-digit number.")]
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}