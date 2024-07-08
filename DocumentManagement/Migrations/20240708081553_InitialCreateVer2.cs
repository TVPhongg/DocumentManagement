using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateVer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.CreateTable(
                name: "ApprovalFlows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalFlows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request_Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request_Document", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ApprovalFlowId = table.Column<int>(type: "int", nullable: false),
                    ApprovalFlowsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalLevels_ApprovalFlows_ApprovalFlowsId",
                        column: x => x.ApprovalFlowsId,
                        principalTable: "ApprovalFlows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Request_id = table.Column<int>(type: "int", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    ApprovalLevel_id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    requestId = table.Column<int>(type: "int", nullable: false),
                    ApproverUsers_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalSteps_ApprovalLevels_ApprovalLevel_id",
                        column: x => x.ApprovalLevel_id,
                        principalTable: "ApprovalLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalSteps_Request_Document_requestId",
                        column: x => x.requestId,
                        principalTable: "Request_Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovalSteps_User_ApproverUsers_id",
                        column: x => x.ApproverUsers_id,
                        principalTable: "User",
                        principalColumn: "Users_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevels_ApprovalFlowsId",
                table: "ApprovalLevels",
                column: "ApprovalFlowsId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSteps_ApprovalLevel_id",
                table: "ApprovalSteps",
                column: "ApprovalLevel_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSteps_ApproverUsers_id",
                table: "ApprovalSteps",
                column: "ApproverUsers_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalSteps_requestId",
                table: "ApprovalSteps",
                column: "requestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalSteps");

            migrationBuilder.DropTable(
                name: "ApprovalLevels");

            migrationBuilder.DropTable(
                name: "Request_Document");

            migrationBuilder.DropTable(
                name: "ApprovalFlows");

            migrationBuilder.CreateTable(
                name: "Folders",
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
                    table.PrimaryKey("PK_Folders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoledersId = table.Column<int>(type: "int", nullable: true),
                    Created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    File_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File_size = table.Column<float>(type: "real", nullable: false),
                    Foleders_id = table.Column<int>(type: "int", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Folders_FoledersId",
                        column: x => x.FoledersId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FoledersId",
                table: "Files",
                column: "FoledersId");
        }
    }
}
