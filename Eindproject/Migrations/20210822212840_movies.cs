using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class movies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Lijsts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 22, 23, 28, 39, 946, DateTimeKind.Local).AddTicks(3431), new DateTime(2021, 8, 22, 23, 28, 39, 938, DateTimeKind.Local).AddTicks(7863) });

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Lijsts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 22, 18, 32, 55, 784, DateTimeKind.Local).AddTicks(3201), new DateTime(2021, 8, 22, 18, 32, 55, 775, DateTimeKind.Local).AddTicks(1696) });
        }
    }
}
