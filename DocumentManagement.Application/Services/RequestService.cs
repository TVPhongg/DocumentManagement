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

            var approvers = new List<Approver_DTO>();

            // Xử lý từng bước phê duyệt
            foreach (var level in approvalLevels)
            {
                var users = level.Role.RoleName switch
                {
                    "GD" => await _context.User.Where(u => u.RoleId == level.RoleId).ToListAsync(),
                    "KT" => await _context.User.Where(u => u.RoleId == level.RoleId).ToListAsync(),
                    "BA" => await _context.User.Where(u => u.RoleId == level.RoleId && u.DepartmentId == userDepartmentId).ToListAsync(),
                    "Dev" => await _context.User.Where(u => u.RoleId == level.RoleId && u.DepartmentId == userDepartmentId).ToListAsync(),
                    "Tester" => await _context.User.Where(u => u.RoleId == level.RoleId).ToListAsync(),
                    "QL" => await _context.User.Where(u => u.RoleId == level.RoleId && u.DepartmentId == userDepartmentId).ToListAsync(),
                    _ => await _context.User.Where(u => u.RoleId == level.RoleId && u.DepartmentId == userDepartmentId).ToListAsync()
                };

                approvers.AddRange(users.Select(approver => new Approver_DTO
                {
                    Step = level.Step,
                    UserId = approver.Id,
                    NameUser = $"{approver.FirstName} {approver.LastName}",
                    RoleName = level.Role.RoleName
                }));
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
                    Subject = "Thông báo: Xác nhận yêu cầu phê duyệt",
                    Body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #f0ad4e;'>Thông báo: Yêu cầu phê duyệt mới</h2>
                        <p>Chào bạn,</p>
                        <p>Bạn có một yêu cầu phê duyệt mới. Vui lòng đăng nhập vào ứng dụng để kiểm tra chi tiết.</p>
                        <p>
                            <a href='https://your-application-url.com/approval-requests' 
                                style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                Kiểm tra yêu cầu phê duyệt
                            </a>
                        </p>
                        <p>Trân trọng,</p>
                        <p><b>Đội ngũ quản lý hệ thống</b></p>
                        <hr style='border: none; border-top: 1px solid #ddd;' />
                        <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                    </div>"
                });

            }
        }


        public async Task<IEnumerable<Request_DTO>> Get_RequestDocument(int userId)
        {
            var requests = await _context.Request
                .Include(rq => rq.ApprovalSteps)
                    .ThenInclude(st => st.User)
                .Where(rq => rq.UserId == userId ||
                             rq.ApprovalSteps.Any(st => st.UserId == userId)) // Kiểm tra userId trong cả bảng Request và ApprovalSteps
                .Select(rq => new Request_DTO
                {
                    Id = rq.Id,
                    Title = rq.Title,
                    CreatedDate = rq.CreatedDate,
                    Files = rq.File,
                    FlowId = rq.FlowId,
                    UserId = rq.UserId,
                    UserName = $"{rq.User.FirstName} {rq.User.LastName}",
                    ApprovalSteps = rq.ApprovalSteps

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
            int totalRequests = requests.Count;
            int approvedRequests = requests.Count( rq => rq.ApprovalSteps.Any(st => st.Status == 2));
            int pendingRequests = requests.Count(rq => rq.ApprovalSteps.Any(st => st.Status == 1));
            requests.ForEach(request =>
            {
                request.TotalRequests = totalRequests;
                request.ApprovedRequests = approvedRequests;
                request.PendingRequests = pendingRequests;
            });

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
                        Subject = "Thông báo: Xác nhận yêu cầu phê duyệt",
                        Body = $@"
                        <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                            <h2 style='color: #f0ad4e;'>Thông báo: Yêu cầu phê duyệt mới</h2>
                            <p>Chào bạn,</p>
                            <p>Bạn có một yêu cầu phê duyệt mới. Vui lòng đăng nhập vào hệ thống để kiểm tra chi tiết:</p>
                            <p>
                                <a href='https://your-application-url.com/approval-requests' 
                                    style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                    Xem yêu cầu phê duyệt
                                </a>
                            </p>
                            <p>Trân trọng,</p>
                            <p><b>Đội ngũ quản lý hệ thống</b></p>
                            <hr style='border: none; border-top: 1px solid #ddd;' />
                            <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                        </div>"
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
                                Subject = "Thông báo: Yêu cầu phê duyệt đã được xử lý",
                                Body = $@"
                                <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                                    <h2 style='color: #5cb85c;'>Yêu cầu phê duyệt đã được chấp thuận</h2>
                                    <p>Chào bạn,</p>
                                    <p>Yêu cầu phê duyệt của bạn đã được chấp thuận. Vui lòng đăng nhập vào hệ thống để kiểm tra chi tiết:</p>
                                    <p>
                                        <a href='https://your-application-url.com/approval-requests/' 
                                           style='color: #fff; background-color: #0275d8; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                                           Xem chi tiết yêu cầu
                                        </a>
                                    </p>
                                    <p>Trân trọng,</p>
                                    <p><b>Đội ngũ quản lý hệ thống</b></p>
                                    <hr style='border: none; border-top: 1px solid #ddd;' />
                                    <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                                </div>"
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
                    Subject = "Thông báo: Yêu cầu phê duyệt không được chấp thuận",
                    Body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <h2 style='color: #d9534f;'>Yêu cầu phê duyệt không được chấp thuận</h2>
                        <p>Chào bạn,</p>
                        <p>Rất tiếc, yêu cầu phê duyệt của bạn không được chấp thuận.</p>
                        <p>Vui lòng đăng nhập vào hệ thống để kiểm tra chi tiết:</p>
                        <p>
                            <a href='https://your-application-url.com/approval-requests/' 
                               style='color: #fff; background-color: #d9534f; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>
                               Xem chi tiết yêu cầu
                            </a>
                        </p>
                        <p>Trân trọng,</p>
                        <p><b>Đội ngũ quản lý hệ thống</b></p>
                        <hr style='border: none; border-top: 1px solid #ddd;' />
                        <p style='font-size: 12px; color: #888;'>Đây là email tự động, vui lòng không trả lời.</p>
                    </div>"
                });

            }

        }
    }
}
