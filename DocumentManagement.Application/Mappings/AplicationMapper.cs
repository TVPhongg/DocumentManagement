using AutoMapper;
using DocumentManagement.Application.DTOs;
using DocumentManagement.Domain.Entities;
namespace DocumentManagement.Application.Mappings
{
    public class AplicationMapper : Profile
    {
        public AplicationMapper()
        {
            CreateMap<ApprovalFlows, View_ApprovalFlow>().ReverseMap();
            CreateMap<ApprovalLevels, View_ApprovalLevel>().ReverseMap();
            CreateMap<ApprovalSteps, View_ApprovalStep>().ReverseMap();
            CreateMap<Request_Document, View_ApprovalRequest>().ReverseMap();
        }
    }

  
}
