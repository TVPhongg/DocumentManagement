using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface ISalaryService
    {
        Task<List<Salary_DTOs>> GetAllSalarysAsync();

        Task<Salary_DTOs> GetSalaryByIdAsync(int Id);

        Task UpdateSalaryAsync(UpdateSalary_DTOs salary, int Id);

        Task DeleteSalaryAsync(int Id);

        Task InsertSalaryAsync(UpdateSalary_DTOs salary, int Id);

        //Task CreateSalaryAsync(Salary_DTOs salary, int userId);

        //Task CalculateMonthlySalary(int userId);

    }
}
