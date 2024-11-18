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
        private readonly EmailService _emailService;

        public RequestService(MyDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;

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
                            RoleName = level.Role.RoleName,
                            
                             
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

        public async Task Add_Request(Request_DTO request, IFormFile file, int userId)
        {
            var approvalFlow = await _context.ApprovalFlow
                .Include(af => af.ApprovalLevels)
                .FirstOrDefaultAsync(af => af.Id == request.FlowId);

            if (approvalFlow == null)
            {
                throw new ArgumentException("Luồng phê duyệt không tồn tại.");
            }

            string fileExtension = Path.GetExtension(file.FileName);
            string fileName = $"{DateTime.Now:yyyyMMddssffff}{fileExtension}";
            string filePath = Path.Combine(@"E:\DocumentManagement\DocumentManagement\File", fileName);

            // Lưu file vào hệ thống
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var requestDocument = new RequestDocument
            {
                Title = request.Title,
                CreatedDate = DateTime.Now,
                File = filePath,
                UserId = userId,
                FlowId = request.FlowId,
                ApprovalSteps = new List<ApprovalSteps>()
            };

            _context.Request.Add(requestDocument);
            await _context.SaveChangesAsync();

            // Cập nhật RequestId cho các ApprovalSteps
            var approvalSteps = approvalFlow.ApprovalLevels.Select(lv => new ApprovalSteps
            {
                Step = lv.Step,
                RequestId = requestDocument.Id,
                UserId = 0,
                Status = 0,
                UpdateTime = DateTime.Now,
                Comment = ""
            }).ToList();

            // Lấy danh sách người phê duyệt
            var approvers = await Get_Approvers(request.FlowId, request.UserId);
            ApprovalSteps firstStep = null;
            string firstApproverEmail = null;

            // Cập nhật UserId cho các bước phê duyệt
            foreach (var step in approvalSteps)
            {
                var approver = approvers.FirstOrDefault(a => a.Step == step.Step);
                if (approver != null)
                {
                    step.UserId = approver.UserId;

                    var user = await _context.User.FirstOrDefaultAsync(u => u.Id == approver.UserId);
                    if (user != null)
                    {
                        // Xác định người phê duyệt đầu tiên
                        if (firstStep == null && step.Step == 1)
                        {
                            step.Status = 1; // Đặt trạng thái cho bước đầu tiên
                            firstStep = step; // Lưu vào biến firstStep
                            firstApproverEmail = user.Email; // Lưu email của người phê duyệt đầu tiên
                        }
                    }
                }
            }

            // Thêm các bước phê duyệt vào cơ sở dữ liệu
            _context.ApprovalStep.AddRange(approvalSteps);
            await _context.SaveChangesAsync();

            // Gửi email cho người phê duyệt đầu tiên
            if (!string.IsNullOrEmpty(firstApproverEmail))
            {
                await _emailService.SendEmail(new SendEmailDTOs
                {
                    ToEmail = firstApproverEmail,
                    Subject = "Xác nhận yêu cầu phê duyệt",
                    Body = "Bạn có một yêu cầu phê duyệt mới. Vui lòng kiểm tra ứng dụng để biết thêm chi tiết."
                });
            }
        }


        public async Task<IEnumerable<Request_DTO>> Get_RequestDocument(int userId)
        {
            var requests = await _context.Request
                .Include(rq => rq.ApprovalSteps)
                    .ThenInclude(st => st.User)
                .Where(rq => rq.UserId == userId ||
                             rq.ApprovalSteps.Any(st => st.UserId == userId && st.Status == 1)) // Kiểm tra userId trong cả bảng Request và ApprovalSteps
                .Select(rq => new Request_DTO
                {
                    Id = rq.Id,
                    Title = rq.Title,
                    CreatedDate = rq.CreatedDate,
                    Files = rq.File,
                    FlowId = rq.FlowId,
                    UserId = rq.UserId,
                    ApprovalSteps = rq.ApprovalSteps
                        .Where(st => st.Status == 1) 
                        .Select(st => new Step_DTO
                        {
                            Id = st.Id,
                            Step = st.Step,
                            UserId = st.UserId,
                            UserName = $"{st.User.FirstName} {st.User.LastName}",
                            Status = st.Status,
                            UpdateTime = st.UpdateTime,
                            Comment = st.Comment
                        }).ToList()
                }).ToListAsync();

            return requests;
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

        public async Task Approval_Request(ApprovalAction_DTO action, int userId, int RequestId)
        {
            // Tìm bước hiện tại cần phê duyệt
            var approvalStep = await _context.ApprovalStep
                .FirstOrDefaultAsync(st => st.RequestId == RequestId && st.Status == 1 && st.UserId == userId);

            if (approvalStep == null)
            {
                throw new ArgumentException("approvalStep không tồn tại");
            }

            // Cập nhật trạng thái phê duyệt
            approvalStep.Status = 2; 
            approvalStep.Comment = action.Comment;
            approvalStep.UpdateTime = DateTime.Now;

            _context.ApprovalStep.Update(approvalStep);
            await _context.SaveChangesAsync();

            // Tìm bước phê duyệt tiếp theo
            var nextStep = await _context.ApprovalStep
                .FirstOrDefaultAsync(st => st.RequestId == RequestId && st.Step == approvalStep.Step + 1);

            if (nextStep != null)
            {
                nextStep.Status = 1; 
                await _context.SaveChangesAsync();
                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == nextStep.UserId);
                if (user != null)
                {
                    // Gửi email thông báo
                    await _emailService.SendEmail(new SendEmailDTOs
                    {
                        ToEmail = user.Email,
                        Subject = "Xác nhận yêu cầu phê duyệt",
                        Body = "Bạn có một yêu cầu phê duyệt mới. Vui lòng kiểm tra ứng dụng để biết thêm chi tiết."
                    });
                }
            }
            else
            {
                // Nếu không còn bước nào nữa, kiểm tra xem tất cả các bước đã được phê duyệt chưa
                var allApproved = await _context.ApprovalStep
                    .Where(st => st.RequestId == RequestId)
                    .AllAsync(st => st.Status == 2); // Kiểm tra tất cả bước đã được phê duyệt

                if (allApproved)
                {
                    // Lấy thông tin người gửi yêu cầu
                    var request = await _context.Request
                        .FirstOrDefaultAsync(r => r.Id == RequestId);

                    if (request != null)
                    {
                        // Gửi email thông báo cho người đã gửi yêu cầu
                        var requester = await _context.User.FirstOrDefaultAsync(u => u.Id == request.UserId);
                        if (requester != null)
                        {
                            await _emailService.SendEmail(new SendEmailDTOs
                            {
                                ToEmail = requester.Email,
                                Subject = "Yêu cầu phê duyệt đã được xử lý",
                                Body = "Yêu cầu phê duyệt của bạn đã được chấp thuận. Vui lòng kiểm tra ứng dụng để biết thêm chi tiết."
                            });
                        }
                    }
                }
            }
        }


        public async Task Reject_Request(ApprovalAction_DTO action,int userId, int RequestId)
        {
            // Tìm bước hiện tại cần phê duyệt (có Status là 1 - Active)
            var approvalStep = await _context.ApprovalStep
           .Include(st => st.request)
           .ThenInclude(rd => rd.User)
           .FirstOrDefaultAsync(st => st.RequestId == RequestId && st.Status == 1 && st.UserId == userId);

            if (approvalStep == null)
            {
                throw new ArgumentException("approvalStep không tồn tại");
            }

            approvalStep.Status = 3; // Đã từ chối phê duyệt
            approvalStep.Comment = action.Comment;
            approvalStep.UpdateTime =DateTime.Now;

            _context.ApprovalStep.Update(approvalStep);
            await _context.SaveChangesAsync();

            // Lấy email của người gửi yêu cầu
            var userEmail = approvalStep.request.User.Email;
            if (userEmail != null)
            {
                // Gửi email thông báo
                await _emailService.SendEmail(new SendEmailDTOs
                {
                    ToEmail = userEmail,
                    Subject = "Xác nhận yêu cầu phê duyệt",
                    Body = "Yêu cầu phê duyệt của bạn không được chấp thuận. Vui lòng kiểm tra ứng dụng để biết thêm chi tiết."
                });
            }

        }
    }
}
