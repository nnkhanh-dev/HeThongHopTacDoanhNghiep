using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddTableLienHe1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ViecLams_LinhVucs_LinhVucId1",
                table: "ViecLams");

            migrationBuilder.DropIndex(
                name: "IX_ViecLams_LinhVucId1",
                table: "ViecLams");

            migrationBuilder.DropColumn(
                name: "LinhVucId1",
                table: "ViecLams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinhVucId1",
                table: "ViecLams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ViecLams_LinhVucId1",
                table: "ViecLams",
                column: "LinhVucId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ViecLams_LinhVucs_LinhVucId1",
                table: "ViecLams",
                column: "LinhVucId1",
                principalTable: "LinhVucs",
                principalColumn: "Id");
        }
    }
}
