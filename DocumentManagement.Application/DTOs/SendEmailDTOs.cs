using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class SendEmailDTOs
    {
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
