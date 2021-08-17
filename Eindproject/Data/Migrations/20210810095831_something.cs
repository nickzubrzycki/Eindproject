using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Data.Migrations
{
    public partial class something : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lijsts",
                columns: table => new
                {
                    LijstId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerId = table.Column<int>(type: "int", nullable: false),
                    ToegeVoegdOp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BewerktOp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lijsts", x => x.LijstId);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "SerieOfFilms",
                columns: table => new
                {
                    SerieOfFilmId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LijstId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    aantalAfleveringen = table.Column<int>(type: "int", nullable: false),
                    tijdPerAflevering = table.Column<TimeSpan>(type: "time", nullable: false),
                    aantalGekekenAfleveringen = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerieOfFilms", x => x.SerieOfFilmId);
                    table.ForeignKey(
                        name: "FK_SerieOfFilms_Lijsts_LijstId",
                        column: x => x.LijstId,
                        principalTable: "Lijsts",
                        principalColumn: "LijstId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SerieOfFilms_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SerieOfFilms_LijstId",
                table: "SerieOfFilms",
                column: "LijstId");

            migrationBuilder.CreateIndex(
                name: "IX_SerieOfFilms_StatusId",
                table: "SerieOfFilms",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
