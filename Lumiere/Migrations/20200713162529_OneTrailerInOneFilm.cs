using Microsoft.EntityFrameworkCore.Migrations;

namespace Lumiere.Migrations
{
    public partial class OneTrailerInOneFilm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trailers_FilmId",
                table: "Trailers");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_FilmId",
                table: "Trailers",
                column: "FilmId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trailers_FilmId",
                table: "Trailers");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_FilmId",
                table: "Trailers",
                column: "FilmId");
        }
    }
}
