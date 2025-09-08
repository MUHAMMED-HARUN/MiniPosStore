using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateORderItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
clsTVFMigration.AddColumnPriceAdjustmentToOrderItem(migrationBuilder);
            clsTVFMigration.AddTVFCustomersFilter(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceAdjustment",
                table: "OrderItems");
        }
    }
}
