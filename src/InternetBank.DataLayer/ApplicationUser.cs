using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public int CustomUserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public int Age { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}