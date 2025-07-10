using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace training_management_internship.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_DangKyKhoaHocs_DangKyKhoaHocId",
                table: "DanhGias");

            migrationBuilder.AlterColumn<int>(
                name: "DangKyKhoaHocId",
                table: "DanhGias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HocVienId",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LopId",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NguoiDanhGiaId",
                table: "DanhGias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DanhGiaTheoNams",
                columns: table => new
                {
                    DanhGiaTheoNamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HocVienId = table.Column<int>(type: "int", nullable: false),
                    Nam = table.Column<int>(type: "int", nullable: false),
                    LoaiDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NguoiDanhGiaId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGiaTheoNams", x => x.DanhGiaTheoNamId);
                    table.ForeignKey(
                        name: "FK_DanhGiaTheoNams_AspNetUsers_NguoiDanhGiaId",
                        column: x => x.NguoiDanhGiaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGiaTheoNams_HocViens_HocVienId",
                        column: x => x.HocVienId,
                        principalTable: "HocViens",
                        principalColumn: "HocVienId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_HocVienId",
                table: "DanhGias",
                column: "HocVienId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_LopId",
                table: "DanhGias",
                column: "LopId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_NguoiDanhGiaId",
                table: "DanhGias",
                column: "NguoiDanhGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaTheoNams_HocVienId",
                table: "DanhGiaTheoNams",
                column: "HocVienId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGiaTheoNams_NguoiDanhGiaId",
                table: "DanhGiaTheoNams",
                column: "NguoiDanhGiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_AspNetUsers_NguoiDanhGiaId",
                table: "DanhGias",
                column: "NguoiDanhGiaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_DangKyKhoaHocs_DangKyKhoaHocId",
                table: "DanhGias",
                column: "DangKyKhoaHocId",
                principalTable: "DangKyKhoaHocs",
                principalColumn: "DangKyKhoaHocId");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_HocViens_HocVienId",
                table: "DanhGias",
                column: "HocVienId",
                principalTable: "HocViens",
                principalColumn: "HocVienId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_Lops_LopId",
                table: "DanhGias",
                column: "LopId",
                principalTable: "Lops",
                principalColumn: "LopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_AspNetUsers_NguoiDanhGiaId",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_DangKyKhoaHocs_DangKyKhoaHocId",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_HocViens_HocVienId",
                table: "DanhGias");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGias_Lops_LopId",
                table: "DanhGias");

            migrationBuilder.DropTable(
                name: "DanhGiaTheoNams");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_HocVienId",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_LopId",
                table: "DanhGias");

            migrationBuilder.DropIndex(
                name: "IX_DanhGias_NguoiDanhGiaId",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "HocVienId",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "LopId",
                table: "DanhGias");

            migrationBuilder.DropColumn(
                name: "NguoiDanhGiaId",
                table: "DanhGias");

            migrationBuilder.AlterColumn<int>(
                name: "DangKyKhoaHocId",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGias_DangKyKhoaHocs_DangKyKhoaHocId",
                table: "DanhGias",
                column: "DangKyKhoaHocId",
                principalTable: "DangKyKhoaHocs",
                principalColumn: "DangKyKhoaHocId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
