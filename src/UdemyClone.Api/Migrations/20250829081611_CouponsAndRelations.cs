using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdemyClone.Api.Migrations
{
    /// <inheritdoc />
    public partial class CouponsAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "Kuponlar",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KuponKurs",
                columns: table => new
                {
                    KuponId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KuponKurs", x => new { x.KuponId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_KuponKurs_Kuponlar_KuponId",
                        column: x => x.KuponId,
                        principalTable: "Kuponlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KuponKurs_Kurslar_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Kurslar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kuponlar_InstructorId",
                table: "Kuponlar",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_KuponKurs_CourseId",
                table: "KuponKurs",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kuponlar_Kullanicilar_InstructorId",
                table: "Kuponlar",
                column: "InstructorId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kuponlar_Kullanicilar_InstructorId",
                table: "Kuponlar");

            migrationBuilder.DropTable(
                name: "KuponKurs");

            migrationBuilder.DropIndex(
                name: "IX_Kuponlar_InstructorId",
                table: "Kuponlar");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Kuponlar");
        }
    }
}
