using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.Extensions;
using InternetBank.ModelLayer.AccountDtos;
using InternetBank.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternetBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        public AccountController(ILogger<AccountController> logger ,UserManager<ApplicationUser> userManager,
                                     IAccountRepository accountRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _accountRepository = accountRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
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
            var accounts = await _accountRepository.GetAllAccounts(appUser);
            return Ok(accounts);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAccountById([FromRoute] int id)
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
            var account = await _accountRepository.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromQuery] CreateAccountDto model)
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
            var account = await _accountRepository.CreateAccount(model, appUser);
            if (account == null)
            {
                return BadRequest();
            }
            return Ok(account);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangeStaticPassword([FromQuery] ChangePasswordDto model)
        {
            var result = await _accountRepository.ChangeStaticPassword(model);
            if (result)
            {
                return Ok("the static password changes successfully");
            }
            return BadRequest();
        }
        [HttpGet("balance/{accountId:int}")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var accountBalance = await _accountRepository.Balance(accountId);
            if (accountBalance == null)
            {
                return BadRequest();
            }
            return Ok(accountBalance);
        }
        [HttpPut("block/{accountId:int}")]
        public async Task<IActionResult> BlockAccount(int accountId)
        {
            var result = await _accountRepository.BlockAccount(accountId);
            if (result)
            {
                return Ok("this account has been blocked");
            }
            return NotFound();
        }
        [HttpPut("unblock/{accountId:int}")]
        public async Task<IActionResult> UnblockAccount(int accountId)
        {
            var result = await _accountRepository.UnblockAccount(accountId);
            if (result)
            {
                return Ok("this account has been active");
            }
            return NotFound();
        }
    }
}