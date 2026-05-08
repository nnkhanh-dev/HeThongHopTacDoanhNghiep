using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopTacDoanhNghiep.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SinhViens_MaSV",
                table: "SinhViens");

            migrationBuilder.DropIndex(
                name: "IX_DoanhNghieps_MaDN",
                table: "DoanhNghieps");

            migrationBuilder.AddColumn<string>(
                name: "TuKhoa",
                table: "BaiViets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LinhVucs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhVucs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nganhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNganh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenNganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenChuyenNganh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nganhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViecLams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YeuCau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UuTien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuyenLoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LuongToiThieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LuongToiDa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuKhoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayHetHan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoaiViecLam = table.Column<int>(type: "int", nullable: false),
                    DoiTuongUngTuyen = table.Column<int>(type: "int", nullable: false),
                    TrinhDo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DoanhNghiepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinhVucId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinhVucId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViecLams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViecLams_DoanhNghieps_DoanhNghiepId",
                        column: x => x.DoanhNghiepId,
                        principalTable: "DoanhNghieps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ViecLams_LinhVucs_LinhVucId",
                        column: x => x.LinhVucId,
                        principalTable: "LinhVucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ViecLams_LinhVucs_LinhVucId1",
                        column: x => x.LinhVucId1,
                        principalTable: "LinhVucs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LinhVucNganhs",
                columns: table => new
                {
                    LinhVucId = table.Column<int>(type: "int", nullable: false),
                    NganhId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinhVucNganhs", x => new { x.LinhVucId, x.NganhId });
                    table.ForeignKey(
                        name: "FK_LinhVucNganhs_LinhVucs_LinhVucId",
                        column: x => x.LinhVucId,
                        principalTable: "LinhVucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LinhVucNganhs_Nganhs_NganhId",
                        column: x => x.NganhId,
                        principalTable: "Nganhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinhVienNganhs",
                columns: table => new
                {
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NganhId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "LichPhongVans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThoiGianKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiaDiem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongUngVien = table.Column<int>(type: "int", nullable: false),
                    ViecLamId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichPhongVans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichPhongVans_ViecLams_ViecLamId",
                        column: x => x.ViecLamId,
                        principalTable: "ViecLams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LuuTrus",
                columns: table => new
                {
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViecLamId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuuTrus", x => new { x.SinhVienId, x.ViecLamId });
                    table.ForeignKey(
                        name: "FK_LuuTrus_SinhViens_SinhVienId",
                        column: x => x.SinhVienId,
                        principalTable: "SinhViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LuuTrus_ViecLams_ViecLamId",
                        column: x => x.ViecLamId,
                        principalTable: "ViecLams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SinhVienViecLams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SinhVienId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ViecLamId = table.Column<int>(type: "int", nullable: true),
                    HoSoUngTuyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    LoaiHoSo = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhVienViecLams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinhVienViecLams_SinhViens_SinhVienId",
                        column: x => x.SinhVienId,
                        principalTable: "SinhViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SinhVienViecLams_ViecLams_ViecLamId",
                        column: x => x.ViecLamId,
                        principalTable: "ViecLams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DangKyPhongVans",
                columns: table => new
                {
                    LichPhongVanId = table.Column<int>(type: "int", nullable: false),
                    SinhVienViecLamId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyPhongVans", x => new { x.LichPhongVanId, x.SinhVienViecLamId });
                    table.ForeignKey(
                        name: "FK_DangKyPhongVans_LichPhongVans_LichPhongVanId",
                        column: x => x.LichPhongVanId,
                        principalTable: "LichPhongVans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DangKyPhongVans_SinhVienViecLams_SinhVienViecLamId",
                        column: x => x.SinhVienViecLamId,
                        principalTable: "SinhVienViecLams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaSV_DeletedAt",
                table: "SinhViens",
                columns: new[] { "MaSV", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DoanhNghieps_MaDN_DeletedAt",
                table: "DoanhNghieps",
                columns: new[] { "MaDN", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyPhongVans_SinhVienViecLamId",
                table: "DangKyPhongVans",
                column: "SinhVienViecLamId");

            migrationBuilder.CreateIndex(
                name: "IX_LichPhongVans_ViecLamId",
                table: "LichPhongVans",
                column: "ViecLamId");

            migrationBuilder.CreateIndex(
                name: "IX_LinhVucNganhs_NganhId",
                table: "LinhVucNganhs",
                column: "NganhId");

            migrationBuilder.CreateIndex(
                name: "IX_LuuTrus_ViecLamId",
                table: "LuuTrus",
                column: "ViecLamId");

            migrationBuilder.CreateIndex(
                name: "IX_Nganhs_MaNganh_DeletedAt",
                table: "Nganhs",
                columns: new[] { "MaNganh", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVienNganhs_NganhId",
                table: "SinhVienNganhs",
                column: "NganhId");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVienViecLams_SinhVienId",
                table: "SinhVienViecLams",
                column: "SinhVienId");

            migrationBuilder.CreateIndex(
                name: "IX_SinhVienViecLams_ViecLamId",
                table: "SinhVienViecLams",
                column: "ViecLamId");

            migrationBuilder.CreateIndex(
                name: "IX_ViecLams_DoanhNghiepId",
                table: "ViecLams",
                column: "DoanhNghiepId");

            migrationBuilder.CreateIndex(
                name: "IX_ViecLams_LinhVucId",
                table: "ViecLams",
                column: "LinhVucId");

            migrationBuilder.CreateIndex(
                name: "IX_ViecLams_LinhVucId1",
                table: "ViecLams",
                column: "LinhVucId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKyPhongVans");

            migrationBuilder.DropTable(
                name: "LinhVucNganhs");

            migrationBuilder.DropTable(
                name: "LuuTrus");

            migrationBuilder.DropTable(
                name: "SinhVienNganhs");

            migrationBuilder.DropTable(
                name: "LichPhongVans");

            migrationBuilder.DropTable(
                name: "SinhVienViecLams");

            migrationBuilder.DropTable(
                name: "Nganhs");

            migrationBuilder.DropTable(
                name: "ViecLams");

            migrationBuilder.DropTable(
                name: "LinhVucs");

            migrationBuilder.DropIndex(
                name: "IX_SinhViens_MaSV_DeletedAt",
                table: "SinhViens");

            migrationBuilder.DropIndex(
                name: "IX_DoanhNghieps_MaDN_DeletedAt",
                table: "DoanhNghieps");

            migrationBuilder.DropColumn(
                name: "TuKhoa",
                table: "BaiViets");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaSV",
                table: "SinhViens",
                column: "MaSV",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoanhNghieps_MaDN",
                table: "DoanhNghieps",
                column: "MaDN",
                unique: true);
        }
    }
}
