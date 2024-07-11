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
        Task<IEnumerable<Folder_DTOs>> GetAllFolder(int user_id);
        Task AddFolder(Folder_DTOs Folder);
        Task UpdateFolder(Folder_DTOs Folder, int id);
        Task DeleteFolder(int id);
        Task<IEnumerable<Folder_DTOs>> SearchFolder(string searchTerm);
    }
}
