using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class geupdatedComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId1",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 27, 19, 51, 54, 859, DateTimeKind.Local).AddTicks(6256), new DateTime(2021, 8, 27, 19, 51, 54, 842, DateTimeKind.Local).AddTicks(9448) });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentId1",
                table: "Comments",
                column: "CommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_CommentId1",
                table: "Comments",
                column: "CommentId1",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_CommentId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentId1",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 27, 14, 25, 15, 829, DateTimeKind.Local).AddTicks(3847), new DateTime(2021, 8, 27, 14, 25, 15, 820, DateTimeKind.Local).AddTicks(9344) });
        }
    }
}
