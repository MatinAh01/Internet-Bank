using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using InternetBank.DataLayer;
using InternetBank.DataLayer.Context;
using InternetBank.ModelLayer;
using InternetBank.ModelLayer.TransactionDtos;
using InternetBank.Repository.Interfaces;
using InternetBank.Repository.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TransactionRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<(bool maching, List<string> errors, string message)> SendOtp(ApplicationUser user, CreateTransactionDto createTransactionDto)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == user.Id &&
                                                                        a.CardNumber.Replace(" ", "") == createTransactionDto.CardNumber &&
                                                                        a.ExpireDate == createTransactionDto.ExpireDate &&
                                                                        a.CVV2 == createTransactionDto.Cvv2);


            var errors = new List<string>();
            if (account == null)
            {
                errors.Add("the input datas don't mach!");
            }
            else if (!account.IsActive)
            {
                errors.Add($"Account is not active.");
            }
            if (errors.Count != 0)
            {
                return (false, errors, string.Empty);
            }
            var sms = SendSMS(createTransactionDto.Amount, createTransactionDto.DestinationCardNumber, user);

            var transaction = createTransactionDto.ToTransaction(account.AccountId);
            transaction.Otp = sms[0];
            transaction.OtpExpireDate = DateTime.UtcNow.AddMinutes(2);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return (true, new List<string>(), sms[1]);
        }
        public async Task<(bool IsSuccess, List<string> errors)> TransferMoney(TransferMoneyDto transferMoneyDto, ApplicationUser user)
        {
            var errors = new List<string>();

            // Retrieve transaction and user accounts data in one query with necessary checks.
            var transaction = await _context.Transactions
                                            .Include(t => t.Account)
                                            .FirstOrDefaultAsync(t => t.Amount == transferMoneyDto.Amount &&
                                                                      t.Otp == transferMoneyDto.Otp);

            var userAccounts = await _userManager.Users
                                                 .Include(u => u.Accounts)
                                                 .FirstOrDefaultAsync(u => u.Id == user.Id);

            // Validation: Check if transaction exists
            if (transaction == null)
            {
                errors.Add("The transaction was not found.");
                return (false, errors);
            }

            // Validate the OTP value and expiration
            if (!ValidateOtp(transaction, transferMoneyDto.Otp, out string otpError))
            {
                errors.Add(otpError);
                transaction.Status = TransactionStatus.Unsuccess;
                transaction.StatusMessage = otpError;
                await _context.SaveChangesAsync();
                return (false, errors);
            }

            // Check if account has sufficient funds
            if (!HasSufficientFunds(transaction, transferMoneyDto.Amount, out string fundError))
            {
                errors.Add(fundError);
                transaction.Status = TransactionStatus.Unsuccess;
                transaction.StatusMessage = fundError;
                await _context.SaveChangesAsync();
                return (false, errors);
            }

            // Perform transfer operation
            await PerformTransfer(transaction, transferMoneyDto, userAccounts, errors);
            return errors.Count == 0 ? (true, errors) : (false, errors);
        }
        public async Task<List<TransactionReportDto>> Report(TransactionReportFilterDto reportFilterDto, ApplicationUser user)
        {
            // Get user's account IDs
            var userAccountIds = await _context.Accounts
                                                .Where(a => a.UserId == user.Id)
                                                .Select(a => a.AccountId)
                                                .ToListAsync();

            // Define query for filtering transactions
            IQueryable<Transaction> transactionQuery = _context.Transactions.Where(t => userAccountIds.Contains(t.AccountId));

            // Apply date filters if provided
            if (reportFilterDto.DateFrom.HasValue && reportFilterDto.DateTo.HasValue)
            {
                transactionQuery = transactionQuery.Where(t => t.Created >= reportFilterDto.DateFrom && t.Created <= reportFilterDto.DateTo);
            }
            else
            {
                // If no date range is provided, return the latest 5 transactions
                transactionQuery = transactionQuery.OrderByDescending(t => t.Created).Take(5);
                return await transactionQuery.Select(t => t.ToTransactionReportDto()).ToListAsync();
            }

            // Apply success status filter if specified
            if (reportFilterDto.IsSuccess)
            {
                transactionQuery = transactionQuery.Where(t => t.Status == TransactionStatus.Success);
            }
            else
            {
                transactionQuery = transactionQuery.Where(t => t.Status == TransactionStatus.Unsuccess);
            }

            // Execute the query and map results to DTOs
            var transactions = await transactionQuery
                                    .OrderByDescending(t => t.Created) // Ensure ordering for consistent results
                                    .Select(t => t.ToTransactionReportDto())
                                    .ToListAsync();

            return transactions;
        }

        private static bool ValidateOtp(Transaction transaction, string providedOtp, out string error)
        {
            if (transaction.Otp != providedOtp)
            {
                error = "The OTP was incorrect.";
                return false;
            }

            if (transaction.OtpExpireDate <= DateTime.UtcNow || transaction.Status == TransactionStatus.Success)
            {
                error = "The OTP has expired.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private static bool HasSufficientFunds(Transaction transaction, decimal transferAmount, out string error)
        {
            if (transaction.Account.Amount < transferAmount)
            {
                error = "The balance is insufficient.";
                return false;
            }

            error = string.Empty;
            return true;
        }

        private async Task PerformTransfer(Transaction transaction, TransferMoneyDto transferMoneyDto, ApplicationUser user, List<string> errors)
        {
            // Deduct the amount from sender's account
            transaction.Account.Amount -= transferMoneyDto.Amount;

            // Find destination account and update the balance
            var destinationAccount = user.Accounts.FirstOrDefault(a => a.CardNumber == transaction.DestinationCardNumber);
            if (destinationAccount != null)
            {
                destinationAccount.Amount += transferMoneyDto.Amount;
            }

            // Update transaction status to success and save changes
            transaction.Status = TransactionStatus.Success;
            transaction.Description = transferMoneyDto.Description;
            transaction.StatusMessage = transaction.Status.ToString();
            await _context.SaveChangesAsync();
        }

        private static string[] SendSMS(decimal amount, string destenationCardNumber, ApplicationUser user)
        {
            //var sender = "2000660110";
            var receptor = user.PhoneNumber;
            var api = new Kavenegar.KavenegarApi("384A366B367863754A3676614F566649766F57494879666E64614568727A50312F4B5733615130586262493D");

            string date = DateTime.Now.ToLongPersianDateString();
            string time = DateTime.Now.ToString("HH:mm");
            Random random = new Random();
            var otp = random.Next(10000, 100000).ToString();

            string message = $"مبلغ : {amount}\nتاریخ : {date}\nساعت : {time}\nشماره کارت : {destenationCardNumber}\nرمز پویا : {otp}";
            var sms = new string[] { otp, message };

            // api.Send(sender, receptor, message);
            return sms;
        }
    }
}