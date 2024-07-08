using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foleders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Foleders_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Folders_lever = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foleders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Foleders_id = table.Column<int>(type: "int", nullable: false),
                    File_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    File_size = table.Column<float>(type: "real", nullable: false),
                    FoledersId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Foleders_FoledersId",
                        column: x => x.FoledersId,
                        principalTable: "Foleders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FoledersId",
                table: "Files",
                column: "FoledersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Foleders");
        }
    }
}
