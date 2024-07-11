using DocumentManagement.Domain.Entities;
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
        public int Folders_id { get; set; }
        public string File_name { get; set; }
        public string File_path { get; set; }
        public DateTime Created_date { get; set; }
        public int User_id { get; set; }
        public float File_size { get; set; }

    }
}
