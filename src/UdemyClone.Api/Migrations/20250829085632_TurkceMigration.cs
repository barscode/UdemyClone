using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdemyClone.Api.Migrations
{
    /// <inheritdoc />
    public partial class TurkceMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dersler_Kurslar_CourseId",
                table: "Dersler");

            migrationBuilder.DropForeignKey(
                name: "FK_Kurslar_Categories_CategoryId",
                table: "Kurslar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kurslar_Kullanicilar_InstructorId",
                table: "Kurslar");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Kurslar_InstructorId",
                table: "Kurslar");

            migrationBuilder.DropIndex(
                name: "IX_Dersler_CourseId",
                table: "Dersler");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Kurslar",
                newName: "Seviye");

            migrationBuilder.RenameColumn(
                name: "Published",
                table: "Kurslar",
                newName: "YayindaMi");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Kurslar",
                newName: "Fiyat");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "Kurslar",
                newName: "TahminiSure");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Kurslar",
                newName: "KapakResmi");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Kurslar",
                newName: "KategoriId");

            migrationBuilder.RenameIndex(
                name: "IX_Kurslar_CategoryId",
                table: "Kurslar",
                newName: "IX_Kurslar_KategoriId");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Kullanicilar",
                newName: "Soyad");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Kullanicilar",
                newName: "SifreHash");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Dersler",
                newName: "Baslik");

            migrationBuilder.RenameColumn(
                name: "DurationSeconds",
                table: "Dersler",
                newName: "SureSaniye");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Dersler",
                newName: "Sira");

            migrationBuilder.RenameColumn(
                name: "ContentUrl",
                table: "Dersler",
                newName: "IcerikUrl");

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Kurslar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Baslik",
                table: "Kurslar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EgitmenId",
                table: "Kurslar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellenmeTarihi",
                table: "Kurslar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KategoriId1",
                table: "Kurslar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturulmaTarihi",
                table: "Kurslar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TanitimVideosu",
                table: "Kurslar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "Kuponlar",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Kuponlar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "KursId",
                table: "Kuponlar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ad",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Fotograf",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hakkinda",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SonGirisTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Dersler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KursId",
                table: "Dersler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UcretsizMi",
                table: "Dersler",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Kategoriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ikon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UstKategoriId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kategoriler_Kategoriler_UstKategoriId",
                        column: x => x.UstKategoriId,
                        principalTable: "Kategoriler",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kurslar_EgitmenId",
                table: "Kurslar",
                column: "EgitmenId");

            migrationBuilder.CreateIndex(
                name: "IX_Kurslar_KategoriId1",
                table: "Kurslar",
                column: "KategoriId1");

            migrationBuilder.CreateIndex(
                name: "IX_Kuponlar_KursId",
                table: "Kuponlar",
                column: "KursId");

            migrationBuilder.CreateIndex(
                name: "IX_Dersler_KursId",
                table: "Dersler",
                column: "KursId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoriler_UstKategoriId",
                table: "Kategoriler",
                column: "UstKategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dersler_Kurslar_KursId",
                table: "Dersler",
                column: "KursId",
                principalTable: "Kurslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kuponlar_Kurslar_KursId",
                table: "Kuponlar",
                column: "KursId",
                principalTable: "Kurslar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kurslar_Kategoriler_KategoriId",
                table: "Kurslar",
                column: "KategoriId",
                principalTable: "Kategoriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kurslar_Kategoriler_KategoriId1",
                table: "Kurslar",
                column: "KategoriId1",
                principalTable: "Kategoriler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kurslar_Kullanicilar_EgitmenId",
                table: "Kurslar",
                column: "EgitmenId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dersler_Kurslar_KursId",
                table: "Dersler");

            migrationBuilder.DropForeignKey(
                name: "FK_Kuponlar_Kurslar_KursId",
                table: "Kuponlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kurslar_Kategoriler_KategoriId",
                table: "Kurslar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kurslar_Kategoriler_KategoriId1",
                table: "Kurslar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kurslar_Kullanicilar_EgitmenId",
                table: "Kurslar");

            migrationBuilder.DropTable(
                name: "Kategoriler");

            migrationBuilder.DropIndex(
                name: "IX_Kurslar_EgitmenId",
                table: "Kurslar");

            migrationBuilder.DropIndex(
                name: "IX_Kurslar_KategoriId1",
                table: "Kurslar");

            migrationBuilder.DropIndex(
                name: "IX_Kuponlar_KursId",
                table: "Kuponlar");

            migrationBuilder.DropIndex(
                name: "IX_Dersler_KursId",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "Baslik",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "EgitmenId",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "GuncellenmeTarihi",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "KategoriId1",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "OlusturulmaTarihi",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "TanitimVideosu",
                table: "Kurslar");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Kuponlar");

            migrationBuilder.DropColumn(
                name: "KursId",
                table: "Kuponlar");

            migrationBuilder.DropColumn(
                name: "Ad",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Fotograf",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Hakkinda",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SonGirisTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "KursId",
                table: "Dersler");

            migrationBuilder.DropColumn(
                name: "UcretsizMi",
                table: "Dersler");

            migrationBuilder.RenameColumn(
                name: "YayindaMi",
                table: "Kurslar",
                newName: "Published");

            migrationBuilder.RenameColumn(
                name: "TahminiSure",
                table: "Kurslar",
                newName: "InstructorId");

            migrationBuilder.RenameColumn(
                name: "Seviye",
                table: "Kurslar",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "KategoriId",
                table: "Kurslar",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "KapakResmi",
                table: "Kurslar",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Fiyat",
                table: "Kurslar",
                newName: "Price");

            migrationBuilder.RenameIndex(
                name: "IX_Kurslar_KategoriId",
                table: "Kurslar",
                newName: "IX_Kurslar_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Soyad",
                table: "Kullanicilar",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "SifreHash",
                table: "Kullanicilar",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "SureSaniye",
                table: "Dersler",
                newName: "DurationSeconds");

            migrationBuilder.RenameColumn(
                name: "Sira",
                table: "Dersler",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "IcerikUrl",
                table: "Dersler",
                newName: "ContentUrl");

            migrationBuilder.RenameColumn(
                name: "Baslik",
                table: "Dersler",
                newName: "Title");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "Kuponlar",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kurslar_InstructorId",
                table: "Kurslar",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dersler_CourseId",
                table: "Dersler",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dersler_Kurslar_CourseId",
                table: "Dersler",
                column: "CourseId",
                principalTable: "Kurslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kurslar_Categories_CategoryId",
                table: "Kurslar",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kurslar_Kullanicilar_InstructorId",
                table: "Kurslar",
                column: "InstructorId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
