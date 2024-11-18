using DocumentManagement.Application.DTOs;
using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role_Dtos>> GetRolesAsync();
        Task<Role_Dtos> GetRoleByIdAsync(int id);
        Task<Role_Dtos> CreateRoleAsync(Role_Dtos role);
        Task<bool> UpdateRoleAsync(int id, Role_Dtos role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
