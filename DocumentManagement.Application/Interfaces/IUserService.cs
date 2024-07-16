using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> Signin(string email, string password);
        Task<bool> RegisterUserAsync(RegisterUserDto userDto);
    }
}
