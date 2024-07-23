using DocumentManagement.Application.DTOs;
using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFileService
    {
        Task <List<File_DTOs>> GetAllFile(int foldersId);
        Task AddFile(File_DTOs File);
        Task UpdateFile(File_DTOs File, int id);
        Task DeleteFile(int id);
        Task <File_DTOs> SearchFile(string searchTerm);
    }
}
