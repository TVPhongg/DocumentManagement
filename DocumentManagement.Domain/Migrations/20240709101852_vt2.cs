using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class vt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "Role_id1",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User",
                column: "Role_id1",
                principalTable: "Role",
                principalColumn: "Role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "Role_id1",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_Role_id1",
                table: "User",
                column: "Role_id1",
                principalTable: "Role",
                principalColumn: "Role_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
