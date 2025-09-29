using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Migrations
{
    internal class clsTVFMigration
    {
        public static void AddTVFProductFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[GetProductsFiltered]    Script Date: 01/04/47 02:02:32 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  FUNCTION [dbo].[GetProductsFiltered]
                    (
                        @id int =null,
                        @Name NVARCHAR(200) = NULL,
                        @Description NVARCHAR(MAX) = NULL,
                        @MinRetailPrice DECIMAL(18,2) = NULL,
                        @MaxRetailPrice DECIMAL(18,2) = NULL,
                        @MinWholesalePrice DECIMAL(18,2) = NULL, 
                        @MaxWholesalePrice DECIMAL(18,2) = NULL,
                        @MinAvailableQuantity INT = NULL,
                        @MaxAvailableQuantity INT = NULL,
                        @CurrencyType NVARCHAR(50) = NULL,
                        @UnitOfMeasureName NVARCHAR(200) = NULL
                    )
                    RETURNS TABLE
                    AS
                    RETURN
                    (
                        SELECT DISTINCT
                            p.ID,
                            p.Name, 
                            p.Description, 
                            p.RetailPrice, 
                            p.WholesalePrice, 
                            p.AvailableQuantity, 
                            p.CurrencyType, 
                            p.ImagePath,
							P.UOMID,
                            u.Name AS UnitOfMeasureName
                        FROM dbo.Products p
                        INNER JOIN dbo.UnitOfMeasures u ON p.UOMID = u.ID
                        WHERE
                            (p.ActionType !=3) and
                            (@id IS NULL OR p.id = @id) AND
                            (@Name IS NULL OR p.Name LIKE '%' + @Name + '%') AND
                            (@Description IS NULL OR p.Description LIKE '%' + @Description + '%') AND
        
                            (@MinRetailPrice IS NULL OR p.RetailPrice >= @MinRetailPrice) AND
                            (@MaxRetailPrice IS NULL OR p.RetailPrice <= @MaxRetailPrice) AND

                            (@MinWholesalePrice IS NULL OR p.WholesalePrice >= @MinWholesalePrice) AND
                            (@MaxWholesalePrice IS NULL OR p.WholesalePrice <= @MaxWholesalePrice) AND

                            (@MinAvailableQuantity IS NULL OR p.AvailableQuantity >= @MinAvailableQuantity) AND
                            (@MaxAvailableQuantity IS NULL OR p.AvailableQuantity <= @MaxAvailableQuantity) AND

                            (@CurrencyType IS NULL OR p.CurrencyType = @CurrencyType) AND
                            (@UnitOfMeasureName IS NULL OR u.Name LIKE '%' + @UnitOfMeasureName + '%')
                    );




");
        }

        public static void AddTVFOrderFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[GetOrdersFiltred]    Script Date: 01/04/47 02:02:11 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[GetOrdersFiltred]
(
    @OrderID INT = NULL,
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @StartTotalAmount DECIMAL(18,2) = NULL,
    @EndTotalAmount DECIMAL(18,2) = NULL,
    @StartPaidAmount DECIMAL(18,2) = NULL,
    @EndPaidAmount DECIMAL(18,2) = NULL,
    @PaymentStatus NVARCHAR(50) = NULL,
    @ActionByUser NVARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        o.ID AS OrderID,
        o.CustomerID,
        p.ID AS PersonID,
        p.FirstName,
        p.LastName,
        p.PhoneNumber,
        o.OrderDate,
        o.TotalAmount,
        o.PaidAmount,
        o.PaymentStatus,
        o.ActionByUser
    FROM dbo.Orders o
    INNER JOIN dbo.Customers c ON o.CustomerID = c.ID
    INNER JOIN dbo.People p ON c.PersonID = p.ID
    WHERE (@OrderID IS NULL OR o.ID = @OrderID)
      AND (@FirstName IS NULL OR p.FirstName LIKE '%' + @FirstName + '%')
      AND (@LastName IS NULL OR p.LastName LIKE '%' + @LastName + '%')
      AND (@StartDate IS NULL OR o.OrderDate >= @StartDate)
      AND (@EndDate IS NULL OR o.OrderDate <= @EndDate)
      AND (@StartTotalAmount IS NULL OR o.TotalAmount >= @StartTotalAmount)
      AND (@EndTotalAmount IS NULL OR o.TotalAmount <= @EndTotalAmount)
      AND (@StartPaidAmount IS NULL OR o.PaidAmount >= @StartPaidAmount)
      AND (@EndPaidAmount IS NULL OR o.PaidAmount <= @EndPaidAmount)
      AND (@PaymentStatus IS NULL OR o.PaymentStatus = @PaymentStatus)
      AND (@ActionByUser IS NULL OR o.ActionByUser = @ActionByUser)
);
");
        }

        public static void AddTVFImportOrderFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[GetImportOrdersFiltered]    Script Date: 01/04/47 02:01:43 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[GetImportOrdersFiltered]
(
	@IOID int =null,
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @PhoneNumber NVARCHAR(50) = NULL,

    @StartImportDate DATETIME = NULL,
    @EndImportDate DATETIME = NULL,

    @StartTotalAmount DECIMAL(18,2) = NULL,
    @EndTotalAmount DECIMAL(18,2) = NULL,

    @StartPaidAmount DECIMAL(18,2) = NULL,
    @EndPaidAmount DECIMAL(18,2) = NULL,

    @PaymentStatus NVARCHAR(50) = NULL,

    @StartItemsCount INT = NULL,
    @EndItemsCount INT = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        io.ID as ImportOrderID,
        io.SupplierID,
        io.TotalAmount,
        io.PaidAmount,
        io.ImportDate,
        io.PaymentStatus,
        io.ActionByUser,
        s.StoreAddress,
        p.FirstName,
        p.LastName,
        p.PhoneNumber,
        (
            SELECT COUNT(*) 
            FROM dbo.ImportOrderItems ioi 
            WHERE ioi.ImportOrderID = io.ID
        ) AS ItemsCount
    FROM dbo.ImportOrders io
    INNER JOIN dbo.Suppliers s ON io.SupplierID = s.ID
    INNER JOIN dbo.People p ON s.PersonID = p.ID
    WHERE 
		  (@IOID IS NULL OR IO.ID=@IOID)
	  AND (@FirstName IS NULL OR p.FirstName LIKE '%' + @FirstName + '%')
      AND (@LastName IS NULL OR p.LastName LIKE '%' + @LastName + '%')
      AND (@PhoneNumber IS NULL OR p.PhoneNumber LIKE '%' + @PhoneNumber + '%')
      AND (@StartImportDate IS NULL OR io.ImportDate >= @StartImportDate)
      AND (@EndImportDate IS NULL OR io.ImportDate <= @EndImportDate)
      AND (@StartTotalAmount IS NULL OR io.TotalAmount >= @StartTotalAmount)
      AND (@EndTotalAmount IS NULL OR io.TotalAmount <= @EndTotalAmount)
      AND (@StartPaidAmount IS NULL OR io.PaidAmount >= @StartPaidAmount)
      AND (@EndPaidAmount IS NULL OR io.PaidAmount <= @EndPaidAmount)
      AND (@PaymentStatus IS NULL OR io.PaymentStatus = @PaymentStatus)
      AND (
            @StartItemsCount IS NULL OR 
            (SELECT COUNT(*) FROM dbo.ImportOrderItems ioi WHERE ioi.ImportOrderID = io.ID) >= @StartItemsCount
          )
      AND (
            @EndItemsCount IS NULL OR 
            (SELECT COUNT(*) FROM dbo.ImportOrderItems ioi WHERE ioi.ImportOrderID = io.ID) <= @EndItemsCount
          )
);
");
        }
        public static void AddTVFCustomersFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[GetCustomersFiltred]    Script Date: 01/04/47 02:00:39 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create FUNCTION [dbo].[GetCustomersFiltred]
(
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @PhoneNumber NVARCHAR(50) = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        c.ID as CustomerID,
        c.PersonID,
        p.FirstName,
        p.LastName,
        p.PhoneNumber,
        SUM(o.TotalAmount - o.PaidAmount) AS RemainingAmount
    FROM dbo.Customers c
    left JOIN dbo.Orders o ON c.ID = o.CustomerID
    INNER JOIN dbo.People p ON c.PersonID = p.ID
    WHERE 
        (@FirstName IS NULL OR p.FirstName LIKE '%' + @FirstName + '%')
        AND (@LastName IS NULL OR p.LastName LIKE '%' + @LastName + '%')
        AND (@PhoneNumber IS NULL OR p.PhoneNumber LIKE '%' + @PhoneNumber + '%')
    GROUP BY c.ID, c.PersonID, p.FirstName, p.LastName, p.PhoneNumber
);
");
        }
        public static void AddGetTopProductsID(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"-- هذه الطريقة تضمن أن الدالة موجودة بدون أخطاء
CREATE OR ALTER FUNCTION dbo.GetTopProductsID
(
    @StartDate DATE,
    @EndDate   DATE
)
RETURNS TABLE
AS
RETURN
(
    SELECT TOP 10
        p.ID AS ProductID,
        SUM(oi.Quantity) AS TotalSold
    FROM Orders o
    JOIN OrderItems oi ON oi.OrderID = o.ID
    JOIN Products p ON p.ID = oi.ProductID
    WHERE o.PaymentStatus IN (1,2,3)
      AND o.OrderDate BETWEEN @StartDate AND @EndDate
    GROUP BY p.ID
    ORDER BY TotalSold DESC
);
");
        }
        public static void AddGetTopProductsFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"USE [MiniPosStore]
GO
/****** Object:  UserDefinedFunction [dbo].[GetTopProducts]    Script Date: 01/04/47 01:59:32 م ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create FUNCTION [dbo].[GetTopProducts]
(
    -- Parameters for filtering products
    @id INT = NULL,
    @Name NVARCHAR(200) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @MinRetailPrice DECIMAL(18,2) = NULL,
    @MaxRetailPrice DECIMAL(18,2) = NULL,
    @MinWholesalePrice DECIMAL(18,2) = NULL, 
    @MaxWholesalePrice DECIMAL(18,2) = NULL,
    @MinAvailableQuantity INT = NULL,
    @MaxAvailableQuantity INT = NULL,
    @CurrencyType NVARCHAR(50) = NULL,
    @UnitOfMeasureName NVARCHAR(200) = NULL,
    
    -- Parameters for top products (date range)
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        FP.ID AS ProductID,
        FP.Name AS ProductName,
        FP.Description,
        FP.RetailPrice,
        FP.WholesalePrice,
        FP.AvailableQuantity,
        FP.CurrencyType,
        FP.ImagePath,
        FP.UnitOfMeasureName,
		FP.UOMID,
        TP.TotalSold
    FROM dbo.GetProductsFiltered(
            @id, @Name, @Description, @MinRetailPrice, @MaxRetailPrice,
            @MinWholesalePrice, @MaxWholesalePrice, @MinAvailableQuantity,
            @MaxAvailableQuantity, @CurrencyType, @UnitOfMeasureName
         ) AS FP
    INNER JOIN dbo.GetTopProductsID(@StartDate, @EndDate) AS TP
        ON FP.ID = TP.ProductID
);
");
        }
        public static void AddGetNetProfit_SP(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create or  ALTER PROCEDURE [dbo].[GetNetProfit]
    @TargetDate DATE,
	@TargetOrder int =null,
	@TargetProduct int =null
	
	AS
BEGIN
    SET NOCOUNT ON;

    WITH ProfitCTE AS (
        SELECT 
            ((oi.SellingPrice - oi.WholesalePrice) * oi.Quantity)
            - ISNULL(oi.PriceAdjustment, 0) AS Profit
        FROM Orders AS o
        JOIN OrderItems AS oi
            ON oi.OrderID = o.ID
        JOIN Products AS p
            ON p.ID = oi.ProductID
        WHERE @TargetDate  is null  or CAST(o.OrderDate AS DATE) = @TargetDate 
		and @TargetOrder is null or o.ID = @TargetOrder
		and @TargetProduct is null or p.ID=@TargetProduct
    )
    SELECT 
        SUM(Profit) AS NetProfit
    FROM ProfitCTE;
END
");
        }
        public static void AddGetTotalStockValue_SP(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

CREATE or alter PROCEDURE dbo.GetTotalStockValue
    @ProductID INT = NULL  -- إذا تم تمرير قيمة، يتم حساب هذا المنتج فقط
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SUM(p.WholesalePrice * p.AvailableQuantity) AS TotalStockValue
    FROM Products AS p
    WHERE (@ProductID IS NULL OR p.ID = @ProductID);
END;");
        }




        public static void AddWholesalePriceColumnToOI(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
    name: "WholesalePrice",
    table: "OrderItems",
    type: "real",
    nullable: false,
    defaultValueSql: "0");

        }
    }
}
// add new column
// add Customer TVF
