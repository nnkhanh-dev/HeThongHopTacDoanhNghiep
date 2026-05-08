using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteTimeStamptForSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SinhViens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DoanhNghieps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DoanhNghieps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DanhMucBaiViets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "DanhMucBaiViets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "BaiViets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BaiViets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DanhMucBaiViets");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DanhMucBaiViets");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "BaiViets");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BaiViets");
        }
    }
}
