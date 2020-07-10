using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lumiere.Migrations
{
    public partial class AddedFilmsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeanceId",
                table: "ReservedSeats",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId",
                table: "FilmFeedbacks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AgeLimit = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    FilmId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posters_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    FilmId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seances_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    FilmId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trailers_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSeats_SeanceId",
                table: "ReservedSeats",
                column: "SeanceId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmFeedbacks_FilmId",
                table: "FilmFeedbacks",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Posters_FilmId",
                table: "Posters",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Seances_FilmId",
                table: "Seances",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_FilmId",
                table: "Trailers",
                column: "FilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmFeedbacks_Films_FilmId",
                table: "FilmFeedbacks",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_Seances_SeanceId",
                table: "ReservedSeats",
                column: "SeanceId",
                principalTable: "Seances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmFeedbacks_Films_FilmId",
                table: "FilmFeedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_Seances_SeanceId",
                table: "ReservedSeats");

            migrationBuilder.DropTable(
                name: "Posters");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropIndex(
                name: "IX_ReservedSeats_SeanceId",
                table: "ReservedSeats");

            migrationBuilder.DropIndex(
                name: "IX_FilmFeedbacks_FilmId",
                table: "FilmFeedbacks");

            migrationBuilder.DropColumn(
                name: "SeanceId",
                table: "ReservedSeats");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "FilmFeedbacks");
        }
    }
}
