using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.ModelLayer.AccountDtos;

namespace InternetBank.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAllAccounts(ApplicationUser user);
        Task<AccountDetailsDto?> GetAccountById(int id);
        Task<AccountDetailsDto?> CreateAccount(CreateAccountDto createAccountDto, ApplicationUser user);
        Task<bool> ChangeStaticPassword(ChangePasswordDto changePasswordDto);
        Task<AccountBalanceDto?> Balance(int id);
        Task<bool> BlockAccount(int id);
        Task<bool> UnblockAccount(int id);

    }
}