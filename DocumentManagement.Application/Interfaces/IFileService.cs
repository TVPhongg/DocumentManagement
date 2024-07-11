using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<File_DTOs>> GetAllFile(int user_id);
        Task AddFile(File_DTOs File);
        Task UpdateFile(File_DTOs File, int id);
        Task DeleteFile(int id);
        Task<IEnumerable<File_DTOs>> SearchFile(string searchTerm);
    }
}
