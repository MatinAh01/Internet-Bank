using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;

namespace InternetBank.ModelLayer.AccountDtos
{
    public class AccountDetailsDto
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string CVV2 { get; set; } = string.Empty;
        public string ExpireDate { get; set; } = string.Empty;
        public string StaticPassword { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
    }
}