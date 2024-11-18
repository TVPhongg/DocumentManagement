using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Request_DTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Files { get; set; }
        public IFormFile File { get; set; }
        public int FlowId { get; set; }
        public int UserId { get; set; }
        public int status { get; set; }
        public List<Step_DTO> ApprovalSteps { get; set; }

    }
}
