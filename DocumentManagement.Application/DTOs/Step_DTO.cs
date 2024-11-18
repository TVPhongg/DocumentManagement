using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Step_DTO
    {
        public int Id {  get; set; }
        public int Step { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime UpdateTime { get; set; }
    
    
    }
}
