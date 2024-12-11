using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly MyDbContext _dbContext;
        private readonly EmailService _emailService;

        public TaskService( MyDbContext dbContext, EmailService emailService) 
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task CreateTaskAsync(Task_DTOs task)
        {
            try 
            {
                var newTask = new Tasks
                {
                    TaskName = task.TaskName,
                    StartDate = task.StartDate,
                    AssignedTo = task.AssignedTo,
                    Description = task.Description,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    ProjectId = task.ProjectId,
                    Status = task.Status,
                };

                // Lưu task vào cơ sở dữ liệu
                await _dbContext.Task.AddAsync(newTask);
                await _dbContext.SaveChangesAsync();

                // Lấy thông tin email của người dùng được giao công việc
                var assignedUser = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == task.AssignedTo);
                if (assignedUser == null || string.IsNullOrEmpty(assignedUser.Email))
                {
                    throw new Exception("Không tìm thấy người dùng được giao công việc hoặc người dùng không có email.");
                }

                // Gửi email thông báo
                await _emailService.SendEmail(new SendEmailDTOs
                {
                    ToEmail = assignedUser.Email,
                    Subject = "Thông báo: Công việc mới được giao",
                    Body = $@"
                            <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                                <h2 style='color: #5cb85c;'>Công việc mới được giao</h2>
                                <p>Chào <b>{assignedUser.FirstName} {assignedUser.LastName}</b>,</p>
                                <p>Bạn đã được giao một công việc mới:</p>
                                <ul>
                                    <li><strong>Tên công việc:</strong> {task.TaskName}</li>
                                    <li><strong>Mô tả:</strong> {task.Description}</li>
                                </ul>
                                <p>Vui lòng đăng nhập hệ thống để biết thêm chi tiết và cập nhật trạng thái công việc:</p>
                                <p>
                                    <a href='https://your-project-management-system.com/tasks/{task.Id}' 
                                       style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                       Xem chi tiết công việc
                                    </a>
                                </p>
                                <p>Trân trọng,</p>
                                <p><b>Bộ phận quản lý dự án</b></p>
                                <hr style='border: none; border-top: 1px solid #ddd;' />
                                <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                            </div>"
                });

            }
            catch
            {
                throw;
            }
        }


        public async Task DeleteTaskAsync(int Id)
        {
            try
            {
                var task = await _dbContext.Task.FirstOrDefaultAsync(x => x.Id == Id);
                if (task == null)
                {
                    throw new("Không tìm thấy task");
                }
                _dbContext.Task.Remove(task);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {

            throw; 
            }         
        }

        public async Task<Task_DTOs> GetTaskByIdAsync(int Id)
        {
            try
            {
                var task = await _dbContext.Task.FirstOrDefaultAsync(t => t.Id == Id);
                if (task == null)
                {
                    throw new("Không tìm thấy task");
                }
                return new Task_DTOs
                {
                    Id = task.Id,
                    AssignedTo = task.AssignedTo,
                    Description = task.Description,
                    EndDate = task.EndDate,
                    Priority = task.Priority,
                    ProjectId = task.ProjectId,
                    StartDate = task.StartDate,
                    Status = task.Status,
                    TaskName = task.TaskName,

                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Task_DTOs>> GetTasksAsync()
        {
            var result = await _dbContext.Task
               .Select(p => new Task_DTOs
               {
                   Id = p.Id,
                   TaskName = p.TaskName,
                   AssignedTo = p.AssignedTo,
                   Description = p.Description,
                   EndDate = p.EndDate,
                   Priority = p.Priority,
                   ProjectId = p.ProjectId,
                   StartDate = p.StartDate,
                   Status = p.Status,
               })
               .ToListAsync();
            return result;
        }

        public async Task<List<Task_DTOs>> GetTasksByProjectAsync(int ProjectId)
        {
            var tasks = await _dbContext.Task
                .Where(t => t.ProjectId == ProjectId)
                .Join(_dbContext.User, 
                    task => task.AssignedTo, 
                    user => user.Id, 
                    (task, user) => new Task_DTOs 
                    {
                        Id = task.Id,
                        AssignedTo = task.AssignedTo, 
                        FullName = user.FirstName + " " +user.LastName,
                        AssignedToEmail = user.Email, 
                        Description = task.Description,
                        EndDate = task.EndDate,
                        Priority = task.Priority,
                        ProjectId = task.ProjectId,
                        StartDate = task.StartDate,
                        Status = task.Status,
                        TaskName = task.TaskName,
                    })
                .ToListAsync();

            return tasks;
        }

        public async Task UpdateTaskAsync(Task_DTOs task, int Id)
        {
            try
            {
                var result = await _dbContext.Task.FirstOrDefaultAsync(t => t.Id == Id);
                if (result == null)
                {
                    throw new Exception("Không tìm thấy task.");
                }

                var previousAssignedTo = result.AssignedTo;
                var isAssignedToChanged = previousAssignedTo != task.AssignedTo;

                result.StartDate = task.StartDate;
                result.EndDate = task.EndDate;
                result.Priority = task.Priority;
                result.ProjectId = task.ProjectId;
                result.Status = task.Status;
                result.TaskName = task.TaskName;
                result.AssignedTo = task.AssignedTo;
                result.Description = task.Description;

                _dbContext.Task.Update(result);
                await _dbContext.SaveChangesAsync();

                if (isAssignedToChanged)
                {
                    // Gửi email thông báo cho người dùng cũ (nếu có)
                    if (previousAssignedTo != 0)
                    {
                        var previousUser = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == previousAssignedTo);
                        if (previousUser != null && !string.IsNullOrEmpty(previousUser.Email))
                        {
                            await _emailService.SendEmail(new SendEmailDTOs
                            {
                                ToEmail = previousUser.Email,
                                Subject = "Thông báo: Bạn đã được gỡ khỏi công việc",
                                Body = $@"
                                    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                                        <h2 style='color: #d9534f;'>Thông báo</h2>
                                        <p>Chào <b>{previousUser.FirstName} {previousUser.LastName}</b>,</p>
                                        <p>Bạn đã được gỡ khỏi công việc:</p>
                                        <ul>
                                            <li><strong>Tên công việc:</strong> {result.TaskName}</li>
                                        </ul>
                                        <p>Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với bộ phận quản lý dự án.</p>
                                        <p>Trân trọng,</p>
                                        <p><b>Bộ phận quản lý dự án</b></p>
                                        <hr style='border: none; border-top: 1px solid #ddd;' />
                                        <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                                    </div>"
                            });

                        }
                    }

                    // Gửi email thông báo cho người dùng mới (nếu có)
                    var newUser = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == task.AssignedTo);
                    if (newUser != null && !string.IsNullOrEmpty(newUser.Email))
                    {
                        await _emailService.SendEmail(new SendEmailDTOs
                        {
                            ToEmail = newUser.Email,
                            Subject = "Thông báo: Bạn đã được giao công việc mới",
                            Body = $@"
                                <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                                    <h2 style='color: #5cb85c;'>Thông báo công việc mới</h2>
                                    <p>Chào <b>{newUser.FirstName} {newUser.LastName}</b>,</p>
                                    <p>Bạn đã được giao công việc mới:</p>
                                    <ul>
                                        <li><strong>Tên công việc:</strong> {task.TaskName}</li>
                                        <li><strong>Mô tả:</strong> {task.Description}</li>
                                        <li><strong>Mức độ ưu tiên:</strong> {task.Priority}</li>
                                    </ul>
                                    <p>Vui lòng đăng nhập hệ thống để xem thêm chi tiết và cập nhật trạng thái công việc:</p>
                                    <p><a href='https://your-project-management-system.com/tasks/{result.Id}' 
                                          style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                          Xem công việc
                                       </a>
                                    </p>
                                    <p>Trân trọng,</p>
                                    <p><b>Bộ phận quản lý dự án</b></p>
                                    <hr style='border: none; border-top: 1px solid #ddd;' />
                                    <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                                </div>"
                        });
                    }
                }
            }
            catch
            {
                throw;
            }
          
        }

    }
}
