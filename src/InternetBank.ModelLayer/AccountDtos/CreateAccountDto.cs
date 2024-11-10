using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using InternetBank.DataLayer;

namespace InternetBank.ModelLayer.AccountDtos
{
    public class CreateAccountDto
    {
        [Required]
        [Range(10000, double.MaxValue, ErrorMessage = "حداقل موجودی برای افتتاح حساب 10000 تومان می‌باشد")]
        public decimal Amount { get; set; }
        [Required]
        public AccountTypes AccountType { get; set; }
    }
}