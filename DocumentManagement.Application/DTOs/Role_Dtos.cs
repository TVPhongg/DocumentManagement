using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class Role_Dtos
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
