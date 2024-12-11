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
        public async Task AddFiles(File_DTOs fileDto, List<IFormFile> files)
        {
            long totalSize = files.Sum(f => f.Length);
            const long maxUploadSize = 500 * 1024 * 1024; // 500MB in bytes
            if (totalSize > maxUploadSize)
            {
                throw new ArgumentException("Tổng dung lượng của các file không được vượt quá 500MB.");
            }

            var newFiles = new List<Files>();

            foreach (var file in files)
            {
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

                var newFile = new Files
                {
                    FoldersId = fileDto.FoldersId,
                    Name = file.FileName,
                    FilePath = filePath,
                    CreatedDate = DateTime.Now,
                    UserId = fileDto.UserId,
                    FileSize = file.Length,
                };

                newFiles.Add(newFile);
            }

            await _dbContext.AddRangeAsync(newFiles);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteFile(int id)
        {
            var file = await _dbContext.File.SingleOrDefaultAsync(p => p.Id == id);
            if (file == null)
            {
                throw new Exception("Không tìm thấy file");
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
                                                    CreatedDate = file.CreatedDate,
                                                    UserName = user.FirstName + " " + user.LastName,
                                                    UserId = file.UserId,
                                                    FileSize = (decimal)file.FileSize / (1024 * 1024),
                                                    FoldersId = file.FoldersId
                                                })
                                          .ToListAsync();

            return results ?? new List<File_DTOs>();
        }
        public async Task<List<File_DTOs>> SearchFile(string searchTerm)
        {
            return await _dbContext.File
                .Where(file => file.Name.Contains(searchTerm))
                .Join(_dbContext.User,
                    file => file.UserId,
                    user => user.Id,
                    (file, user) => new File_DTOs
                    {
                        Id = file.Id,
                        Name = file.Name,
                        CreatedDate = file.CreatedDate,
                        UserName = user.FirstName + " " + user.LastName,
                        UserId = file.UserId,
                    })
                .ToListAsync();
        }

        /* public async Task ShareFile(List<FilePermissionDTOs> filePermissions)
         {
             var permissionsToAdd = new List<FilePermissions>();

             foreach (var filePermission in filePermissions)
             {
                 // Lấy thông tin về tệp
                 var file = await _dbContext.File
                     .SingleOrDefaultAsync(p => p.Id == filePermission.FileId);

                 if (file == null)
                 {
                     throw new ArgumentException("Tệp không tồn tại.");
                 }

                 // Kiểm tra xem quyền đã tồn tại trên file chưa
                 var permissionExists = await _dbContext.FilePermission
                     .AnyAsync(fp => fp.FileId == filePermission.FileId
                                     && fp.UserId == filePermission.UserId
                                     && fp.Name == filePermission.Name);

                 if (!permissionExists)
                 {
                     var newPermission = new FilePermissions
                     {
                         FileId = filePermission.FileId,
                         UserId = filePermission.UserId,
                         Name = filePermission.Name
                     };

                     permissionsToAdd.Add(newPermission);
                 }
             }

             if (permissionsToAdd.Count > 0)
             {
                 await _dbContext.FilePermission.AddRangeAsync(permissionsToAdd);
                 await _dbContext.SaveChangesAsync();
             }
         }*/

        public async Task UpdateFile(string newName, int id)
        {
            var file = await _dbContext.File.SingleOrDefaultAsync(p => p.Id == id);

            if (file == null)
            {
                throw new ArgumentException("Tệp không tồn tại.");
            }   
            
            file.Name = newName;
            await _dbContext.SaveChangesAsync();

        }

    }
}
