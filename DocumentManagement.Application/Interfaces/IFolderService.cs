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
        Task<List<Folder_DTOs>> GetAllFolder();
        Task AddFolder(Folder_DTOs Folder, int currentUserId);
        Task UpdateFolder(string newName, int id, int currentUserId);
        Task DeleteFolder(int id, int currentUserId);
        Task<List<Folder_DTOs>> SearchFolder(string searchTerm);
    }
}
