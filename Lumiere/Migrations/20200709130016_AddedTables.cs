using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lumiere.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeat_AspNetUsers_UserId",
                table: "ReservedSeat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedSeat",
                table: "ReservedSeat");

            migrationBuilder.RenameTable(
                name: "ReservedSeat",
                newName: "ReservedSeats");

            migrationBuilder.RenameIndex(
                name: "IX_ReservedSeat_UserId",
                table: "ReservedSeats",
                newName: "IX_ReservedSeats_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedSeats",
                table: "ReservedSeats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FilmFeedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Rating = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmFeedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmFeedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmFeedbacks_UserId",
                table: "FilmFeedbacks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeats_AspNetUsers_UserId",
                table: "ReservedSeats",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservedSeats_AspNetUsers_UserId",
                table: "ReservedSeats");

            migrationBuilder.DropTable(
                name: "FilmFeedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservedSeats",
                table: "ReservedSeats");

            migrationBuilder.RenameTable(
                name: "ReservedSeats",
                newName: "ReservedSeat");

            migrationBuilder.RenameIndex(
                name: "IX_ReservedSeats_UserId",
                table: "ReservedSeat",
                newName: "IX_ReservedSeat_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservedSeat",
                table: "ReservedSeat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservedSeat_AspNetUsers_UserId",
                table: "ReservedSeat",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
