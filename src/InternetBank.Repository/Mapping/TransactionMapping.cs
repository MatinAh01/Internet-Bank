using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using InternetBank.DataLayer;
using InternetBank.ModelLayer;
using InternetBank.ModelLayer.TransactionDtos;
using Transaction = InternetBank.DataLayer.Transaction;
using TransactionStatus = InternetBank.DataLayer.TransactionStatus;

namespace InternetBank.Repository.Mapping
{
    public static class TransactionMapping
    {
        public static Transaction ToTransaction(this CreateTransactionDto createDto, int accountId)
        {
            return new Transaction
            {
                AccountId = accountId,
                Amount = createDto.Amount,
                Status = TransactionStatus.Pending,
                Created = DateTime.UtcNow,
                DestinationCardNumber = FomattedCardNumber(createDto.DestinationCardNumber)
            };
        }
        public static TransactionReportDto ToTransactionReportDto(this Transaction transaction)
        {
            return new TransactionReportDto
            {
                Amount = transaction.Amount,
                TransactionStatus = transaction.Status.ToString(),
                CreatedAt = transaction.Created,
                AccountId = transaction.AccountId,
                DestinationCardNumber = transaction.DestinationCardNumber,
                Description = transaction.Description
            };
        }
        private static string FomattedCardNumber(string destinationCardNumber)
        {
            var formattedCardNumber = Regex.Replace(destinationCardNumber, ".{4}(?=.{4})", "$0 ");
            return formattedCardNumber;
        }
    }
}