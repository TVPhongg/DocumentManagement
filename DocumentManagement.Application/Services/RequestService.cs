using DocumentManagement.Application.DTOs;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Domain.Context;
using DocumentManagement.Domain.Entities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly MyDbContext _context;
        public RequestService(MyDbContext context)
        {
            _context = context;

        }

        public async Task<List<Approver_DTO>> Get_Approvers(int FlowId, int UserId)
        {
            var user = await _context.User.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null)
            {
                throw new ArgumentException("User không tồn tại");
            }

            var userDepartmentId = user.DepartmentId;

            var approvalLevels = await _context.ApprovalLevel
                .Where(level => level.FlowId == FlowId)
                .Include(level => level.Role)
                .OrderBy(level => level.Step)
                .ToListAsync();
            // Tạo danh sách để chứa thông tin những người phê duyệt
            var approvers = new List<Approver_DTO>();
            // Duyệt qua từng bước phê duyệt trong luồng
            foreach (var level in approvalLevels)
            {
                if (level.Role.RoleName == "GD")
                {
                    // Lấy tất cả người dùng có vai trò "GD" không quan tâm đến DepartmentId
                    var users = await _context.User
                        .Where(u => u.RoleId == level.RoleId)
                        .ToListAsync();

                    foreach (var approver in users)
                    {
                        approvers.Add(new Approver_DTO
                        {
                            Step = level.Step,
                            UserId = approver.Id,
                            NameUser = approver.FirstName + " " + approver.LastName,
                            RoleName = level.Role.RoleName
                        });
                    }
                }
                else if (level.Role.RoleName == "KT")
                {
                    // Lấy tất cả người dùng có vai trò "KT" không quan tâm đến DepartmentId
                    var users = await _context.User
                        .Where(u => u.RoleId == level.RoleId)
                        .ToListAsync();

                    foreach (var approver in users)
                    {
                        approvers.Add(new Approver_DTO
                        {
                            Step = level.Step,
                            UserId = approver.Id,
                            NameUser = approver.FirstName + " " + approver.LastName,
                            RoleName = level.Role.RoleName
                        });
                    }
                }
                else
                {
                    // Lấy tất cả người dùng có vai trò khác "GD" và có cùng DepartmentId
                    var users = await _context.User
                        .Where(u => u.RoleId == level.RoleId && u.DepartmentId == userDepartmentId)
                        .ToListAsync();

                    foreach (var approver in users)
                    {
                        approvers.Add(new Approver_DTO
                        {
                            Step = level.Step,
                            UserId = approver.Id,
                            NameUser = approver.FirstName + " " + approver.LastName,
                            RoleName = level.Role.RoleName
                        });
                    }
                }
            }

            return approvers;
        }

        public async Task Add_Request(Request_DTO request)
        {
           

            var approvalFlow = await _context.ApprovalFlow
                .Include(af => af.ApprovalLevels)
                .FirstOrDefaultAsync(af => af.Id == request.FlowId);

            //Lấy đường dẫn của thư mục hiện tại nơi ứng dụng đang chạy kết hợp với tên thư mục "Upload".
            var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Upload");

            // Tạo đường dẫn đầy đủ cho tệp tải lên bằng cách kết hợp đường dẫn thư mục upload và tên tệp
            var filePath = Path.Combine(uploadDir, Path.GetFileName(request.File.FileName));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var requestDocument = new RequestDocument
            {
                Title = request.Title,
                CreatedDate = DateTime.UtcNow.Date,
                File = filePath,
                UserId = request.UserId,
                FlowId = request.FlowId,
                ApprovalSteps = new List<ApprovalSteps>()
            };

            _context.Request.Add(requestDocument);
            await _context.SaveChangesAsync();

            // Cập nhật RequestId cho các ApprovalSteps
            var approvalSteps = approvalFlow.ApprovalLevels.Select(lv => new ApprovalSteps
            {
                Step = lv.Step,
                RequestId = requestDocument.Id, // Thiết lập RequestId
                UserId = 0,
                Status = 0,
                UpdateTime = DateTime.MinValue,
                Comment = ""
            }).ToList();

            // Cập nhật UserId cho các ApprovalSteps sử dụng hàm Get_Approvers
            var approvers = await Get_Approvers(request.FlowId, request.UserId);
            ApprovalSteps firstStep = null;
            foreach (var step in approvalSteps)
            {
                var approver = approvers.FirstOrDefault(a => a.Step == step.Step);
                if (approver != null)
                {

                    step.UserId = approver.UserId; // Cập nhật UserId từ hàm Get_Approvers
                }
                 if (step.Step == 1)
                {
                    step.Status = 1; // Cập nhật Step 1 trạng thái thành 1(Active)
                    firstStep = step; // lưu vào biến firstStep 
                }
            }

            // Thêm ApprovalSteps vào cơ sở dữ liệu
            _context.ApprovalStep.AddRange(approvalSteps);
            await _context.SaveChangesAsync();
            // Gửi mail cho người phê duyệt đầu tiên 
         /*   if (firstStep != null && firstStep.UserId != 0)
            {
                var firstApprover = await _context.Users.FindAsync(firstStep.UserId);
                if (firstApprover != null)
                {
                    var emailService = HttpContext.RequestServices.GetRequiredService<EmailService>();
                    await emailService.SendEmailAsync(firstApprover.Email, "New Document Approval Request", "You have a new document to approve.");
                }
            }*/


        }

        public async Task<IEnumerable<Request_DTO>> Get_RequestDocument(int pageNumber , int pageSize)
        {
            var Requestes = await _context.Request
                     .Include(st => st.ApprovalSteps)
                     .OrderBy(r => r.Id)
                     .Skip((pageNumber - 1)*pageSize)
                     .Take(pageSize)
                     .Select(rq => new Request_DTO
                     {
                         Id = rq.Id,
                         Title = rq.Title,
                         CreatedDate = rq.CreatedDate,
                         Files = rq.File,
                         FlowId = rq.FlowId,
                         UserId = rq.UserId,
                         ApprovalSteps = rq.ApprovalSteps.Select(st => new Step_DTO
                         {
                            Step = st.Step,
                            UserId = st.UserId,
                            Status = st.Status,
                            UpdateTime = st.UpdateTime,
                            Comment= st.Comment
                         }).ToList()
                     }).ToListAsync();
            return Requestes;
        }

        public async Task Delete_Request(int id)
        {
            var Request = await _context.Request
        .Include(rq => rq.ApprovalSteps)
        .FirstOrDefaultAsync(rq => rq.Id == id);
  
            // Xóa các bước phê duyệt liên quan
            _context.ApprovalStep.RemoveRange(Request.ApprovalSteps);

            // Xóa yêu cầu phê duyệt
            _context.Request.Remove(Request);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Request_DTO>> Search_Request(string? name, DateTime? StarDate, DateTime? EndDate)
        {
            var Requestes = await _context.Request
        .Include(rq => rq.ApprovalSteps)
        .Where(rq => (string.IsNullOrEmpty(name) || rq.Title.ToUpper().Contains(name.ToUpper())) &&
                     (!StarDate.HasValue || rq.CreatedDate >= StarDate) &&
                     (!EndDate.HasValue || rq.CreatedDate <= EndDate))
        .Select(af => new Request_DTO
        {
            Id = af.Id,
            Title = af.Title,
            CreatedDate = af.CreatedDate,
            Files = af.File,
            FlowId = af.FlowId,
            UserId = af.UserId,
            ApprovalSteps = af.ApprovalSteps.Select(st => new Step_DTO
            {
                Step = st.Step,
                UserId = st.UserId,
                Status = st.Status,
                UpdateTime = st.UpdateTime,
                Comment = st.Comment
            }).ToList()
        }).ToListAsync();
            return Requestes;
        }

        public async Task Approval_Request(ApprovalAction_DTO action)
        {
            // Tìm bước hiện tại cần phê duyệt (có Status là 1 - Active)
            var approvalStep = await _context.ApprovalStep
                .FirstOrDefaultAsync(st => st.RequestId == action.RequestId && st.Status == 1);

            if (approvalStep == null)
            {
                throw new ArgumentException("approvalStep không tồn tại");
            }

            approvalStep.Status = 2; // Đã đồng ý phê duyệt
            approvalStep.Comment = action.Comment;
            approvalStep.UpdateTime = DateTime.UtcNow.Date;

            _context.ApprovalStep.Update(approvalStep);
            await _context.SaveChangesAsync();

            // Tìm bước phê duyệt tiếp theo
            var nextStep = await _context.ApprovalStep
                .FirstOrDefaultAsync(st => st.RequestId == action.RequestId && st.Step == approvalStep.Step + 1);

            if (nextStep != null)
            {
                nextStep.Status = 1; // Cập nhật trạng thái của bước tiếp theo thành Active
                await _context.SaveChangesAsync();

                // Gửi email cho người phê duyệt tiếp theo
            
            }
        }

        public async Task Reject_Request(ApprovalAction_DTO action)
        {
            // Tìm bước hiện tại cần phê duyệt (có Status là 1 - Active)
            var approvalStep = await _context.ApprovalStep
           .Include(st => st.request)
           .ThenInclude(rd => rd.User)
           .FirstOrDefaultAsync(st => st.RequestId == action.RequestId && st.Status == 1);

            if (approvalStep == null)
            {
                throw new ArgumentException("approvalStep không tồn tại");
            }

            approvalStep.Status = 3; // Đã từ chối phê duyệt
            approvalStep.Comment = action.Comment;
            approvalStep.UpdateTime = DateTime.UtcNow.Date;

            _context.ApprovalStep.Update(approvalStep);
            await _context.SaveChangesAsync();

            // Lấy email của người gửi yêu cầu
            var userEmail = approvalStep.request.User.Email;
            // Gửi email thông báo

        }
    }
}
