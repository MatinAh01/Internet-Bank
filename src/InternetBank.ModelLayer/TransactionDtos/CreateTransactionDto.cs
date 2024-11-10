using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer
{
    public class CreateTransactionDto
    {
        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "cardnumber must be a 16-digit number.")]
        public string CardNumber { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "cvv2 must be a 4-digit number.")]
        public string Cvv2 { get; set; } = string.Empty;
        [Required]
        public string ExpireDate { get; set; } = string.Empty;
        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "cardnumber must be a 16-digit number.")]
        public string DestinationCardNumber { get; set; } = string.Empty;
        [Required]
        [Range(1000, 5000000, ErrorMessage = "حداقل مقدار قابل انتقال 1000 تومان و حداکثر 5000000 تومان می‌باشد")]
        public decimal Amount { get; set; }
    }
}