using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableLichSuNhapDuLieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "LichSuNhapDuLieus",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "LichSuNhapDuLieus");
        }
    }
}
