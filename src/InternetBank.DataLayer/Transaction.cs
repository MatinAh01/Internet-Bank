using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.DataLayer
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DestinationCardNumber { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public DateTime OtpExpireDate { get; set; }
        public Account? Account { get; set; }
    }
}