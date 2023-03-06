using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS03.EntityModels.ORM.Migrations
{
    public partial class EMSUpdate001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Sex = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    EquipmentCategoryId = table.Column<Guid>(nullable: true),
                    SituationEnum = table.Column<int>(nullable: false),
                    IsScrap = table.Column<bool>(nullable: false),
                    Islend = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_EquipmentCategory_EquipmentCategoryId",
                        column: x => x.EquipmentCategoryId,
                        principalTable: "EquipmentCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    Account = table.Column<string>(maxLength: 16, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsRoot = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    PassTime = table.Column<DateTime>(nullable: true),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Login_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionNotification_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentReport",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    IsRepair = table.Column<bool>(nullable: false),
                    RepairUserId = table.Column<Guid>(nullable: true),
                    ReportUserId = table.Column<Guid>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentReport_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReport_Users_RepairUserId",
                        column: x => x.RepairUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentReport_Users_ReportUserId",
                        column: x => x.ReportUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentScrap",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentScrap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentScrap_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUseWithUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UseUserId = table.Column<Guid>(nullable: true),
                    EquipmentId = table.Column<Guid>(nullable: true),
                    IsReturn = table.Column<bool>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: true),
                    ReturnSituation = table.Column<int>(nullable: false),
                    ExamineEnum = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUseWithUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUseWithUser_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUseWithUser_Users_UseUserId",
                        column: x => x.UseUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_EquipmentCategoryId",
                table: "Equipment",
                column: "EquipmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReport_EquipmentId",
                table: "EquipmentReport",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReport_RepairUserId",
                table: "EquipmentReport",
                column: "RepairUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentReport_ReportUserId",
                table: "EquipmentReport",
                column: "ReportUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentScrap_EquipmentId",
                table: "EquipmentScrap",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUseWithUser_EquipmentId",
                table: "EquipmentUseWithUser",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUseWithUser_UseUserId",
                table: "EquipmentUseWithUser",
                column: "UseUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Login_UserId",
                table: "Login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionNotification_UserId",
                table: "TransactionNotification",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentReport");

            migrationBuilder.DropTable(
                name: "EquipmentScrap");

            migrationBuilder.DropTable(
                name: "EquipmentUseWithUser");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "TransactionNotification");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EquipmentCategory");
        }
    }
}
