using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.DataLayer.Context;
using InternetBank.ModelLayer.AccountDtos;
using InternetBank.Repository.Interfaces;
using InternetBank.Repository.Mapping;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccountDto>> GetAllAccounts(ApplicationUser user)
        {
            var accounts = await _context.Accounts.Where(x => x.UserId == user.Id).Select(x => x.ToAccountDto()).ToListAsync();
            return accounts;
        }

        public async Task<AccountDetailsDto?> GetAccountById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                var accountDetailsDto = account.ToAccountDetailsDto();
                return accountDetailsDto;
            }
            return null;
        }

        public async Task<AccountDetailsDto?> CreateAccount(CreateAccountDto createAccountDto, ApplicationUser user)
        {
            var account = createAccountDto.ToAccount();
            account.AccountNumber = GenerateAccountNumber(user.CustomUserId, (int)account.AccountType);
            account.CardNumber = GenerateCardNumber();
            account.CVV2 = GenerateCVV2();
            account.ExpireDate = GenerateExpireDate();
            account.StaticPassword = GenerateStaticPassword();
            account.UserId = user.Id;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var accountDetails = await GetAccountById(account.AccountId);
            if (accountDetails != null)
            {
                return accountDetails;
            }
            return null;
        }
        public async Task<bool> ChangeStaticPassword(ChangePasswordDto changePasswordDto)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.StaticPassword == changePasswordDto.OldPassword
                                                                         && x.AccountId == changePasswordDto.AccountId);
            if (account != null)
            {
                account.StaticPassword = changePasswordDto.NewPassword;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<AccountBalanceDto?> Balance(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                return account.ToAccountBalanceDto();
            }
            return null;
        }
        public async Task<bool> BlockAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return false;
            }
            account.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UnblockAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return false;
            }
            account.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }
        private static string GenerateAccountNumber(int userId, int accountType)
        {
            Random random = new Random();
            string part1 = random.Next(10, 100).ToString();
            string part2;
            if (userId < 10)
            {
                part2 = $"00{userId}";
            }
            else if (userId >= 10 && userId < 100)
            {
                part2 = $"0{userId}";
            }
            else
            {
                part2 = userId.ToString();
            }
            string part3 = random.Next(1000, 10000).ToString();
            string part4 = accountType.ToString();
            return $"{part1}.{part2}{part3}.{part4}";
        }
        private static string GenerateCardNumber()
        {
            Random random = new Random();
            var cardNumber = new string[4];
            for (int i = 0; i < cardNumber.Length; i++)
            {
                cardNumber[i] = random.Next(1000, 10000).ToString();
            }
            return $"{cardNumber[0]} {cardNumber[1]} {cardNumber[2]} {cardNumber[3]}";
        }
        private static string GenerateCVV2()
        {
            string cvv2;
            Random random = new Random();
            cvv2 = random.Next(1000, 10000).ToString();
            return cvv2;
        }
        private static string GenerateExpireDate()
        {
            DateTime exipreDate = DateTime.Now.AddYears(5);
            string year = (exipreDate.Year % 100).ToString();
            string month = exipreDate.Month.ToString("D2");
            return $"{year}/{month}";
        }
        private static string GenerateStaticPassword()
        {
            string staticPassword;
            Random random = new Random();
            staticPassword = random.Next(100000, 1000000).ToString();
            return staticPassword;
        }
    }
}