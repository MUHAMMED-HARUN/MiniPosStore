using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ChangeRawMaterialOrderItemFloatsToReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop default constraints if any and alter to real
            migrationBuilder.Sql(@"
DECLARE @tbl SYSNAME = N'dbo.RawMaterialOrderItems';

DECLARE @dropConstraint NVARCHAR(MAX) = '';

-- Helper to drop default constraint for a column
DECLARE @col SYSNAME;

-- Quantity
SET @col = N'Quantity';
SELECT @dropConstraint = N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(t.object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name)
FROM sys.default_constraints dc
JOIN sys.columns c ON c.default_object_id = dc.object_id
JOIN sys.tables t ON t.object_id = c.object_id
WHERE t.name = 'RawMaterialOrderItems' AND c.name = @col;
IF (@dropConstraint <> '') EXEC(@dropConstraint);
SET @dropConstraint = '';

-- SellingPrice
SET @col = N'SellingPrice';
SELECT @dropConstraint = N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(t.object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name)
FROM sys.default_constraints dc
JOIN sys.columns c ON c.default_object_id = dc.object_id
JOIN sys.tables t ON t.object_id = c.object_id
WHERE t.name = 'RawMaterialOrderItems' AND c.name = @col;
IF (@dropConstraint <> '') EXEC(@dropConstraint);
SET @dropConstraint = '';

-- WholesalePrice
SET @col = N'WholesalePrice';
SELECT @dropConstraint = N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(t.object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name)
FROM sys.default_constraints dc
JOIN sys.columns c ON c.default_object_id = dc.object_id
JOIN sys.tables t ON t.object_id = c.object_id
WHERE t.name = 'RawMaterialOrderItems' AND c.name = @col;
IF (@dropConstraint <> '') EXEC(@dropConstraint);

ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN Quantity real NOT NULL;
ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN SellingPrice real NOT NULL;
ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN WholesalePrice real NOT NULL;

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert to float(53) (SQL Server float) if needed
            migrationBuilder.Sql(@"
ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN Quantity float NOT NULL;
ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN SellingPrice float NOT NULL;
ALTER TABLE dbo.RawMaterialOrderItems ALTER COLUMN WholesalePrice float NOT NULL;
");
        }
    }
}


