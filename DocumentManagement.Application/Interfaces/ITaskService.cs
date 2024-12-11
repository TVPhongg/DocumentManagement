using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<List<Task_DTOs>> GetTasksAsync();

        Task<Task_DTOs> GetTaskByIdAsync(int Id);

        Task UpdateTaskAsync(Task_DTOs task, int Id);

        Task DeleteTaskAsync(int Id);

        Task CreateTaskAsync(Task_DTOs task);

        Task <List<Task_DTOs>> GetTasksByProjectAsync(int ProjectId);
    } 
}
