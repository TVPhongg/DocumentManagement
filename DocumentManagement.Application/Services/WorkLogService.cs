using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly MyDbContext _dbContext;

        public WorkLogService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task CreateWorkLogAsync(WorkLog_DTOs workLog)
        {
            var existingWorkLog = await _dbContext.WorkLog
                .FirstOrDefaultAsync(w => w.UserId == workLog.UserId && w.WorkDate.Date == workLog.WorkDate.Date);

            if (existingWorkLog != null)
            {
                existingWorkLog.HoursWorked = workLog.HoursWorked;
                _dbContext.WorkLog.Update(existingWorkLog);
            }
            else
            {
                var newWorkLog = new WorkLog
                {
                    HoursWorked = workLog.HoursWorked,
                    UserId = workLog.UserId,
                    WorkDate = workLog.WorkDate,
                };
                await _dbContext.WorkLog.AddAsync(newWorkLog);
            }
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteWorkLogAsync(int id)
        {
            var result = await _dbContext.WorkLog.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy giờ làm việc trong WorkLog cho nhân viên này");
            }
            _dbContext.Remove(id);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<WorkLog_DTOs>> GetWorkLogAsync()
        {
           var result = await _dbContext.WorkLog
               .Select(W => new WorkLog_DTOs
               {
                   Id = W.Id,
                   HoursWorked = W.HoursWorked,
                   UserId = W.UserId,
                   WorkDate = W.WorkDate,

               }).ToListAsync();
            return result;
        }

        public async Task<List<WorkLog_DTOs>> GetWorkLogsByUserIdAsync(int userId)
        {
            // Lấy danh sách WorkLog theo UserId
            var result = await _dbContext.WorkLog
                .Where(wkl => wkl.UserId == userId)
                .OrderBy(wkl => wkl.WorkDate)
                .ToListAsync();

            if (!result.Any())
            {
                throw new KeyNotFoundException("Không tìm thấy giờ làm việc cho nhân viên này.");
            }

            // Trả về danh sách WorkLog dưới dạng DTO
            return result.Select(wkl => new WorkLog_DTOs
            {
                Id = wkl.Id,
                HoursWorked = wkl.HoursWorked,
                UserId = wkl.UserId,
                WorkDate = wkl.WorkDate,
            }).ToList();
        }

        public async Task UpdateWorkLogAsync(WorkLog_DTOs workLog, int Id)
        {
           var result = await _dbContext.WorkLog.FirstOrDefaultAsync(w => w.Id == Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy giờ làm việc trong WorkLog cho nhân viên này");
            }    
            result.WorkDate = workLog.WorkDate;
            result.HoursWorked = workLog.HoursWorked;
        }     
    }
}
