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
                migrationBuilder.Sql(@"CREATE FUNCTION dbo.GetProductsFiltered
                    (
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
                            u.Name AS UnitOfMeasureName
                        FROM dbo.Products p
                        INNER JOIN dbo.UnitOfMeasures u ON p.UOMID = u.ID
                        WHERE
                            (p.ActionType !=3) and
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

        public static void AlterTVFProductFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER  FUNCTION dbo.GetProductsFiltered
                    (
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
                            u.Name AS UnitOfMeasureName
                        FROM dbo.Products p
                        INNER JOIN dbo.UnitOfMeasures u ON p.UOMID = u.ID
                        WHERE
                            (p.ActionType !=3) and
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
            migrationBuilder.Sql(@"CREATE FUNCTION dbo.GetOrdersFiltred
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
GO


");
        }

        public static void AddTVFImportOrderFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION dbo.GetImportOrdersFiltered
(
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
    WHERE (@FirstName IS NULL OR p.FirstName LIKE '%' + @FirstName + '%')
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
GO


");
        }
    }
}
