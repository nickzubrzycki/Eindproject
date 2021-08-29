using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 29, 15, 26, 29, 864, DateTimeKind.Local).AddTicks(3451), new DateTime(2021, 8, 29, 15, 26, 29, 862, DateTimeKind.Local).AddTicks(1268) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Lijsts",
                keyColumn: "LijstId",
                keyValue: 1,
                columns: new[] { "BewerktOp", "ToegeVoegdOp" },
                values: new object[] { new DateTime(2021, 8, 26, 10, 2, 53, 11, DateTimeKind.Local).AddTicks(2714), new DateTime(2021, 8, 26, 10, 2, 53, 2, DateTimeKind.Local).AddTicks(2967) });
        }
    }
}
