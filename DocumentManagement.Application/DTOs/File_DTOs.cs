using DocumentManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class File_DTOs
    {
        public int Id { get; set; }
        public int FoldersId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UserName { get; set; }
        public int UserId { get; set; }
        public decimal FileSize { get; set; }
        public List<IFormFile> file { get; set; }
    }
}
