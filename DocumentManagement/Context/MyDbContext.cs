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
        public DbSet<Folders> Folder { get; set; }
        public DbSet<Users> User { get; set; }
        public DbSet<ApprovalFlows> ApprovalFlows { get; set; }
        public DbSet<ApprovalLevels> ApprovalLevels { get; set; }
        public DbSet<ApprovalSteps> ApprovalSteps { get; set; }
        public DbSet<Request_Document> Request_Document { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalLevels>()
                .HasOne(al => al.ApprovalStep)
                .WithOne(as1 => as1.approvalLevel)
                .HasForeignKey<ApprovalSteps>(as1 => as1.ApprovalLevel_id);
        }
    }
}
