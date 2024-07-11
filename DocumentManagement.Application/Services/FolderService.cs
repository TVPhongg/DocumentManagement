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
                Folders_name = Folder.Folders_name,
                Folders_lever = Folder.Folders_lever,
                Created_date = DateTime.Now,
                User_id = Folder.User_id,

            };
            await _dbContext.AddAsync(newFolder);
            await _dbContext.SaveChangesAsync();
            var newLog = new Logs
            {
                Foleders_id = newFolder.Id, 
                File_id = 0, 
                Activity = "Thêm thư mục",
                Created_date = DateTime.Now,
                User_id = Folder.User_id,
                Request_id = 0, 
                ApprovalSteps_id = 0 
            };

            await _dbContext.AddAsync(newLog);
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
            var newLog = new Logs
            {
                Foleders_id = resuilt.Id,
                File_id = 0,
                Activity = "xóa thư mục",
                Created_date = DateTime.Now,
                User_id = resuilt.User_id,
                Request_id = 0,
                ApprovalSteps_id = 0
            };

            await _dbContext.AddAsync(newLog);
            await _dbContext.SaveChangesAsync();
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
        /// Tim kiếm folder theo tên folder và email cảu người đã tạo thư mục ấy
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Folder_DTOs>> SearchFolder(string searchTerm)
        {
            var folders = await _dbContext.Folder
                   .Include(f => f.User) 
                   .Where(f => f.Folders_name.Contains(searchTerm) || f.User.Email.Contains(searchTerm))
                   .ToListAsync();

            var folderDTOs = folders.Select(f => new Folder_DTOs
            {
                Id = f.Id,
                Folders_name = f.Folders_name,
                Created_date = f.Created_date,
                User_id = f.User_id,
                Folders_lever = f.Folders_lever
            });

            return folderDTOs;
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
                existingFolder.Folders_name = Folder.Folders_name;
                await _dbContext.SaveChangesAsync();
            }
            var newLog = new Logs
            {
                Foleders_id = existingFolder.Id,
                File_id = 0,
                Activity = "Update tên thư mục",
                Created_date = DateTime.Now,
                User_id = existingFolder.User_id,
                Request_id = 0,
                ApprovalSteps_id = 0
            };

            await _dbContext.AddAsync(newLog);
            await _dbContext.SaveChangesAsync();
        }

    }
}
