﻿using Microsoft.EntityFrameworkCore;
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
<<<<<<< HEAD
        public DbSet<Request_Document> Request { get; set; }
        public DbSet<ApprovalFlows> ApprovalFlow { get; set; }
        public DbSet<ApprovalLevels> ApprovalLevel { get; set; }
        public DbSet<ApprovalSteps> ApprovalStep { get; set; }
        public DbSet<Users> User { get; set; } 
        public DbSet<Logs> Log { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalLevels>()
                .HasOne(al => al.ApprovalStep)
                .WithOne(as1 => as1.ApprovalLevel)
                .HasForeignKey<ApprovalSteps>(as1 => as1.ApprovalLevel_id);
        }
=======
        public DbSet<Files> File { get; set; }
        public DbSet<Foleders> Foleder { get; set; }
        public DbSet<Users> User { get; set; }
>>>>>>> f758459249cd26cd0ba411c424254ee7d0f3cc9a
    }
}
