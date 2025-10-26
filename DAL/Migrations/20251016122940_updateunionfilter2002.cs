using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateunionfilter2002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER FUNCTION [dbo].[ufn_GetUnionOrderItemsFiltered]
(
	@OrderItemID int =null,
    @OrderID INT = NULL,
    @ItemID INT = NULL,
    @ItemType NVARCHAR(20) = NULL,          -- 'Product' أو 'Material'
    @Name NVARCHAR(200) = NULL,
    @Description NVARCHAR(500) = NULL,
    @QuantityFrom FLOAT = NULL,
    @QuantityTo FLOAT = NULL,
    @SellingPriceFrom FLOAT = NULL,
    @SellingPriceTo FLOAT = NULL,
    @WholesalePriceFrom FLOAT = NULL,
    @WholesalePriceTo FLOAT = NULL,
    @UOMID INT = NULL,
    @CurrencyType INT = NULL,
    @ReservedQuantityFrom FLOAT = NULL,
    @ReservedQuantityTo FLOAT = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH UnionOrderItems AS
    (
        SELECT 
            oi.ID AS OrderItemID,
            oi.OrderID,
            p.ID AS ItemID,
            p.Name,
            p.Description,
            oi.Quantity,
            oi.SellingPrice,
            oi.PriceAdjustment,
            oi.WholesalePrice,
            p.UOMID,
            p.CurrencyType,
            p.ReservedQuantity,
            1 AS ItemType
        FROM OrderItems oi
        INNER JOIN Products p ON oi.ProductID = p.ID

        UNION ALL

        SELECT 
            rmi.ID AS OrderItemID,
            rmi.OrderID,
            rm.ID AS ItemID,
            rm.Name,
            rm.Description,
            rmi.Quantity,
            rmi.SellingPrice,
            NULL AS PriceAdjustment,
            rmi.WholesalePrice,
            rm.UOMID,
            rm.CurrencyTypeID AS CurrencyType,
            rm.ReservedQuantity,
            2 AS ItemType
        FROM RawMaterialOrderItems rmi
        INNER JOIN RawMaterials rm ON rmi.RawMaterialID = rm.ID
    )
    SELECT *
    FROM UnionOrderItems
    WHERE (@OrderID IS NULL OR OrderID = @OrderID)
	  and (@OrderItemID is null or OrderItemID= @OrderItemID)
      AND (@ItemID IS NULL OR ItemID = @ItemID)
      AND (@ItemType IS NULL OR ItemType = @ItemType)
      AND (@Name IS NULL OR Name LIKE '%' + @Name + '%')
      AND (@Description IS NULL OR Description LIKE '%' + @Description + '%')
      AND (@QuantityFrom IS NULL OR Quantity >= @QuantityFrom)
      AND (@QuantityTo IS NULL OR Quantity <= @QuantityTo)
      AND (@SellingPriceFrom IS NULL OR SellingPrice >= @SellingPriceFrom)
      AND (@SellingPriceTo IS NULL OR SellingPrice <= @SellingPriceTo)
      AND (@WholesalePriceFrom IS NULL OR WholesalePrice >= @WholesalePriceFrom)
      AND (@WholesalePriceTo IS NULL OR WholesalePrice <= @WholesalePriceTo)
      AND (@UOMID IS NULL OR UOMID = @UOMID)
      AND (@CurrencyType IS NULL OR CurrencyType = @CurrencyType)
      AND (@ReservedQuantityFrom IS NULL OR ReservedQuantity >= @ReservedQuantityFrom)
      AND (@ReservedQuantityTo IS NULL OR ReservedQuantity <= @ReservedQuantityTo)
);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
