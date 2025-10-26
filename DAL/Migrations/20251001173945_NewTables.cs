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
            migrationBuilder.Sql(@"
CREATE OR ALTER FUNCTION [dbo].[GetRawMaterialsFiltered]
(
    @id int = null,
    @Name NVARCHAR(200) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @PurchasePriceFrom DECIMAL(18,2) = NULL,
    @PurchasePriceTo DECIMAL(18,2) = NULL,
    @AvailableQuantityFrom DECIMAL(18,2) = NULL,
    @AvailableQuantityTo DECIMAL(18,2) = NULL,
    @UOMID INT = NULL,
    @CurrencyTypeID INT = NULL,
    @SupplierName NVARCHAR(200) = NULL,
    @ActionDateFrom DATETIME = NULL,
    @ActionDateTo DATETIME = NULL

)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT
        rm.ID,
        rm.Name, 
        rm.Description, 
        rm.PurchasePrice, 
        rm.AvailableQuantity, 
        rm.UOMID,
        u.Name AS UOMName,
        rm.CurrencyTypeID,
        rm.MaterialSupplier,
        p.FirstName + ' ' + p.LastName AS SupplierName,
        rm.ActionDate
    FROM dbo.RawMaterials rm
    INNER JOIN dbo.UnitOfMeasures u ON rm.UOMID = u.ID
    LEFT JOIN dbo.Suppliers s ON rm.MaterialSupplier = s.ID
    LEFT JOIN dbo.People p ON s.PersonID = p.ID
    WHERE
        (rm.ActionType != 3) AND
        (@id IS NULL OR rm.id = @id) AND
        (@Name IS NULL OR rm.Name LIKE '%' + @Name + '%') AND
        (@Description IS NULL OR rm.Description LIKE '%' + @Description + '%') AND
        (@PurchasePriceFrom IS NULL OR rm.PurchasePrice >= @PurchasePriceFrom) AND
        (@PurchasePriceTo IS NULL OR rm.PurchasePrice <= @PurchasePriceTo) AND
        (@AvailableQuantityFrom IS NULL OR rm.AvailableQuantity >= @AvailableQuantityFrom) AND
        (@AvailableQuantityTo IS NULL OR rm.AvailableQuantity <= @AvailableQuantityTo) AND
        (@UOMID IS NULL OR rm.UOMID = @UOMID) AND
        (@CurrencyTypeID IS NULL OR rm.CurrencyTypeID = @CurrencyTypeID) AND
        (@SupplierName IS NULL OR (p.FirstName + ' ' + p.LastName) LIKE '%' + @SupplierName + '%') AND
        (@ActionDateFrom IS NULL OR rm.ActionDate >= @ActionDateFrom) AND
        (@ActionDateTo IS NULL OR rm.ActionDate <= @ActionDateTo) 
);
");
            //clsTVFMigration.AddTVFRecipesFilter(migrationBuilder);
            //clsTVFMigration.AddTVFRecipeInfoFilter(migrationBuilder);
            // add New Tables
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
