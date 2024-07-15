using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class FileService : IFileService
    {
        private readonly MyDbContext _dbContext;

        public FileService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public Task AddFile(File_DTOs File)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File_DTOs>> GetAllFile(int user_id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<File_DTOs>> SearchFile(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFile(File_DTOs File, int id)
        {
            throw new NotImplementedException();
        }
    }
}
