using DocumentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Interfaces
{
    public interface IFlowService
    {
        public Task AddFlow (ApprovalFlow_DTO request);
        public Task<IEnumerable<ApprovalFlow_DTO>> Get_ApprovalFlows();

        public Task<ApprovalFlow_DTO> Get_ApprovalFlow(int id);
        public Task Update_ApprovalFlow(int id , ApprovalFlow_DTO request);
        public Task Delete_ApprovalFlow(int id);

        public Task<IEnumerable<ApprovalFlow_DTO>> Seach_ApprovalFlows(string? name);

    }
}
