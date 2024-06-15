using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace htmx_prototype.Migrations
{
    public partial class UpdateProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PreviewImage",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PreviewImage",
                table: "Products",
                type: "BLOB",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
