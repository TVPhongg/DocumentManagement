using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore;
using DocumentManagement.Domain.Entities;
using System.IO.Compression;

namespace DocumentManagement.Application.Services
{
    public class FolderService : IFolderService
    {
        public readonly MyDbContext _dbContext;
        public FolderService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Folder"></param>
      /// <returns></returns>
      /// <exception cref="Exception"></exception>
        public async Task AddFolder(Folder_DTOs Folder)
        {
            var folderExists = await _dbContext.Folder.AnyAsync(f => f.Name == Folder.Name);
            if (folderExists)
            {
                throw new Exception("Tên thư mục đã tồn tại.");
            }
            var newFolder = new Folders
            {
                Name = Folder.Name,
                CreateDate = DateTime.Now,
                UserId = Folder.UserId,
            };
            await _dbContext.AddAsync(newFolder);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FolderPermission"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteFolder(int id, int currentUserId)
        {
            var folder = await _dbContext.Folder
                .SingleOrDefaultAsync(f => f.Id == id);

            if (folder == null)
            {
                throw new ArgumentException("Thư mục không tồn tại.");
            }

            _dbContext.Folder.Remove(folder);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Hàm để lấy tất cả Folder theo quyền người dùng
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<Folder_DTOs>> GetAllFolders()
        {
            var Folders = await _dbContext.Folder
                .Select(f => new Folder_DTOs
                {
                    Id = f.Id,
                    Name = f.Name,
                    CreateDate = f.CreateDate,
                    UserId = f.UserId,

                })
                .ToListAsync();
            return Folders;
        }


        /// <summary> 
        /// Tim kiếm folder theo tên folder và email của người đã tạo thư mục ấy
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<List<Folder_DTOs>> SearchFolder(string searchTerm)
        {
            return await _dbContext.Folder
                .Where(folder => folder.Name.Contains(searchTerm))
                .Join(_dbContext.User,
                    folder => folder.UserId,
                    user => user.Id,
                    (folder, user) => new Folder_DTOs
                    {
                        Id = folder.Id,
                        Name = folder.Name,
                        CreateDate = folder.CreateDate,
                        UserName = user.FirstName + " " + user.LastName,
                        UserId = folder.UserId,
                    })
                .ToListAsync();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task UpdateFolder(string newName, int id)
        {
            var folder = await _dbContext.Folder.SingleOrDefaultAsync(p => p.Id == id);

            if (folder == null)
            {
                throw new Exception("Tên thư mục đã tồn tại.");
            }           

            folder.Name = newName;
            await _dbContext.SaveChangesAsync();
        }

        //public async Task ShareFolder(List<FolderPermissionDTOs> folderPermissions)
        //{
        //    // Danh sách để lưu các quyền mới cần thêm
        //    var permissionsToAdd = new List<FolderPermissions>();

        //    foreach (var folderPermission in folderPermissions)
        //    {
        //        var folder = await _dbContext.Folder
        //            .SingleOrDefaultAsync(p => p.Id == folderPermission.FolderId);

        //        // Kiểm tra xem quyền đã tồn tại chưa
        //        var permissionExists = await _dbContext.FolderPermission
        //            .AnyAsync(fp => fp.FolderId == folderPermission.FolderId
        //                            && fp.UserId == folderPermission.UserId
        //                            && fp.Name == folderPermission.Name);

        //        if (!permissionExists)
        //        {
        //            var share = new FolderPermissions
        //            {
        //                FolderId = folderPermission.FolderId,
        //                UserId = folderPermission.UserId,
        //                Name = folderPermission.Name
        //            };

        //            permissionsToAdd.Add(share);
        //        }
        //    }

        //    if (permissionsToAdd.Count > 0)
        //    {
        //        await _dbContext.FolderPermission.AddRangeAsync(permissionsToAdd);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //}
    }
}
