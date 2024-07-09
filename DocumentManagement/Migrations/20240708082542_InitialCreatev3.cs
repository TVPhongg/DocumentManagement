using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatev3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Foleders_FoledersId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Foleders");

            migrationBuilder.RenameColumn(
                name: "Foleders_id",
                table: "Files",
                newName: "Folders_id");

            migrationBuilder.RenameColumn(
                name: "FoledersId",
                table: "Files",
                newName: "FoldersId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FoledersId",
                table: "Files",
                newName: "IX_Files_FoldersId");

            migrationBuilder.AddColumn<int>(
                name: "FilesId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FoldersId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Folders_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Folders_lever = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_FilesId",
                table: "User",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_User_FoldersId",
                table: "User",
                column: "FoldersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_FoldersId",
                table: "Files",
                column: "FoldersId",
                principalTable: "Folders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Files_FilesId",
                table: "User",
                column: "FilesId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Folders_FoldersId",
                table: "User",
                column: "FoldersId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_FoldersId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Files_FilesId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Folders_FoldersId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_User_FilesId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_FoldersId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FilesId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FoldersId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Folders_id",
                table: "Files",
                newName: "Foleders_id");

            migrationBuilder.RenameColumn(
                name: "FoldersId",
                table: "Files",
                newName: "FoledersId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FoldersId",
                table: "Files",
                newName: "IX_Files_FoledersId");

            migrationBuilder.CreateTable(
                name: "Foleders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Folders_lever = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foleders_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foleders", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Foleders_FoledersId",
                table: "Files",
                column: "FoledersId",
                principalTable: "Foleders",
                principalColumn: "Id");
        }
    }
}
