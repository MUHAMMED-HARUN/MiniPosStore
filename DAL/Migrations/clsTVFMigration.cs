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
            migrationBuilder.Sql(@"
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
        public static void AddTVFGetUnionImportOrderItemsFiltered(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION [dbo].[ufn_GetUnionImportOrderItemsFiltered]
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
    @AvailableQuantityFrom FLOAT = NULL,
    @AvailableQuantityTo FLOAT = NULL,
    @ReservedQuantityFrom FLOAT = NULL,
    @ReservedQuantityTo FLOAT = NULL,
    @CurrencyTypeID INT = NULL,
    @CurrencyType NVARCHAR(50) = NULL
)
RETURNS TABLE
AS
RETURN
(
    WITH UnionImportOrderItems AS
    (
        -- المنتجات المستوردة
        SELECT 
            ioi.ID AS ImportOrderItemID,
            ioi.ImportOrderID,
            p.ID AS ItemID,
            p.Name AS ItemName,
            p.Description,
            ioi.Quantity,
            ioi.SellingPrice,
            p.AvailableQuantity,
            p.ReservedQuantity,
            p.CurrencyType,
            NULL AS CurrencyTypeID,
            u.Name AS UOMName,
            u.Seymbol AS UOMSymbol,
            1 AS ItemType
        FROM ImportOrderItems ioi
        INNER JOIN Products p ON ioi.ProductID = p.ID
        INNER JOIN UnitOfMeasures u ON p.UOMID = u.ID

        UNION ALL

        -- المواد الخام المستوردة
        SELECT 
            irmi.ID AS ImportOrderItemID,
            irmi.ImportOrderID,
            rm.ID AS ItemID,
            rm.Name AS ItemName,
            rm.Description,
            irmi.Quantity,
            irmi.SellingPrice,
            rm.AvailableQuantity,
            rm.ReservedQuantity,
            NULL AS CurrencyType,
            rm.CurrencyTypeID,
            u.Name AS UOMName,
            u.Seymbol AS UOMSymbol,
            2 AS ItemType
        FROM ImportRawMaterialItems irmi
        INNER JOIN RawMaterials rm ON irmi.RawMaterialID = rm.ID
        INNER JOIN UnitOfMeasures u ON rm.UOMID = u.ID
    )
    SELECT *
    FROM UnionImportOrderItems
    WHERE (@ImportOrderItemID IS NULL OR ImportOrderItemID = @ImportOrderItemID)
      AND (@ImportOrderID IS NULL OR ImportOrderID = @ImportOrderID)
      AND (@ItemID IS NULL OR ItemID = @ItemID)
      AND (@ItemType IS NULL OR ItemType = @ItemType)
      AND (@ItemName IS NULL OR ItemName LIKE '%' + @ItemName + '%')
      AND (@Description IS NULL OR Description LIKE '%' + @Description + '%')
      AND (@QuantityFrom IS NULL OR Quantity >= @QuantityFrom)
      AND (@QuantityTo IS NULL OR Quantity <= @QuantityTo)
      AND (@SellingPriceFrom IS NULL OR SellingPrice >= @SellingPriceFrom)
      AND (@SellingPriceTo IS NULL OR SellingPrice <= @SellingPriceTo)
      AND (@AvailableQuantityFrom IS NULL OR AvailableQuantity >= @AvailableQuantityFrom)
      AND (@AvailableQuantityTo IS NULL OR AvailableQuantity <= @AvailableQuantityTo)
      AND (@ReservedQuantityFrom IS NULL OR ReservedQuantity >= @ReservedQuantityFrom)
      AND (@ReservedQuantityTo IS NULL OR ReservedQuantity <= @ReservedQuantityTo)
      AND (@CurrencyTypeID IS NULL OR CurrencyTypeID = @CurrencyTypeID)
      AND (@CurrencyType IS NULL OR CurrencyType = @CurrencyType)
);
");
        }
        public static void AddTVFOrderFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
        public static void Addufn_GetUnionOrderItemsFiltered(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"


CREATE FUNCTION dbo.ufn_GetUnionOrderItemsFiltered
(
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
);
GO
");
        }
        public static void AddTVFImportOrderFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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
            migrationBuilder.Sql(@"
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
            migrationBuilder.Sql(@"CREATE OR ALTER FUNCTION dbo.GetTopProductsID
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
            migrationBuilder.Sql(@"

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
        public static void AddReservedQuantityForProduct(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float?>(
                name: "ReservedQuantity",
                table: "Products",
                type: "real",
                nullable: true,
                defaultValueSql: "0");
        }

        // ========== Create Raw Materials Table ==========
        public static void CreateRawMaterialsTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RawMaterials",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false),
                    AvailableQuantity = table.Column<float>(type: "real", nullable: false),
                    UOMID = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeID = table.Column<int>(type: "int", nullable: false),
                    MaterialSupplier = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RawMaterials_UnitOfMeasures_UOMID",
                        column: x => x.UOMID,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RawMaterials_Suppliers_MaterialSupplier",
                        column: x => x.MaterialSupplier,
                        principalTable: "Suppliers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RawMaterials_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_UOMID",
                table: "RawMaterials",
                column: "UOMID");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_MaterialSupplier",
                table: "RawMaterials",
                column: "MaterialSupplier");

            migrationBuilder.CreateIndex(
                name: "IX_RawMaterials_UserID",
                table: "RawMaterials",
                column: "UserID");
        }

        // ========== Create Recipes Table ==========
        public static void CreateRecipesTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    YieldQuantity = table.Column<float>(type: "real", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Recipes_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Recipes_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductID",
                table: "Recipes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserID",
                table: "Recipes",
                column: "UserID");
        }

        // ========== Create Recipe Info Table ==========
        public static void CreateRecipeInfoTable(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipeInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeID = table.Column<int>(type: "int", nullable: false),
                    RawMaterialID = table.Column<int>(type: "int", nullable: false),
                    RequiredMaterialQuantity = table.Column<float>(type: "real", nullable: false),
                    ProductionLossQuantity = table.Column<float>(type: "real", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionType = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeInfos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RecipeInfos_Recipes_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RecipeInfos_RawMaterials_RawMaterialID",
                        column: x => x.RawMaterialID,
                        principalTable: "RawMaterials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RecipeInfos_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInfos_RecipeID",
                table: "RecipeInfos",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInfos_RawMaterialID",
                table: "RecipeInfos",
                column: "RawMaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeInfos_UserID",
                table: "RecipeInfos",
                column: "UserID");
        }

        // ========== TVF for Raw Materials Filter ==========
        public static void AddTVFRawMaterialsFilter(MigrationBuilder migrationBuilder)
        {
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
    @ActionDateTo DATETIME = NULL,
    @ReservedQuantityFrom DECIMAL(18,2) = NULL,
    @ReservedQuantityTo DECIMAL(18,2) = NULL
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
        rm.ReservedQuantity,
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
        (@ActionDateTo IS NULL OR rm.ActionDate <= @ActionDateTo) AND
        (@ReservedQuantityFrom IS NULL OR rm.ReservedQuantity >= @ReservedQuantityFrom) AND
        (@ReservedQuantityTo IS NULL OR rm.ReservedQuantity <= @ReservedQuantityTo)
);
");
        }


        // ========== TVF for Recipes Filter ==========
        public static void AddTVFRecipesFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE FUNCTION [dbo].[GetRecipesFiltered]
(
    @id int = null,
    @Name NVARCHAR(200) = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @ProductName NVARCHAR(200) = NULL,
    @YieldQuantityFrom DECIMAL(18,2) = NULL,
    @YieldQuantityTo DECIMAL(18,2) = NULL,
    @ActionDateFrom DATETIME = NULL,
    @ActionDateTo DATETIME = NULL
)
RETURNS TABLE
AS
RETURN
(
    SELECT DISTINCT
        r.ID,
        r.Name, 
        r.Description, 
        r.ProductID,
        p.Name AS ProductName,
        r.YieldQuantity,
        r.ActionDate
    FROM dbo.Recipes r
    INNER JOIN dbo.Products p ON r.ProductID = p.ID
    WHERE
        (r.ActionType != 3) and
        (@id IS NULL OR r.id = @id) AND
        (@Name IS NULL OR r.Name LIKE '%' + @Name + '%') AND
        (@Description IS NULL OR r.Description LIKE '%' + @Description + '%') AND
        (@ProductName IS NULL OR p.Name LIKE '%' + @ProductName + '%') AND
        (@YieldQuantityFrom IS NULL OR r.YieldQuantity >= @YieldQuantityFrom) AND
        (@YieldQuantityTo IS NULL OR r.YieldQuantity <= @YieldQuantityTo) AND
        (@ActionDateFrom IS NULL OR r.ActionDate >= @ActionDateFrom) AND
        (@ActionDateTo IS NULL OR r.ActionDate <= @ActionDateTo)
);
");
        }

        // ========== TVF for Recipe Info Filter ==========
        public static void AddTVFRecipeInfoFilter(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create or ALTER FUNCTION [dbo].[GetRecipeInfoFiltered]
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
//        public static void AddTVFRecipeInfoFilter(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.Sql(@"USE [MiniPosStore]
//GO
///****** Object:  UserDefinedFunction [dbo].[GetRecipeInfoFiltered]    Script Date: 01/04/47 02:02:32 م ******/
//SET ANSI_NULLS ON
//GO
//SET QUOTED_IDENTIFIER ON
//GO
//CREATE FUNCTION [dbo].[GetRecipeInfoFiltered]
//(
//    @id int = null,
//    @RecipeName NVARCHAR(200) = NULL,
//    @MaterialName NVARCHAR(200) = NULL,
//    @RequiredMaterialQuantityFrom DECIMAL(18,2) = NULL,
//    @RequiredMaterialQuantityTo DECIMAL(18,2) = NULL,
//    @ActionDateFrom DATETIME = NULL,
//    @ActionDateTo DATETIME = NULL
//)
//RETURNS TABLE
//AS
//RETURN
//(
//    SELECT DISTINCT
//        ri.id,
//        ri.RecipeID,
//        r.ItemName AS RecipeName,
//        ri.RawMaterialID AS MaterialID,
//        rm.ItemName AS MaterialName,
//        ri.RequiredMaterialQuantity,
//        ri.ActionDate
//    FROM dbo.RecipeInfos ri
//    INNER JOIN dbo.Recipes r ON ri.RecipeID = r.id
//    INNER JOIN dbo.RawMaterials rm ON ri.RawMaterialID = rm.id
//    WHERE
//        (ri.ActionType != 3) and
//        (@id IS NULL OR ri.id = @id) AND
//        (@RecipeName IS NULL OR r.ItemName LIKE '%' + @RecipeName + '%') AND
//        (@MaterialName IS NULL OR rm.ItemName LIKE '%' + @MaterialName + '%') AND
//        (@RequiredMaterialQuantityFrom IS NULL OR ri.RequiredMaterialQuantity >= @RequiredMaterialQuantityFrom) AND
//        (@RequiredMaterialQuantityTo IS NULL OR ri.RequiredMaterialQuantity <= @RequiredMaterialQuantityTo) AND
//        (@ActionDateFrom IS NULL OR ri.ActionDate >= @ActionDateFrom) AND
//        (@ActionDateTo IS NULL OR ri.ActionDate <= @ActionDateTo)
//);
//");
//        }

    }
}
// add new column
// add Customer TVF
