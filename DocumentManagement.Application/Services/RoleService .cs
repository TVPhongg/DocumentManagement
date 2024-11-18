using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly MyDbContext _dbContext;
        public RoleService(MyDbContext dbContext)
        {
            _dbContext = dbContext;  
        }

        public async Task<IEnumerable<Role_Dtos>> GetRolesAsync()
        {
            var roles = await _dbContext.Role.ToListAsync();
            return roles.Select(role => new Role_Dtos
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
            }).ToList(); // Chuyển đổi thành danh sách
        }

        public async Task<Role_Dtos> GetRoleByIdAsync(int id)
        {
            var role = await _dbContext.Role.FindAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {id} not found.");
            }
            return new Role_Dtos
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
            };
        }
        public async Task<Role_Dtos> CreateRoleAsync(Role_Dtos roleDto)
        {
            if (roleDto == null)
            {
                throw new ArgumentNullException(nameof(roleDto), "Role cannot be null.");
            }

            // Ánh xạ từ Role_Dtos sang Roles
            var role = new Roles
            {
                RoleName = roleDto.RoleName,
                Description = roleDto.Description,

            };

            _dbContext.Role.Add(role);
            await _dbContext.SaveChangesAsync();
            return roleDto;
        }


        public async Task<bool> UpdateRoleAsync(int id, Role_Dtos role)
        {
            if (role == null || id != role.Id)
            {
                return false;
            }

            _dbContext.Entry(role).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return false;
                }
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _dbContext.Role.FindAsync(id);
            if (role == null)
            {
                return false;
            }

            _dbContext.Role.Remove(role);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool RoleExists(int id)
        {
            return _dbContext.Role.Any(e => e.Id == id);
        }
    }
}

