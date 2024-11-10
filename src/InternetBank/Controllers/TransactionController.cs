using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.Extensions;
using InternetBank.ModelLayer;
using InternetBank.ModelLayer.TransactionDtos;
using InternetBank.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.Controllers
{
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionController(ILogger<AccountController> logger ,UserManager<ApplicationUser> userManager,
                                     ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _transactionRepository = transactionRepository;
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromQuery]CreateTransactionDto model)
        {
            var userName = User.GetUsername();
            if (userName == null)
            {
                return NotFound();
            }
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return NotFound();
            }
            var result = await _transactionRepository.SendOtp(appUser, model);
            if (result.maching)
            {
                return Ok(result.message);
            }
            return BadRequest(new { Errors = result.errors });
        }
        [HttpPut("transfer-money")]
        public async Task<IActionResult> TransferMoney([FromQuery]TransferMoneyDto model)
        {
            var userName = User.GetUsername();
            if (userName == null)
            {
                return NotFound();
            }
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return NotFound();
            }
            var result = await _transactionRepository.TransferMoney(model, appUser);
            if (result.IsSuccess)
            {
                return Ok("the transaction peformed successfully");
            }
            return BadRequest(new { Errors = result.errors});
        }
        [HttpGet("report")]
        public async Task<IActionResult> Report([FromQuery] TransactionReportFilterDto model)
        {
            var userName = User.GetUsername();
            if (userName == null)
            {
                return NotFound();
            }
            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null)
            {
                return NotFound();
            }
            var result = await _transactionRepository.Report(model, appUser);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}