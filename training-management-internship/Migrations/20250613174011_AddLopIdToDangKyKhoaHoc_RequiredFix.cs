using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace training_management_internship.Migrations
{
    /// <inheritdoc />
    public partial class AddLopIdToDangKyKhoaHoc_RequiredFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs");

            migrationBuilder.AlterColumn<int>(
                name: "LopId",
                table: "DangKyKhoaHocs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs",
                column: "LopId",
                principalTable: "Lops",
                principalColumn: "LopId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs");

            migrationBuilder.AlterColumn<int>(
                name: "LopId",
                table: "DangKyKhoaHocs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DangKyKhoaHocs_Lops_LopId",
                table: "DangKyKhoaHocs",
                column: "LopId",
                principalTable: "Lops",
                principalColumn: "LopId");
        }
    }
}
