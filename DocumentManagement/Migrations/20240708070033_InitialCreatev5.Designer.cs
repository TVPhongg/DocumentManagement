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
    [Migration("20240708070033_InitialCreatev5")]
    partial class InitialCreatev5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Files", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("File_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("File_path")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("File_size")
                        .HasColumnType("real");

                    b.Property<int?>("FoledersId")
                        .HasColumnType("int");

                    b.Property<int>("Foleders_id")
                        .HasColumnType("int");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FoledersId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Foleders", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Folders_lever")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foleders_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("User_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Foleders");
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

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Files", b =>
                {
                    b.HasOne("DocumentManagement.Domain.Entities.Foleders", "Foleders")
                        .WithMany("File")
                        .HasForeignKey("FoledersId");

                    b.Navigation("Foleders");
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

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Foleders", b =>
                {
                    b.Navigation("File");
                });

            modelBuilder.Entity("DocumentManagement.Domain.Entities.Roles", b =>
                {
                    b.Navigation("users");
                });
#pragma warning restore 612, 618
        }
    }
}
