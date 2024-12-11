using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentManagement.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Domain.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<RequestDocument> Request { get; set; }
        public DbSet<ApprovalFlows> ApprovalFlow { get; set; }
        public DbSet<ApprovalLevels> ApprovalLevel { get; set; }
        public DbSet<ApprovalSteps> ApprovalStep { get; set; }
        public DbSet<Users> User { get; set; } 
        public DbSet<Logs> Log { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Folders> Folder { get; set; }
        public DbSet<Files> File { get; set; }
        public DbSet<Tasks> Task { get; set; }
        public DbSet<Projects> Project {get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            // ApprovalStep - RequestDocument relationship
            modelBuilder.Entity<ApprovalSteps>()
                .HasOne(r => r.request) 
                .WithMany(rd => rd.ApprovalSteps)
                .HasForeignKey(r => r.RequestId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // ApprovalLevel - ApprovalFlow relationship
            modelBuilder.Entity<ApprovalLevels>()
                .HasOne(al => al.ApprovalFlow)
                .WithMany(af => af.ApprovalLevels)
                .HasForeignKey(al => al.FlowId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            modelBuilder.Entity<ApprovalLevels>()
                .HasOne(r => r.Role)
                .WithMany(al => al.ApprovalLevels)
                .HasForeignKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestDocument>()
                .HasOne(r => r.ApprovalFlow)
                .WithMany(al => al.RequestDocuments)
                .HasForeignKey(r => r.FlowId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
