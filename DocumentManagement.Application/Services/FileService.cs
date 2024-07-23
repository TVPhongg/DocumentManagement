 using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Application.Services
{
    public class FileService : IFileService
    {
        private readonly MyDbContext _dbContext;

        public FileService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task AddFile( File_DTOs fileDto, IFormFile file)
        {
            var hasPermission = await _dbContext.FilePermission
                .AnyAsync(p => p.UserId == fileDto.UserId && p.Name == "Create");
            if (!hasPermission)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện hành động này.");
            }

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File không hợp lệ.");
            }

            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{DateTime.Now:yyyyMMddssffff}{fileExtension}";
            string filePath = Path.Combine(@"E:\DocumentManagement\DocumentManagement\File", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

            fileDto.Name = file.FileName;
            fileDto.FilePath = filePath; 
            fileDto.FileSize = file.Length; 

            var newFile = new Files
            {
                FoldersId = fileDto.FoldersId,
                Name = fileDto.Name,
                FilePath = fileDto.FilePath,
                CreatedDate = DateTime.Now,
                UserId = fileDto.UserId,
                FileSize = fileDto.FileSize,
            };

            await _dbContext.AddAsync(newFile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFile(int id , int currentUserId)
        {
            var file = await _dbContext.File.SingleOrDefaultAsync(p => p.Id == id);
            if (file == null)
            {
                throw new Exception("Không tìm thấy file với ID ");
            }
            var hasPermission = await _dbContext.FilePermission.AnyAsync(p => p.FileId == id && p.UserId == currentUserId && p.Name == "Delete");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }
            _dbContext.File.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<File_DTOs>> GetAllFile(int foldersId)
        {
            var results = await _dbContext.File
                                          .Where(p => p.FoldersId == foldersId)
                                          .Join(_dbContext.User,
                                                file => file.UserId,
                                                user => user.Id,
                                                (file, user) => new File_DTOs
                                                {
                                                    Id = file.Id,
                                                    Name = file.Name,
                                                   // FilePath = file.FilePath,
                                                    CreatedDate = file.CreatedDate,
                                                    UserName = user.FirstName + " " + user.LastName,
                                                    UserId = file.UserId,
                                                    FileSize = file.FileSize,                                         
                                                    FoldersId = file.FoldersId
                                                })
                                          .ToListAsync();

            return results ?? new List<File_DTOs>();
        }



        public Task<File_DTOs> SearchFile(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateFile(string newName, int id, int currentUserId)
        {
            var file = await _dbContext.File.SingleOrDefaultAsync(p => p.Id == id);
            if (file == null)
            {
                throw new KeyNotFoundException("Không tìm thấy file nào");
            }
            var hasPermission = await _dbContext.FilePermission.AnyAsync(p => p.UserId == currentUserId && p.Name == "Update");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }
            else
            {
                file.Name = newName;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
