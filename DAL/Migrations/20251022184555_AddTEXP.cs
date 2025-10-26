using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTEXP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // إنشاء جدول أنواع المصروفات
            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.ID);
                });

            // إنشاء جدول المصروفات
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseDate = table.Column<DateTime>(nullable: false),
                    ExpenseTypeID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    ActionByUser = table.Column<string>(nullable: false),
                    ActionType = table.Column<byte>(nullable: false),
                    ActionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ID);

                    // FK إلى جدول أنواع المصروفات
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseTypes_ExpenseTypeID",
                        column: x => x.ExpenseTypeID,
                        principalTable: "ExpenseTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);

                    // FK إلى جدول المستخدمين AspNetUsers
                    table.ForeignKey(
                        name: "FK_Expenses_AspNetUsers_ActionByUser",
                        column: x => x.ActionByUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // الفهارس
            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseTypeID",
                table: "Expenses",
                column: "ExpenseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ActionByUser",
                table: "Expenses",
                column: "ActionByUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");
        }
    }
}
