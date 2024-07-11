using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class vt4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_User_Users_id",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_Users_id",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "Users_id",
                table: "Folders");

            migrationBuilder.AlterColumn<string>(
                name: "Folders_name",
                table: "Folders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Folders_lever",
                table: "Folders",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_User_id",
                table: "Folders",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_User_User_id",
                table: "Folders",
                column: "User_id",
                principalTable: "User",
                principalColumn: "Users_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_User_User_id",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_User_id",
                table: "Folders");

            migrationBuilder.AlterColumn<string>(
                name: "Folders_name",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Folders_lever",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Users_id",
                table: "Folders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Folders_Users_id",
                table: "Folders",
                column: "Users_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_User_Users_id",
                table: "Folders",
                column: "Users_id",
                principalTable: "User",
                principalColumn: "Users_id");
        }
    }
}
