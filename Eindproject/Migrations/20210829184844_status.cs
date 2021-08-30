using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusWatch",
                table: "Statuses");

            migrationBuilder.AddColumn<string>(
                name: "StatusDescription",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 29, 20, 48, 44, 480, DateTimeKind.Local).AddTicks(2936), new DateTime(2021, 8, 29, 20, 48, 44, 477, DateTimeKind.Local).AddTicks(4499) });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 1,
                column: "StatusDescription",
                value: "Done");

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "StatusDescription",
                value: "Watching");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusDescription",
                table: "Statuses");

            migrationBuilder.AddColumn<bool>(
                name: "StatusWatch",
                table: "Statuses",
                type: "bit",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 29, 20, 39, 9, 854, DateTimeKind.Local).AddTicks(2778), new DateTime(2021, 8, 29, 20, 39, 9, 851, DateTimeKind.Local).AddTicks(9717) });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 1,
                column: "StatusWatch",
                value: false);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: 2,
                column: "StatusWatch",
                value: true);
        }
    }
}
