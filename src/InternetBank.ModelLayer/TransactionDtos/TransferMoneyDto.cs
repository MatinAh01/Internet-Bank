using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.TransactionDtos
{
    public class TransferMoneyDto
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Otp { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}