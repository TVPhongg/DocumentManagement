using DocumentManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFileService
    {
        Task <List<File_DTOs>> GetAllFile(int foldersId);
        Task AddFiles(File_DTOs fileDto, List<IFormFile> file);
        Task UpdateFile(string newName, int id, int currentUserId);
        Task DeleteFile(int id, int currentUserId);
        Task <File_DTOs> SearchFile(string searchTerm);
        Task ShareFile(List<FilePermissionDTOs> filePermissions);
    }
}
