using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.DataLayer
{
    public class Account
    {
        public int AccountId { get; set; }
        public string? UserId { get; set; }
        public AccountTypes AccountType { get; set; }
        public decimal Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CVV2 { get; set; } = string.Empty;
        public string ExpireDate { get; set; } = string.Empty;
        public string StaticPassword { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ApplicationUser? User { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}