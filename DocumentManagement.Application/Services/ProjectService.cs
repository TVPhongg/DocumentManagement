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
    public class ProjectService : IProjectService
    {
        private readonly MyDbContext _dbContext;
        private readonly EmailService _emailService;

        public ProjectService(MyDbContext dbContext, EmailService emailService) 
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task CreateProjectAsync(Project_DTOs project)
        {
            // Tạo đối tượng mới cho Project
            var newProject = new Projects
            {
                ProjectName = project.ProjectName,
                Status = project.Status,
                Priority = project.Priority,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreateBy = project.CreateBy,
                TeamMember = new List<int>() // Khởi tạo danh sách TeamMember
            };

            // Thêm các thành viên vào TeamMember từ DTO
            foreach (var memberId in project.TeamMember)
            {
                newProject.TeamMember.Add(memberId);
            }
            // Lưu project vào cơ sở dữ liệu
            await _dbContext.Project.AddAsync(newProject);
            await _dbContext.SaveChangesAsync();

            // Lấy danh sách email từ bảng User dựa trên các TeamMember
            var emails = await _dbContext.User
                .Where(u => project.TeamMember.Contains(u.Id)) // Lọc các User theo TeamMember
                .Select(u => u.Email) // Lấy ra email của từng User
                .ToListAsync();

            // Gửi email đến từng thành viên trong dự án
            foreach (var email in emails)
            {
                await _emailService.SendEmail(new SendEmailDTOs
                {
                    ToEmail = email,
                    Subject = "Thông báo: Tham gia dự án mới",
                    Body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #5cb85c;'>Thông báo: Tham gia dự án mới</h2>
                        <p>Chào bạn,</p>
                        <p>Bạn đã được thêm vào dự án:</p>
                        <ul>
                            <li><strong>Tên dự án:</strong> {project.ProjectName}</li>
                        </ul>
                        <p>Vui lòng đăng nhập vào hệ thống để kiểm tra thông tin chi tiết và cập nhật tiến độ:</p>
                        <p>
                            <a href='https://your-project-management-system.com/projects/{project.Id}' 
                               style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                               Xem chi tiết dự án
                            </a>
                        </p>
                        <p>Trân trọng,</p>
                        <p><b>Đội ngũ quản lý dự án</b></p>
                        <hr style='border: none; border-top: 1px solid #ddd;' />
                        <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                    </div>"
                });

            }
        }

        public async Task DeleteProjectAsync(int Id)
        {
            var project = await _dbContext.Project.FirstOrDefaultAsync(p=>p.Id == Id);
            if (project != null) 
            {
                _dbContext.Project.Remove(project);
               await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Project_DTOs>> GetProjectsAsync()
        {
            var result = await _dbContext.Project
                .Select(p => new Project_DTOs
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Status = p.Status,
                    Priority = p.Priority,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    CreateBy = p.CreateBy,
                    CreatedByName = _dbContext.User
                    .Where(u => u.Id == p.CreateBy)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .FirstOrDefault(), // Lấy họ và tên của người tạo dự án
                    TeamMember = p.TeamMember,                                             
                    MembersDetail = _dbContext.User
                        .Where(u => p.TeamMember.Contains(u.Id)) 
                        .Join(_dbContext.Role, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
                        .Join(_dbContext.Department, ur => ur.u.DepartmentId, d => d.Id, (ur, d) => new UserDto
                        {
                            Id = ur.u.Id,
                            FirstName = ur.u.FirstName,
                            LastName = ur.u.LastName,
                            Email = ur.u.Email,
                            PhoneNumber = ur.u.PhoneNumber,
                            Address = ur.u.Address,
                            Gender = ur.u.Gender,
                            RoleName = ur.r.RoleName, 
                            DepartmentName = d.Name  
                        })
                        .ToList(),
                     TeamSize = _dbContext.User
                                   .Count(u => p.TeamMember.Contains(u.Id))
                })
                .ToListAsync();
            int projectCount = result.Count();
            result.ForEach(project => project.ProjectCount = projectCount);
            return result;
        }

        public async Task<Project_DTOs> GetProjectsByIdAsync(int id)
        {
            // Lấy dự án theo ID
            var project = await _dbContext.Project.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                throw new Exception("Không tìm thấy dự án");
            }

            var membersDetail = await _dbContext.User
                .Where(u => project.TeamMember.Contains(u.Id))
                .Join(_dbContext.Role, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
                .Join(_dbContext.Department, ur => ur.u.DepartmentId, d => d.Id, (ur, d) => new UserDto
                {
                    Id = ur.u.Id,
                    FirstName = ur.u.FirstName,
                    LastName = ur.u.LastName,
                    Email = ur.u.Email,
                    PhoneNumber = ur.u.PhoneNumber,
                    Address = ur.u.Address,
                    Gender = ur.u.Gender,
                    RoleName = ur.r.RoleName,
                    DepartmentName = d.Name
                })
                .ToListAsync();

            var createdByName = await _dbContext.User
                .Where(u => u.Id == project.CreateBy)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefaultAsync();

            // Trả về dữ liệu
            return new Project_DTOs
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Priority = project.Priority,
                Status = project.Status,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreateBy = project.CreateBy,
                CreatedByName = createdByName,
                TeamMember = project.TeamMember,
                MembersDetail = membersDetail
            };
        }

        public async Task UpdateProjectAsync(Project_DTOs project, int Id)
        {
            var result = await _dbContext.Project.FirstOrDefaultAsync(u => u.Id == Id);
            if (result == null)
            {
                throw new Exception("Không tìm thấy dự án");
            }

            result.StartDate = project.StartDate;
            result.EndDate = project.EndDate;
            result.Description = project.Description;
            result.ProjectName = project.ProjectName;
            result.CreateBy =  project.CreateBy;
            result.Status = project.Status;
            result.Priority = project.Priority;

            var currentTeamMembers = result.TeamMember.ToList(); // Danh sách TeamMember hiện tại
            var newTeamMembers = project.TeamMember; // Danh sách TeamMember mới

            // Tìm các thành viên bị loại bỏ
            var removedMembers = currentTeamMembers.Where(memberId => !newTeamMembers.Contains(memberId)).ToList();

            // Tìm các thành viên mới được thêm vào
            var addedMembers = newTeamMembers.Where(memberId => !currentTeamMembers.Contains(memberId)).ToList();

            // Cập nhật danh sách TeamMember
            result.TeamMember = newTeamMembers;

            // Lưu thay đổi vào cơ sở dữ liệu
            _dbContext.Project.Update(result);
            await _dbContext.SaveChangesAsync();

            // Gửi email thông báo cho các thành viên bị loại bỏ
            if (removedMembers.Any())
            {
                var removedEmails = await _dbContext.User
                    .Where(u => removedMembers.Contains(u.Id))
                    .Select(u => u.Email)
                    .ToListAsync();

                foreach (var email in removedEmails)
                {
                    await _emailService.SendEmail(new SendEmailDTOs
                    {
                        ToEmail = email,
                        Subject = "Thông báo: Rời khỏi dự án",
                        Body = $@"
                        <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                            <h2 style='color: #d9534f;'>Thông báo: Rời khỏi dự án</h2>
                            <p>Chào bạn,</p>
                            <p>Bạn đã bị xóa khỏi dự án:</p>
                            <ul>
                                <li><strong>Tên dự án:</strong> {project.ProjectName}</li>
                            </ul>
                            <p>Nếu bạn có bất kỳ câu hỏi nào hoặc cần thêm thông tin, vui lòng liên hệ với quản lý dự án để được hỗ trợ.</p>
                            <p>Trân trọng,</p>
                            <p><b>Đội ngũ quản lý dự án</b></p>
                            <hr style='border: none; border-top: 1px solid #ddd;' />
                            <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                        </div>"
                    });

                }
            }

            // Gửi email thông báo cho các thành viên mới được thêm vào
            if (addedMembers.Any())
            {
                var addedEmails = await _dbContext.User
                    .Where(u => addedMembers.Contains(u.Id))
                    .Select(u => u.Email)
                    .ToListAsync();

                foreach (var email in addedEmails)
                {
                    await _emailService.SendEmail(new SendEmailDTOs
                    {
                        ToEmail = email,
                        Subject = "Thông báo: Tham gia dự án mới",
                        Body = $@"
                        <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                            <h2 style='color: #5cb85c;'>Thông báo: Tham gia dự án mới</h2>
                            <p>Chào bạn,</p>
                            <p>Bạn đã được thêm vào dự án:</p>
                            <ul>
                                <li><strong>Tên dự án:</strong> {project.ProjectName}</li>
                            </ul>
                            <p>Vui lòng đăng nhập vào hệ thống để kiểm tra thông tin chi tiết và cập nhật tiến độ:</p>
                            <p>
                                <a href='https://your-project-management-system.com/projects/{project.Id}' 
                                    style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                    Xem chi tiết dự án
                                </a>
                            </p>
                            <p>Trân trọng,</p>
                            <p><b>Đội ngũ quản lý dự án</b></p>
                            <hr style='border: none; border-top: 1px solid #ddd;' />
                            <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                        </div>"
                    });

                }
            }
        }

    }
}
