using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Infra.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<Files> File { get; set; }

    }
}
