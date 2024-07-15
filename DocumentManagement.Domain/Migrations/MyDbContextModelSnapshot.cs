﻿// <auto-generated />
using System;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

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

                    b.Property<int>("FlowId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlowId");

                    b.ToTable("ApprovalLevels");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalSteps", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.HasIndex("UserId");

                    b.ToTable("ApprovalSteps");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.FilePermissions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FilePermissions");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Files", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("FileSize")
                        .HasColumnType("real");

                    b.Property<int>("FoldersId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FoldersId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.FolderPermissions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FolderPermissions");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Folders", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FoldersLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Folders");
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

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.RequestDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovalFlowId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("File")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FlowId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalFlowId");

                    b.HasIndex("UserId");

                    b.ToTable("RequestDocument");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ApprovalLevelId")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ApprovalLevelId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.UserPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CheckAction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPermission");
                });

            modelBuilder.Entity("Users", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FilesId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("FilesId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalLevels", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.ApprovalFlows", "ApprovalFlow")
                        .WithMany("ApprovalLevels")
                        .HasForeignKey("FlowId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApprovalFlow");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalSteps", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.RequestDocument", "request")
                        .WithMany("ApprovalStep")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Users", "User")
                        .WithMany("ApprovalStep")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("request");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Files", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.Folders", "Folders")
                        .WithMany("File")
                        .HasForeignKey("FoldersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folders");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Folders", b =>
                {
                    b.HasOne("Users", "User")
                        .WithOne("Folders")
                        .HasForeignKey("DocumentManagement.Domain.Entities.Folders", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.RequestDocument", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.ApprovalFlows", "ApprovalFlow")
                        .WithMany("RequestDocument")
                        .HasForeignKey("ApprovalFlowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Users", "User")
                        .WithMany("RequestDocument")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ApprovalFlow");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.ApprovalLevels", "ApprovalLevel")
                        .WithMany("Role")
                        .HasForeignKey("ApprovalLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DocumentManagement.Domain.Entities.Department", null)
                        .WithMany("Role")
                        .HasForeignKey("DepartmentId");

                    b.Navigation("ApprovalLevel");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.UserPermission", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.Permission", "Permission")
                        .WithMany("UserPermission")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Users", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.Department", null)
                        .WithMany("users")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DocumentManagement.Domain.Entities.Files", "Files")
                        .WithMany("Users")
                        .HasForeignKey("FilesId");

                    b.HasOne("DocumentManagement.Domain.Entities.Roles", "roles")
                        .WithMany("users")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Files");

                    b.Navigation("roles");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalFlows", b =>
                {
                    b.Navigation("ApprovalLevels");

                    b.Navigation("RequestDocument");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.ApprovalLevels", b =>
                {
                    b.Navigation("Role");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Department", b =>
                {
                    b.Navigation("Role");

                    b.Navigation("users");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Files", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Folders", b =>
                {
                    b.Navigation("File");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Permission", b =>
                {
                    b.Navigation("UserPermission");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.RequestDocument", b =>
                {
                    b.Navigation("ApprovalStep");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.Navigation("users");
                });

            modelBuilder.Entity("Users", b =>
                {
                    b.Navigation("ApprovalStep");

                    b.Navigation("Folders");

                    b.Navigation("RequestDocument");
                });
#pragma warning restore 612, 618
        }
    }
}
