using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore;
using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
    /// Hàm thêm mới thư mục
    /// </summary>
    /// <param name="Folder"></param>
    /// <returns></returns>
        public async Task AddFolder(Folder_DTOs Folder, int currentUserId)
        {
            var hasPermission = await _dbContext.FolderPermission.AnyAsync(p => p.UserId == currentUserId && p.Name == "Create");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }
            var newFolder = new Folders
            {
                Name = Folder.Name,
                FoldersLevel = Folder.FoldersLevel,
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
            // Tìm thư mục theo ID
            var folder = await _dbContext.Folder.SingleOrDefaultAsync(p => p.Id == id);
            if (folder == null)
            {
                throw new Exception($"Không tìm thấy thư mục với ID {id}");
            }

            // Kiểm tra quyền xóa thư mục trong bảng phân quyền
            var hasPermission = await _dbContext.FolderPermission.AnyAsync(p => p.FolderId == id && p.UserId == currentUserId && p.Name == "Delete");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }

            // Xóa thư mục
            _dbContext.Folder.Remove(folder);
            await _dbContext.SaveChangesAsync();
        }
    /// <summary>
    /// Hàm để lấy tất cả Folder theo quyền người dùng
    /// </summary>
    /// <param name="user_id"></param>
    /// <returns></returns>
        public async Task<List<Folder_DTOs>> GetAllFolder()
        {
            return await _dbContext.Folder
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
                        FoldersLevel= folder.FoldersLevel,
                    })
                .ToListAsync();
        }

    /// <summary> 
    /// Tim kiếm folder theo tên folder và email của người đã tạo thư mục ấy
    /// </summary>
    /// <param name="searchTerm"></param>
    /// <returns></returns>
        public async Task<List<Folder_DTOs>> SearchFolder(string searchTerm)
        {

            throw new NotImplementedException();
        }
    /// <summary>
    /// Hàm update tên Folder
    /// </summary>
    /// <param name="Folder"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
        public async Task UpdateFolder(string newName , int id, int currentUserId)
        {
            var folder = await _dbContext.Folder.SingleOrDefaultAsync(p => p.Id == id);                 
            if (folder == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thư mục với ID {id}");
            }
            var hasPermission = await _dbContext.FolderPermission.AnyAsync(p => p.UserId == currentUserId && p.Name == "Update");
            if (!hasPermission)
            {
                throw new Exception("Bạn không có quyền thực hiện hành động này.");
            }
            else
            {
                folder.Name = newName;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
