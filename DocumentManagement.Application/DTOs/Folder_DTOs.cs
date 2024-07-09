using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Folder_DTOs
    {
        public int Id { get; set; }
        public string Folders_name { get; set; }
        public DateTime Created_date { get; set; }
        public int User_id { get; set; }
        public string Folders_lever { get; set; }
    }
}
