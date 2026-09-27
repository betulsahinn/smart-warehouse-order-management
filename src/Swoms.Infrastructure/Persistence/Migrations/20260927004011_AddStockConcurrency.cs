using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swoms.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStockConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                table: "StockItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddCheckConstraint(
                name: "CK_StockItems_ValidQuantities",
                table: "StockItems",
                sql: "\"QuantityOnHand\" >= 0 AND \"ReservedQuantity\" >= 0 AND \"ReservedQuantity\" <= \"QuantityOnHand\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_StockItems_ValidQuantities",
                table: "StockItems");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "StockItems");
        }
    }
}
