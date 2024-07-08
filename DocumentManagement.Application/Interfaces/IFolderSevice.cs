using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFolderSevice
    {
        Task<List<FolderDTOs>> GetAllAsync();
        Task<FolderDTOs> GetByIdAsync(int id);
        Task<FolderDTOs> AddAsync(FolderDTOs FolderDTOs);
        Task UpdateAsync(FolderDTOs FolderDTOs);
        Task DeleteAsync(int id);
    }
}
