using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFolderService
    {
        Task<List<Folder_DTOs>> GetAllAsync();
        Task<Folder_DTOs> GetByIdAsync(int Id);
        Task<Folder_DTOs> AddAsync(Folder_DTOs Folder);
        Task UpdateAsync(Folder_DTOs Folder, int Id);
        Task DeleteAsync(int Id);
    }
}
