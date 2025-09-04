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
    }
}
