using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class GGGGG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           clsTVFMigration.AddGetTotalStockValue_SP(migrationBuilder);
            clsTVFMigration.AddWholesalePriceColumnToOI(migrationBuilder);
            clsTVFMigration.AddGetNetProfit_SP(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
