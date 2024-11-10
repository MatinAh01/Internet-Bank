using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.AccountDtos
{
    public class AccountBalanceDto
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }
}