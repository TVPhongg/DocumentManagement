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
        Task<IEnumerable<Roles>> GetRolesAsync();
        Task<Roles> GetRoleByIdAsync(int id);
        Task<Roles> CreateRoleAsync(Roles role);
        Task<bool> UpdateRoleAsync(int id, Roles role);
        Task<bool> DeleteRoleAsync(int id);
    }
}
