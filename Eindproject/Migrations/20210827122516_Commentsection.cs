using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class Commentsection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment_Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieOrSerie_Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 27, 14, 25, 15, 829, DateTimeKind.Local).AddTicks(3847), new DateTime(2021, 8, 27, 14, 25, 15, 820, DateTimeKind.Local).AddTicks(9344) });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 26, 10, 2, 53, 11, DateTimeKind.Local).AddTicks(2714), new DateTime(2021, 8, 26, 10, 2, 53, 2, DateTimeKind.Local).AddTicks(2967) });
        }
    }
}
