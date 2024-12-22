using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class SalaryService : ISalaryService
    {
        private MyDbContext _dbContext;

        public SalaryService(MyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task DeleteSalaryAsync(int Id)
        {
            var salary = await _dbContext.Salary.FirstOrDefaultAsync(s=>s.Id == Id);
            if (salary == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy bảng lương");
            }
             _dbContext.Salary.Remove(salary);
              await _dbContext.SaveChangesAsync();

        }

        public async Task<List<Salary_DTOs>> GetAllSalarysAsync()
        {
            var result = await _dbContext.Salary
                .Select(s=>new Salary_DTOs
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    BaseSalary = s.BaseSalary,
                    Allowances = s.Allowances,
                    Bonus = s.Bonus,
                    TotalSalary  =s.TotalSalary,
                    ActualWorkingHours = s.ActualWorkingHours,
                    Year = s.Year,
                    Month = s.Month,
                }).ToListAsync();
            return result;
        }

        public async Task<Salary_DTOs> GetSalaryByIdAsync(int id)
        {
            var result = await _dbContext.Salary.FirstOrDefaultAsync(s => s.Id == id);

            if (result == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy bảng lương");
            }

            return new Salary_DTOs
            {
                Id = result.Id,
                UserId = result.UserId,
                BaseSalary = result.BaseSalary,
                Allowances = result.Allowances,
                Bonus = result.Bonus,
                TotalSalary = result.TotalSalary,
                ActualWorkingHours = result.ActualWorkingHours,
                Year = result.Year,
                Month = result.Month,           
            };
        }

        public async Task UpdateSalaryAsync(UpdateSalary_DTOs salary, int Id)
        {
            // Tìm bản ghi Salary dựa trên ID
            var result = await _dbContext.Salary.FirstOrDefaultAsync(s => s.Id == Id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy bảng lương");
            }

            // Lấy tổng số giờ làm việc từ WorkLog trong tháng và năm hiện tại
             var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var totalHoursWorked = await _dbContext.WorkLog
                .Where(w => w.UserId == salary.UserId)
                .SumAsync(w => w.HoursWorked);

            if (totalHoursWorked <= 0)
            {
                throw new Exception("Không tìm thấy giờ làm việc trong WorkLog cho nhân viên này.");
            }

            // Tính TotalSalary
            var totalSalary = Math.Floor((result.BaseSalary + result.Allowances + result.Bonus) / result.StandardWorkingHours) * totalHoursWorked;

            salary.BaseSalary = result.BaseSalary;
            salary.Allowances = result.Allowances;
            salary.Bonus = result.Bonus;
            result.ActualWorkingHours = totalHoursWorked; 
            result.TotalSalary = Math.Floor(totalSalary); 
            result.Month = currentMonth;
            result.Year = currentYear;

            // Cập nhật vào cơ sở dữ liệu
            _dbContext.Salary.Update(result);
            await _dbContext.SaveChangesAsync();
        }
        public async Task InsertSalaryAsync(UpdateSalary_DTOs salary, int Id)
        {
            // Tìm bản ghi Salary dựa trên ID
            var existingSalary = await _dbContext.Salary.FirstOrDefaultAsync(s => s.Id == Id);
            if (existingSalary == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy bảng lương");
            }

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var totalHoursWorked = await _dbContext.WorkLog
                .Where(w => w.UserId == salary.UserId && w.WorkDate.Month == currentMonth)
                .SumAsync(w => w.HoursWorked);

            if (totalHoursWorked <= 0)
            {
                throw new Exception("Không tìm thấy giờ làm việc trong WorkLog cho nhân viên này.");
            }

            var totalSalary = Math.Floor((existingSalary.BaseSalary + existingSalary.Allowances + existingSalary.Bonus)
                                          / existingSalary.StandardWorkingHours) * totalHoursWorked;

            var newSalary = new Salary
            {
                UserId = salary.UserId,
                BaseSalary = existingSalary.BaseSalary, 
                Allowances = existingSalary.Allowances, 
                Bonus = existingSalary.Bonus, 
                StandardWorkingHours = existingSalary.StandardWorkingHours, 
                ActualWorkingHours = totalHoursWorked, 
                TotalSalary = Math.Floor(totalSalary), 
                Month = currentMonth, 
                Year = currentYear, 
            };

            await _dbContext.Salary.AddAsync(newSalary);
            await _dbContext.SaveChangesAsync();
        }
    }
}
