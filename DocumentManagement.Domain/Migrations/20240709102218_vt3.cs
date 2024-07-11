using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class vt3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Role_id1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Role_id1",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_id",
                table: "User",
                column: "Role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Role_id",
                table: "User",
                column: "Role_id",
                principalTable: "Role",
                principalColumn: "Role_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Role_id",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Role_id",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Role_id1",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Role_id1",
                table: "User",
                column: "Role_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User",
                column: "Role_id1",
                principalTable: "Role",
                principalColumn: "Role_id");
        }
    }
}
