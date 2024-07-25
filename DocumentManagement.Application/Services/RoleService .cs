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

        public async Task<IEnumerable<Roles>> GetRolesAsync()
        {
            return await _dbContext.Role.ToListAsync();
        }

        public async Task<Roles> GetRoleByIdAsync(int id)
        {
            return await _dbContext.Role.FindAsync(id);
        }

        public async Task<Roles> CreateRoleAsync(Roles role)
        {
            _dbContext.Role.Add(role);
            await _dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<bool> UpdateRoleAsync(int id, Roles role)
        {
            if (id != role.Id)
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
                else
                {
                    throw;
                }
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

