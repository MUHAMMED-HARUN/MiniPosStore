using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ChangeImportRawMaterialItemsFloatsToReal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DECLARE @tbl SYSNAME = N'dbo.ImportRawMaterialItems';

DECLARE @dropConstraint NVARCHAR(MAX) = '';

-- Quantity
SELECT @dropConstraint = N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(t.object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name)
FROM sys.default_constraints dc
JOIN sys.columns c ON c.default_object_id = dc.object_id
JOIN sys.tables t ON t.object_id = c.object_id
WHERE t.name = 'ImportRawMaterialItems' AND c.name = 'Quantity';
IF (@dropConstraint <> '') EXEC(@dropConstraint);
SET @dropConstraint = '';

-- SellingPrice
SELECT @dropConstraint = N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(t.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(t.object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(dc.name)
FROM sys.default_constraints dc
JOIN sys.columns c ON c.default_object_id = dc.object_id
JOIN sys.tables t ON t.object_id = c.object_id
WHERE t.name = 'ImportRawMaterialItems' AND c.name = 'SellingPrice';
IF (@dropConstraint <> '') EXEC(@dropConstraint);

ALTER TABLE dbo.ImportRawMaterialItems ALTER COLUMN Quantity real NOT NULL;
ALTER TABLE dbo.ImportRawMaterialItems ALTER COLUMN SellingPrice real NOT NULL;

");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE dbo.ImportRawMaterialItems ALTER COLUMN Quantity float NOT NULL;
ALTER TABLE dbo.ImportRawMaterialItems ALTER COLUMN SellingPrice float NOT NULL;
");
        }
    }
}


