using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddTableImportData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LichSuNhapDuLieus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongDuLieu = table.Column<int>(type: "int", nullable: true),
                    ThanhCong = table.Column<int>(type: "int", nullable: true),
                    ThatBai = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    PhanLoai = table.Column<int>(type: "int", nullable: false),
                    DuongDanFileGoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDanFileLoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuNhapDuLieus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LichSuNhapDuLieus");
        }
    }
}
