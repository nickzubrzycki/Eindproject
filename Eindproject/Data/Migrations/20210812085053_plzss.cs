using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Data.Migrations
{
    public partial class plzss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lijsts_LijstId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LijstId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GebruikerId",
                table: "Lijsts");

            migrationBuilder.DropColumn(
                name: "LijstId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Lijsts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lijsts_UserId",
                table: "Lijsts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lijsts_AspNetUsers_UserId",
                table: "Lijsts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lijsts_AspNetUsers_UserId",
                table: "Lijsts");

            migrationBuilder.DropIndex(
                name: "IX_Lijsts_UserId",
                table: "Lijsts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lijsts");

            migrationBuilder.AddColumn<int>(
                name: "GebruikerId",
                table: "Lijsts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LijstId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LijstId",
                table: "AspNetUsers",
                column: "LijstId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Lijsts_LijstId",
                table: "AspNetUsers",
                column: "LijstId",
                principalTable: "Lijsts",
                principalColumn: "LijstId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
