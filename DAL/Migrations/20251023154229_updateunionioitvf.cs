using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateunionioitvf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_GetUnionImportOrderItemsFiltered]    Script Date: 01/05/47 06:39:47 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[ufn_GetUnionImportOrderItemsFiltered]
(
    @ImportOrderItemID INT = NULL,
    @ImportOrderID INT = NULL,
    @ItemID INT = NULL,
    @ItemType INT = NULL,           -- 1 = Product, 2 = RawMaterial
    @ItemName NVARCHAR(200) = NULL,
    @Description NVARCHAR(500) = NULL,
    @QuantityFrom FLOAT = NULL,
    @QuantityTo FLOAT = NULL,
    @SellingPriceFrom FLOAT = NULL,
    @SellingPriceTo FLOAT = NULL,
    @WholesalePriceFrom FLOAT = NULL,
    @WholesalePriceTo FLOAT = NULL,
    @AvailableQuantityFrom FLOAT = NULL,
    @AvailableQuantityTo FLOAT = NULL,
    @ImportDateFrom DATE = NULL,
    @ImportDateTo DATE = NULL,
    @SupplierName NVARCHAR(300) = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH UnionImportOrderItems AS
    (
        -- ✅ المنتجات المستوردة
        SELECT 
            ioi.ID AS ImportOrderItemID,
            ioi.ImportOrderID,
            p.ID AS ItemID,
            p.Name AS ItemName,
            p.Description,
            ioi.Quantity,
            ioi.SellingPrice,
            p.WholesalePrice,
			p.UOMID,
            p.CurrencyType,
            p.AvailableQuantity,
            s.StoreName AS SupplierName,
            io.ImportDate,
            1 AS ItemType
        FROM ImportOrderItems AS ioi
        INNER JOIN Products AS p ON ioi.ProductID = p.ID
        INNER JOIN ImportOrders AS io ON ioi.ImportOrderID = io.ID
        INNER JOIN Suppliers AS s ON io.SupplierID = s.ID

        UNION ALL

        -- ✅ المواد الخام المستوردة
        SELECT 
            irmi.ID AS ImportOrderItemID,
            irmi.ImportOrderID,
            rm.ID AS ItemID,
            rm.Name AS ItemName,
            rm.Description,
            irmi.Quantity,
            irmi.SellingPrice,
            rm.PurchasePrice AS WholesalePrice,
			rm.UOMID,
            rm.CurrencyTypeID AS CurrencyType,
            rm.AvailableQuantity,
            s.StoreName AS SupplierName,
            io.ImportDate,
            2 AS ItemType
        FROM ImportRawMaterialItems AS irmi
        INNER JOIN RawMaterials AS rm ON irmi.RawMaterialID = rm.ID
        INNER JOIN ImportOrders AS io ON irmi.ImportOrderID = io.ID
        INNER JOIN Suppliers AS s ON io.SupplierID = s.ID
    )
    SELECT *
    FROM UnionImportOrderItems
    WHERE 1 = 1
      AND (@ImportOrderItemID IS NULL OR ImportOrderItemID = @ImportOrderItemID)
      AND (@ImportOrderID IS NULL OR ImportOrderID = @ImportOrderID)
      AND (@ItemID IS NULL OR ItemID = @ItemID)
      AND (@ItemType IS NULL OR ItemType = @ItemType)
      AND (@ItemName IS NULL OR ItemName LIKE '%' + @ItemName + '%')
      AND (@Description IS NULL OR Description LIKE '%' + @Description + '%')
      AND (@QuantityFrom IS NULL OR Quantity >= @QuantityFrom)
      AND (@QuantityTo IS NULL OR Quantity <= @QuantityTo)
      AND (@SellingPriceFrom IS NULL OR SellingPrice >= @SellingPriceFrom)
      AND (@SellingPriceTo IS NULL OR SellingPrice <= @SellingPriceTo)
      AND (@WholesalePriceFrom IS NULL OR WholesalePrice >= @WholesalePriceFrom)
      AND (@WholesalePriceTo IS NULL OR WholesalePrice <= @WholesalePriceTo)
      AND (@AvailableQuantityFrom IS NULL OR AvailableQuantity >= @AvailableQuantityFrom)
      AND (@AvailableQuantityTo IS NULL OR AvailableQuantity <= @AvailableQuantityTo)
      AND (@ImportDateFrom IS NULL OR ImportDate >= @ImportDateFrom)
      AND (@ImportDateTo IS NULL OR ImportDate <= @ImportDateTo)
      AND (@SupplierName IS NULL OR SupplierName LIKE '%' + @SupplierName + '%')
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
