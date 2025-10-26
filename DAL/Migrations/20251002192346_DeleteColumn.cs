using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE RecipeInfos\r\nDROP COLUMN ProductionLossQuantity;");
            migrationBuilder.Sql(@"
 create or ALTER FUNCTION [dbo].[GetRecipeInfoFiltered]
(
    @id int = null,
    @RecipeName NVARCHAR(200) = NULL,
    @MaterialName NVARCHAR(200) = NULL,
    @RequiredMaterialQuantityFrom DECIMAL(18,2) = NULL,
    @RequiredMaterialQuantityTo DECIMAL(18,2) = NULL,
    @ActionDateFrom DATETIME = NULL,
    @ActionDateTo DATETIME = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT
        ri.ID,
        ri.RecipeID,
        r.Name AS RecipeName,
        ri.RawMaterialID AS MaterialID,
        rm.Name AS MaterialName,
        ri.RequiredMaterialQuantity,
        ri.ActionDate
    FROM dbo.RecipeInfos ri
    INNER JOIN dbo.Recipes r ON ri.RecipeID = r.ID
    INNER JOIN dbo.RawMaterials rm ON ri.RawMaterialID = rm.ID
    WHERE
        (ri.ActionType != 3) and
        (@id IS NULL OR ri.id = @id) AND
        (@RecipeName IS NULL OR r.Name LIKE '%' + @RecipeName + '%') AND
        (@MaterialName IS NULL OR rm.Name LIKE '%' + @MaterialName + '%') AND
        (@RequiredMaterialQuantityFrom IS NULL OR ri.RequiredMaterialQuantity >= @RequiredMaterialQuantityFrom) AND
        (@RequiredMaterialQuantityTo IS NULL OR ri.RequiredMaterialQuantity <= @RequiredMaterialQuantityTo) AND
        (@ActionDateFrom IS NULL OR ri.ActionDate >= @ActionDateFrom) AND
        (@ActionDateTo IS NULL OR ri.ActionDate <= @ActionDateTo)
);

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
