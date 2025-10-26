using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // add Reserved quantit
            clsTVFMigration.AddReservedQuantityForProduct(migrationBuilder);

            // add New Tables
            clsTVFMigration.CreateRawMaterialsTable(migrationBuilder);
            clsTVFMigration.CreateRecipesTable(migrationBuilder);
            clsTVFMigration.CreateRecipeInfoTable(migrationBuilder);


            // add TVF
            clsTVFMigration.AddTVFRawMaterialsFilter(migrationBuilder);
            clsTVFMigration.AddTVFRecipesFilter(migrationBuilder);
            clsTVFMigration.AddTVFRecipeInfoFilter(migrationBuilder);
            // add New Tables
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
