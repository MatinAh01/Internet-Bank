using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.ModelLayer;
using InternetBank.ModelLayer.TransactionDtos;

namespace InternetBank.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<(bool maching, List<string> errors, string message)> SendOtp(ApplicationUser user, CreateTransactionDto createTransactionDto);
        Task<(bool IsSuccess, List<string> errors)> TransferMoney(TransferMoneyDto transferMoneyDto, ApplicationUser user);
        Task<List<TransactionReportDto>> Report(TransactionReportFilterDto reportFilterDto, ApplicationUser user);
    }
}