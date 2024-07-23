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
        public async Task AddFolder(Folder_DTOs Folder)
        {
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
        /// Hàm xóa thư mục 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task DeleteFolder(int id)
        {
            var resuilt = await _dbContext.Folder.SingleOrDefaultAsync(p => p.Id == id);
            if (resuilt == null)
            {
                throw new Exception("ID không tồn tại");
            }
            else
            {
                _dbContext.Folder.Remove(resuilt);
                await _dbContext.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Hàm để lấy tất cả Folder theo quyền người dùng
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Folder_DTOs>> GetAllFolder(int user_id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Tim kiếm folder theo tên folder và email của người đã tạo thư mục ấy
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Folder_DTOs>> SearchFolder(string searchTerm)
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
        public async Task UpdateFolder(Folder_DTOs Folder, int id)
        {
            var existingFolder = await _dbContext.Folder.FirstOrDefaultAsync(p => p.Id == id);

            if (existingFolder == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy thư mục với ID {id}");
            }else
            {
                existingFolder.Name = Folder.Name;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
