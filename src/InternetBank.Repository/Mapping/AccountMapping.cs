using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.ModelLayer.AccountDtos;

namespace InternetBank.Repository.Mapping
{
    public static class AccountMapping
    {
        public static AccountDetailsDto ToAccountDetailsDto(this Account account)
        {
            return new AccountDetailsDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                CardNumber = account.CardNumber,
                CVV2 = account.CVV2,
                ExpireDate = account.ExpireDate,
                StaticPassword = account.StaticPassword,
                AccountType = account.AccountType.ToString()
            };
        }
        public static AccountDto ToAccountDto(this Account account)
        {
            return new AccountDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                CardNumber = account.CardNumber
            };
        }
        public static Account ToAccount(this CreateAccountDto createAccountDto)
        {
            return new Account
            {
                Amount = createAccountDto.Amount,
                AccountType = createAccountDto.AccountType
            };
        }
        public static AccountBalanceDto ToAccountBalanceDto(this Account account)
        {
            return new AccountBalanceDto
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Amount = account.Amount
            };
        }
    }
}