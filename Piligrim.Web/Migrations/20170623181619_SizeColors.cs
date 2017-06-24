using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Piligrim.Web.Migrations
{
    public partial class SizeColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Products_ProductId",
                table: "Color");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Color",
                newName: "SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_Color_ProductId",
                table: "Color",
                newName: "IX_Color_SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Color_Size_SizeId",
                table: "Color",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Size_SizeId",
                table: "Color");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Color",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Color_SizeId",
                table: "Color",
                newName: "IX_Color_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Color_Products_ProductId",
                table: "Color",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
