﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Application.DTOs
{
    public class FolderPermissionDTOs
    {
        public int FolderId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
