using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace training_management_internship.Migrations
{
    /// <inheritdoc />
    public partial class AddLopIdToDangKyKhoaHoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LopId",
                table: "DangKyKhoaHocs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKhoaHocs_LopId",
                table: "DangKyKhoaHocs",
                column: "LopId");

            migrationBuilder.AddForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs",
                column: "LopId",
                principalTable: "Lops",
                principalColumn: "LopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs");

            migrationBuilder.DropIndex(
                name: "IX_DangKyKhoaHocs_LopId",
                table: "DangKyKhoaHocs");

            migrationBuilder.DropColumn(
                name: "LopId",
                table: "DangKyKhoaHocs");
        }
    }
}
