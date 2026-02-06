using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableDoanhNghiepSinhVienRemoveStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "SinhViens");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DoanhNghieps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "SinhViens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "DoanhNghieps",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
