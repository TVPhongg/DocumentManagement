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
            var hasPermission = await _dbContext.FolderPermission.AnyAsync(p => p.UserId == Folder.UserId && p.Name == "Create");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }
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

            bool hasDeletePermission = folder.UserId == currentUserId || await _dbContext.FolderPermission.AnyAsync(fp => fp.FolderId == id && fp.UserId == currentUserId && fp.Name == "Delete");

            if (!hasDeletePermission)
            {

                throw new UnauthorizedAccessException("");

            }

            // Xóa quyền và thư mục
            _dbContext.FolderPermission.RemoveRange(_dbContext.FolderPermission.Where(fp => fp.FolderId == id));
            _dbContext.Folder.Remove(folder);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Hàm để lấy tất cả Folder theo quyền người dùng
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<Folder_DTOs>> GetAllFolders(int currentUserId)
        {
            // Truy vấn để lấy tất cả các thư mục mà người dùng tạo ra hoặc được chia sẻ với người dùng
            var foldersQuery = _dbContext.Folder
                .Where(f => f.UserId == currentUserId ||
                            _dbContext.FolderPermission.Any(fp => fp.FolderId == f.Id && fp.UserId == currentUserId))
                .Select(f => new Folder_DTOs
                {
                    Id = f.Id,
                    Name = f.Name,
                    CreateDate = f.CreateDate,
                    UserId = f.UserId,
                    UserName = _dbContext.User.Where(u => u.Id == f.UserId)
                                              .Select(u => u.FirstName + " " + u.LastName)
                                              .FirstOrDefault()
                });

            var folders = await foldersQuery.ToListAsync();

            // Truy vấn để lấy tất cả các file được chia sẻ tới người dùng, bao gồm cả file trong thư mục của người dùng
            var sharedFilesQuery = _dbContext.File
                .Join(_dbContext.FilePermission,
                      file => file.Id,
                      fp => fp.FileId,
                      (file, fp) => new { file, fp })
                .Where(joined => joined.fp.UserId == currentUserId)
                .Join(_dbContext.User,
                      joined => joined.file.UserId,
                      user => user.Id,
                      (joined, user) => new
                      {
                          joined.file.Id,
                          joined.file.Name,
                          joined.file.FilePath,
                          joined.file.CreatedDate,
                          UserName = user.FirstName + " " + user.LastName,
                          joined.file.FoldersId
                      });

            var sharedFiles = await sharedFilesQuery.ToListAsync();

            var fileDTOs = sharedFiles.Select(f => new File_DTOs
            {
                Id = f.Id,
                Name = f.Name,
                FilePath = f.FilePath,
                CreatedDate = f.CreatedDate,
                UserName = f.UserName,
                FoldersId = f.FoldersId
            }).ToList();

            return folders;
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
        public async Task UpdateFolder(string newName, int id, int currentUserId)
        {
            var folder = await _dbContext.Folder.SingleOrDefaultAsync(p => p.Id == id);
            bool folderExists = await _dbContext.Folder.AnyAsync(f => f.Name == newName);

            if (folderExists)
            {
                throw new Exception("Tên thư mục đã tồn tại.");
            }

            bool canEdit = folder.UserId == currentUserId || await _dbContext.FolderPermission.AnyAsync(fp => fp.FolderId == id && fp.UserId == currentUserId && fp.Name == "Edit");

            if (!canEdit)
            {
                throw new UnauthorizedAccessException();
            }

            folder.Name = newName;
            await _dbContext.SaveChangesAsync();
        }


        public async Task ShareFolder(List<FolderPermissionDTOs> folderPermissions)
        {
            // Danh sách để lưu các quyền mới cần thêm
            var permissionsToAdd = new List<FolderPermissions>();

            foreach (var folderPermission in folderPermissions)
            {
                var folder = await _dbContext.Folder
                    .SingleOrDefaultAsync(p => p.Id == folderPermission.FolderId);

                // Kiểm tra xem quyền đã tồn tại chưa
                var permissionExists = await _dbContext.FolderPermission
                    .AnyAsync(fp => fp.FolderId == folderPermission.FolderId
                                    && fp.UserId == folderPermission.UserId
                                    && fp.Name == folderPermission.Name);

                if (!permissionExists)
                {
                    var share = new FolderPermissions
                    {
                        FolderId = folderPermission.FolderId,
                        UserId = folderPermission.UserId,
                        Name = folderPermission.Name
                    };

                    permissionsToAdd.Add(share);
                }
            }

            if (permissionsToAdd.Count > 0)
            {
                await _dbContext.FolderPermission.AddRangeAsync(permissionsToAdd);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
