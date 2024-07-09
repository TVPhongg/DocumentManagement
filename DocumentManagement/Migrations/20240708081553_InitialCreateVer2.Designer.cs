﻿// <auto-generated />
using System;
using DocumentManagement.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DocumentManagement.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240708081553_InitialCreateVer2")]
    partial class InitialCreateVer2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalFlows", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApprovalFlows");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalLevels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovalFlowId")
                        .HasColumnType("int");

                    b.Property<int>("ApprovalFlowsId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalFlowsId");

                    b.ToTable("ApprovalLevels");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalSteps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Action_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ApprovalLevel_id")
                        .HasColumnType("int");

                    b.Property<int>("ApproverUsers_id")
                        .HasColumnType("int");

                    b.Property<int>("Request_id")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.Property<int>("requestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalLevel_id")
                        .IsUnique();

                    b.HasIndex("ApproverUsers_id");

                    b.HasIndex("requestId");

                    b.ToTable("ApprovalSteps");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ApprovalSteps_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_date")
                        .HasColumnType("datetime2");

                    b.Property<int>("File_id")
                        .HasColumnType("int");

                    b.Property<int>("Foleders_id")
                        .HasColumnType("int");

                    b.Property<int>("Request_id")
                        .HasColumnType("int");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Request_Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Request_Document");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.Property<int>("Role_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Role_id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Roles_name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Role_id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Users", b =>
                {
                    b.Property<int>("Users_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Users_id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Last_name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password_hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role_id")
                        .HasColumnType("int");

                    b.Property<int>("rolesRole_id")
                        .HasColumnType("int");

                    b.HasKey("Users_id");

                    b.HasIndex("rolesRole_id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalLevels", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.ApprovalFlows", "ApprovalFlows")
                        .WithMany("ApprovalLevels")
                        .HasForeignKey("ApprovalFlowsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalFlows");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalSteps", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.ApprovalLevels", "ApprovalLevel")
                        .WithOne("ApprovalStep")
                        .HasForeignKey("DocumentManagement.Domain.Entities.ApprovalSteps", "ApprovalLevel_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DocumentManagement.Domain.Entities.Users", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverUsers_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DocumentManagement.Domain.Entities.Request_Document", "request")
                        .WithMany("ApprovalStep")
                        .HasForeignKey("requestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApprovalLevel");

                    b.Navigation("Approver");

                    b.Navigation("request");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Users", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.Roles", "roles")
                        .WithMany("users")
                        .HasForeignKey("rolesRole_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roles");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalFlows", b =>
                {
                    b.Navigation("ApprovalLevels");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalLevels", b =>
                {
                    b.Navigation("ApprovalStep")
                        .IsRequired();
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Request_Document", b =>
                {
                    b.Navigation("ApprovalStep");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.Navigation("users");
                });
#pragma warning restore 612, 618
        }
    }
}
