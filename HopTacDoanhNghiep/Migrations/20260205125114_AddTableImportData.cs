using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddTableImportData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SinhViens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SinhViens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DoanhNghieps",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DoanhNghieps",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DoanhNghieps",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DoanhNghieps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DoanhNghieps");
        }
    }
}
