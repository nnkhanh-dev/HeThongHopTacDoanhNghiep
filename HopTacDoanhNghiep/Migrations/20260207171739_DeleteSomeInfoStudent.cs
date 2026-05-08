using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSomeInfoStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SinhVienNganhs");

            migrationBuilder.DropIndex(
                name: "IX_SinhViens_MaSV_DeletedAt",
                table: "SinhViens");

            migrationBuilder.DropIndex(
                name: "IX_Nganhs_MaNganh_DeletedAt",
                table: "Nganhs");

            migrationBuilder.DropIndex(
                name: "IX_DoanhNghieps_MaDN_DeletedAt",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "ChuyenNganh",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "Khoa",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "Lop",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Nganhs");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaSV",
                table: "SinhViens",
                column: "MaSV",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nganhs_MaNganh",
                table: "Nganhs",
                column: "MaNganh",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoanhNghieps_MaDN",
                table: "DoanhNghieps",
                column: "MaDN",
                unique: true,
                filter: "[DeletedAt] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SinhViens_MaSV",
                table: "SinhViens");

            migrationBuilder.DropIndex(
                name: "IX_Nganhs_MaNganh",
                table: "Nganhs");

            migrationBuilder.DropIndex(
                name: "IX_DoanhNghieps_MaDN",
                table: "DoanhNghieps");

            migrationBuilder.AddColumn<string>(
                name: "ChuyenNganh",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Khoa",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lop",
                table: "SinhViens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Nganhs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SinhVienNganhs",
                columns: table => new
                {
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NganhId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVienNganhs", x => new { x.SinhVienId, x.NganhId });
                    table.ForeignKey(
                        name: "FK_SinhVienNganhs_Nganhs_NganhId",
                        column: x => x.NganhId,
                        principalTable: "Nganhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SinhVienNganhs_SinhViens_SinhVienId",
                        column: x => x.SinhVienId,
                        principalTable: "SinhViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaSV_DeletedAt",
                table: "SinhViens",
                columns: new[] { "MaSV", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Nganhs_MaNganh_DeletedAt",
                table: "Nganhs",
                columns: new[] { "MaNganh", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoanhNghieps_MaDN_DeletedAt",
                table: "DoanhNghieps",
                columns: new[] { "MaDN", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVienNganhs_NganhId",
                table: "SinhVienNganhs",
                column: "NganhId");
        }
    }
}
