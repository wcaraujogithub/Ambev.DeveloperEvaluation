using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class ajuste_column_SaleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItens_Sales_SaleId",
                table: "SaleItens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleItens",
                table: "SaleItens");

            migrationBuilder.RenameTable(
                name: "SaleItens",
                newName: "SaleItems");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItens_SaleId",
                table: "SaleItems",
                newName: "IX_SaleItems_SaleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "SaleItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleItems_Sales_SaleId",
                table: "SaleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleItems",
                table: "SaleItems");

            migrationBuilder.RenameTable(
                name: "SaleItems",
                newName: "SaleItens");

            migrationBuilder.RenameIndex(
                name: "IX_SaleItems_SaleId",
                table: "SaleItens",
                newName: "IX_SaleItens_SaleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SaleId",
                table: "SaleItens",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleItens",
                table: "SaleItens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleItens_Sales_SaleId",
                table: "SaleItens",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }
    }
}
