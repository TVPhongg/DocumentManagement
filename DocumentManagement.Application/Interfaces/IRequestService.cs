using DocumentManagement.Application.DTOs;
using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IRequestService
    {
         Task Add_Request(Request_DTO request, IFormFile file, int userId);
        public Task<List<Approver_DTO>> Get_Approvers(int FlowId, int UserId);
        public Task<IEnumerable<Request_DTO>> Get_RequestDocument(int UserId);
        public Task Delete_Request(int id);
        public Task<IEnumerable<Request_DTO>> Search_Request(string? name , DateTime? StarDate , DateTime? EndDate);
        public Task Approval_Request(ApprovalAction_DTO action, int userId, int RequestId);
        public Task Reject_Request(ApprovalAction_DTO action , int userId, int RequestId);
    }
}
