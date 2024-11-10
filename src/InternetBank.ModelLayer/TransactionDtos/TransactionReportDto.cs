using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBank.ModelLayer.TransactionDtos
{
    public class TransactionReportDto
    {
        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AccountId { get; set; }
        public string DestinationCardNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}