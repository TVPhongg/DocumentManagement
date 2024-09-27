using DocumentManagement.Application.DTOs;
using DocumentManagement.Domain.Entities;
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
        public Task Add_Request(Request_DTO request);
        public Task<List<Approver_DTO>> Get_Approvers(int FlowId, int UserId);
        public Task<IEnumerable<Request_DTO>> Get_RequestDocument(int pageNumber, int pageSize);
        public Task Delete_Request(int id);
        public Task<IEnumerable<Request_DTO>> Search_Request(string? name , DateTime? StarDate , DateTime? EndDate);
        public Task Approval_Request(ApprovalAction_DTO action);
        public Task Reject_Request(ApprovalAction_DTO action);
    }
}
