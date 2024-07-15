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
        public DbSet<FilePermissions> FilePermission { get; set; }
        public DbSet<FolderPermissions> FolderPermission { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<UserPermission> UserPermission { get; set; }
        public DbSet<Folders> Folder { get; set; }
        public DbSet<Files> File { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Role relationship
            modelBuilder.Entity<Users>()
           .HasOne(u => u.roles)
           .WithMany(r => r.users)
           .HasForeignKey(u => u.Id)
           .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // RequestDocument - User relationship
            modelBuilder.Entity<RequestDocument>()
                .HasOne(rd => rd.User)
                .WithMany(u => u.RequestDocument)
                .HasForeignKey(rd => rd.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // ApprovalStep - User relationship
            modelBuilder.Entity<ApprovalSteps>()
                .HasOne(a => a.User)
                .WithMany(u => u.ApprovalStep)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // ApprovalStep - RequestDocument relationship
            modelBuilder.Entity<ApprovalSteps>()
                .HasOne(r => r.request)
                .WithMany(rd => rd.ApprovalStep)
                .HasForeignKey(r => r.RequestId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // ApprovalLevel - ApprovalFlow relationship
            modelBuilder.Entity<ApprovalLevels>()
                .HasOne(al => al.ApprovalFlow)
                .WithMany(af => af.ApprovalLevels)
                .HasForeignKey(al => al.FlowId)
                .OnDelete(DeleteBehavior.Restrict); // or NoAction
        
        }
    }
}
