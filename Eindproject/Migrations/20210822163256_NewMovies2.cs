using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class NewMovies2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Lijsts",
                columns: new[] { "LijstId", "BewerktOp", "ToegeVoegdOp", "UserId" },
                values: new object[] { 1, new DateTime(2021, 8, 22, 18, 32, 55, 784, DateTimeKind.Local).AddTicks(3201), new DateTime(2021, 8, 22, 18, 32, 55, 775, DateTimeKind.Local).AddTicks(1696), null });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "StatusWatch" },
                values: new object[] { 1, false });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "StatusWatch" },
                values: new object[] { 2, true });

            migrationBuilder.InsertData(
                table: "SerieOfFilms",
                columns: new[] { "SerieOfFilmInLijstId", "ApiId", "FilmUrl", "LijstId", "OriginalTitle", "Score", "StatusId", "aantalAfleveringen", "aantalGekekenAfleveringen", "tijdPerAflevering" },
                values: new object[] { "1", 200, "/jYtNUfMbU6DBbmd4LUS19u4hF4p.jpg", 1, "Star Trek: The Next Generation Collection", 8.0, 1, 0, 0, new TimeSpan(0, 0, 0, 0, 0) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SerieOfFilms",
                keyColumn: "SerieOfFilmInLijstId",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 1);
        }
    }
}
