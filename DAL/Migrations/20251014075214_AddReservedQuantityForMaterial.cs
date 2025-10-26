using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedQuantityForMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float?>(
    name: "ReservedQuantity",
    table: "RawMaterials",
    type: "real",
    nullable: true,
    defaultValueSql: "0");
            clsTVFMigration.AddTVFRawMaterialsFilter(migrationBuilder);

            migrationBuilder.Sql(@"CREATE TABLE [dbo].[RawMaterialOrderItems] (
    [ID]             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [OrderID]        INT NOT NULL,
    [RawMaterialID]  INT NOT NULL,
    [Quantity]       FLOAT NOT NULL,
    [SellingPrice]   FLOAT NOT NULL,
    [WholesalePrice] FLOAT NOT NULL,

    CONSTRAINT [FK_RawMaterialOrderItems_Orders]
        FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Orders] ([ID]) ON DELETE CASCADE,

    CONSTRAINT [FK_RawMaterialOrderItems_RawMaterials]
        FOREIGN KEY ([RawMaterialID]) REFERENCES [dbo].[RawMaterials] ([ID]) ON DELETE CASCADE
);
");
            migrationBuilder.Sql(@"CREATE TABLE [dbo].[ImportRawMaterialItems] (
    [ID]             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ImportOrderID]  INT NOT NULL,
    [RawMaterialID]  INT NOT NULL,
    [Quantity]       FLOAT NOT NULL,
    [SellingPrice]   FLOAT NOT NULL,

    CONSTRAINT [FK_ImportRawMaterialItems_ImportOrders]
        FOREIGN KEY ([ImportOrderID]) REFERENCES [dbo].[ImportOrders] ([ID]) ON DELETE CASCADE,

    CONSTRAINT [FK_ImportRawMaterialItems_RawMaterials]
        FOREIGN KEY ([RawMaterialID]) REFERENCES [dbo].[RawMaterials] ([ID]) ON DELETE CASCADE
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
