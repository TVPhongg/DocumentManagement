﻿using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> GenerateToken(Login_DTOs login_DTOs);
        Task<string> Login(string email, string password);
        Task<bool> RegisterUserAsync(RegisterUserDto userDto);
        //Task<Users> GetUserById(int userId);
        //Task<bool> HasPermissionAsync(int userId, string permissionName);
    }
}
