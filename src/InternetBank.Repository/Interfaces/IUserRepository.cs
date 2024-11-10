using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.ModelLayer.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace InternetBank.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> Register(RegisterDto registerDto);
        Task<string?> Login(LoginDto loginDto);
    }
}