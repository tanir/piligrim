using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Piligrim.Web.Migrations
{
    public partial class ColorSizes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Size_SizeId",
                table: "Color");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Size",
                newName: "ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_Size_ProductId",
                table: "Size",
                newName: "IX_Size_ColorId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Color_ColorId",
                table: "Size",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Color_Products_ProductId",
                table: "Color");

            migrationBuilder.DropForeignKey(
                name: "FK_Size_Color_ColorId",
                table: "Size");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "Size",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Size_ColorId",
                table: "Size",
                newName: "IX_Size_ProductId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Size_Products_ProductId",
                table: "Size",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
