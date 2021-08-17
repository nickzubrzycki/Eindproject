using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Data.Migrations
{
    public partial class appUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Achternaam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LijstId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Voornaam",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Lijsts_LijstId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LijstId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Achternaam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LijstId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Voornaam",
                table: "AspNetUsers");
        }
    }
}
