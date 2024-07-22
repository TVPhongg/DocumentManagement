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

        public async Task DeleteFile(int id)
        {
            var reesuilt = await _dbContext.File.SingleOrDefaultAsync(x => x.Id == id);
            if (reesuilt != null)
            {
                _dbContext.File.Remove(reesuilt);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("ID không tồn tại");
            }
        }

        public async Task<List<File_DTOs>> GetAllFile(int foldersId)
        {
            var results = await _dbContext.File.Where(p => p.FoldersId == foldersId).ToListAsync();

            if (results == null || !results.Any())
            {
                throw new Exception("ID không tồn tại");
            }
            else
            {
                return results.Select(resuilt => new File_DTOs
                {
                    Id = resuilt.Id,
                    FoldersId = resuilt.FoldersId,
                    Name = resuilt.Name,
                    FilePath = resuilt.FilePath,
                    CreatedDate = resuilt.CreatedDate,
                    UserId = resuilt.UserId,
                    FileSize = resuilt.FileSize,
                }).ToList();
            }
        }


        public Task<File_DTOs> SearchFile(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFile(File_DTOs File, int id)
        {
            throw new NotImplementedException();
        }
    }
}
