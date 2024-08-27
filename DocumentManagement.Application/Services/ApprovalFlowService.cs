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
    public class ApprovalFlowService : IFlowService
    {
        private readonly MyDbContext _context;

        public ApprovalFlowService(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddFlow(ApprovalFlow_DTO request)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Tên luồng phê duyệt không được rỗng.");
            }

            if (request.ApprovalLevels == null || !request.ApprovalLevels.Any())
            {
                throw new ArgumentException("Phải có ít nhất một bước phê duyệt.");
            }

            foreach (var levelDto in request.ApprovalLevels)
            {
                if (levelDto.Step <= 0)
                {
                    throw new ArgumentException("Bước phê duyệt phải lớn hơn 0.");
                }

                if (levelDto.RoleId <= 0)
                {
                    throw new ArgumentException("ID vai trò phải lớn hơn 0.");
                }
            }

            var approvalFlow = new ApprovalFlows
            {
                Name = request.Name,
                CreatedDate =DateTime.Now,
                ApprovalLevels = request.ApprovalLevels
                .Select(levelDto => new ApprovalLevels
                {
                    Step = levelDto.Step,
                    RoleId = levelDto.RoleId,
                }).ToList()
            };
            _context.ApprovalFlow.Add(approvalFlow);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalFlow_DTO>> Get_ApprovalFlows()
        {
            var ApprovalFlows = await _context.ApprovalFlow
                     .Include(af => af.ApprovalLevels)
                     .Select(af => new ApprovalFlow_DTO
                     {
                         Id = af.Id,
                         Name = af.Name,
                         CreatedDate = af.CreatedDate.Date,
                         ApprovalLevels = af.ApprovalLevels.Select(lv => new ApprovalLevel_DTO
                         {
                             Step = lv.Step,
                             RoleId = lv.RoleId,
                             Name = lv.Role.Name
                         }).ToList()
                     }).ToListAsync();
            return ApprovalFlows;
        }

        public async Task<ApprovalFlow_DTO> Get_ApprovalFlow(int id)
        {
            var ApprovalFlow = _context.ApprovalFlow
                    .Include(af => af.ApprovalLevels)
                    .Select(af => new ApprovalFlow_DTO
                    {
                        Id = af.Id,
                        Name = af.Name,
                        CreatedDate = af.CreatedDate,
                        ApprovalLevels = af.ApprovalLevels.Select(lv => new ApprovalLevel_DTO
                        {
                            Step = lv.Step,
                            RoleId = lv.RoleId,
                        }).ToList()
                    }).FirstOrDefault(af => af.Id == id);
            return ApprovalFlow;
        }

        public async Task Update_ApprovalFlow(int id, ApprovalFlow_DTO request)
        {
            var existingApprovalFlow = await _context.ApprovalFlow
                   .Include(af => af.ApprovalLevels)
                   .FirstOrDefaultAsync(af => af.Id == id);

            if (existingApprovalFlow == null)
            {
                throw new NotImplementedException("Không tìm thấy luồng phê duyệt này");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Tên luồng phê duyệt không được rỗng.");
            }

            if (request.CreatedDate == default(DateTime) || request.CreatedDate > DateTime.Now)
            {
                throw new ArgumentException("Ngày tạo không hợp lệ.");
            }

            if (request.ApprovalLevels == null || !request.ApprovalLevels.Any())
            {
                throw new ArgumentException("Phải có ít nhất một bước phê duyệt.");
            }

            foreach (var levelDto in request.ApprovalLevels)
            {
                if (levelDto.Step <= 0)
                {
                    throw new ArgumentException("Bước phê duyệt phải lớn hơn 0.");
                }

                if (levelDto.RoleId <= 0)
                {
                    throw new ArgumentException("ID vai trò phải lớn hơn 0.");
                }
            }
            // Cập nhật thông tin của luồng phê duyệt
            existingApprovalFlow.Name = request.Name;
            existingApprovalFlow.CreatedDate = request.CreatedDate;

            // Xử lý các bước phê duyệt (ApprovalLevels)
            // chuyển cac bước phê duyệt hiện tại thành dạng list
            var existingApprovalLevels = existingApprovalFlow.ApprovalLevels.ToList();
            foreach (var levelDto in request.ApprovalLevels)
            {
                var existingLevel = existingApprovalLevels.FirstOrDefault(al => al.Step == levelDto.Step);
                if (existingLevel != null)
                {
                    // Cập nhật bước phê duyệt hiện có
                    existingLevel.RoleId = levelDto.RoleId;
                }
                else
                {
                    // Thêm bước phê duyệt mới
                    var newLevel = new ApprovalLevels
                    {
                        Step = levelDto.Step,
                        RoleId = levelDto.RoleId,
                        FlowId = id
                    };
                    existingApprovalFlow.ApprovalLevels.Add(newLevel);
                }
            }
            // Xóa các bước phê duyệt trong danh danh sach,không có trong yêu cầu ng dùng gửi lên 
            foreach (var existingLevel in existingApprovalLevels)
            {
                if (!request.ApprovalLevels.Any(al => al.Step == existingLevel.Step))
                {
                    _context.ApprovalLevel.Remove(existingLevel);
                }
            }

            // Cập nhật luồng phê duyệt
            _context.ApprovalFlow.Update(existingApprovalFlow);
            await _context.SaveChangesAsync();
        }

        public async Task Delete_ApprovalFlow(int id)
        {
            var approvalFlow = await _context.ApprovalFlow
         .Include(af => af.ApprovalLevels)
         .Include(af => af.RequestDocuments)
         .FirstOrDefaultAsync(af => af.Id == id);

            // Kiểm tra xem luồng phê duyệt có đang được sử dụng bởi bất kỳ yêu cầu nào không
            if (approvalFlow.RequestDocuments.Any())
            {
                throw new InvalidOperationException("Luồng phê duyệt này đang được sử dụng bởi một hoặc nhiều yêu cầu và không thể xóa.");
            }
            // Xóa các cấp phê duyệt liên quan
            _context.ApprovalLevel.RemoveRange(approvalFlow.ApprovalLevels);

            // Xóa luồng phê duyệt
            _context.ApprovalFlow.Remove(approvalFlow);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApprovalFlow_DTO>> Seach_ApprovalFlows(string? name)
        {
            var ApprovalFlows = await _context.ApprovalFlow
                      .Include(af => af.ApprovalLevels)
                      .Where(af => string.IsNullOrEmpty(name) || af.Name.ToUpper().Contains(name.ToUpper()))
                      .Select(af => new ApprovalFlow_DTO
                      {
                          Id = af.Id,
                          Name = af.Name,
                          CreatedDate = af.CreatedDate,
                          ApprovalLevels = af.ApprovalLevels.Select(lv => new ApprovalLevel_DTO
                          {
                              Step = lv.Step,
                              RoleId = lv.RoleId,
                          }).ToList()
                      }).ToListAsync();
            return ApprovalFlows;
        }
     
    }
}
