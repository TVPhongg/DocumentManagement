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
        Task<string> GenerateToken(Login_DTOs login_DTOs);
        Task<string> Login(string email, string password);
        Task<bool> RegisterUserAsync(RegisterUserDto userDto);
        Task<bool> DeleteUser(int Id);
        Task<bool> UpdateUser(int Id, RegisterUserDto userDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<PagedResult<UserDto>> GetUsersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<UserDto>> SearchUsersByEmailAsync(string emailSearchTerm);


    }
}
