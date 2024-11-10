using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetBank.DataLayer;
using InternetBank.ModelLayer.UserDtos;

namespace InternetBank.Repository.Mapping
{
    public static class UserMapping
    {
        public static ApplicationUser ToUSerFromRegisterDto(this RegisterDto registerDto)
        {
            return new ApplicationUser 
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                NationalCode = registerDto.NationalCode,
                PhoneNumber = registerDto.PhoneNumber,
                Age = registerDto.Age
            };
        }
    }
}