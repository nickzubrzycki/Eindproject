using Microsoft.EntityFrameworkCore.Migrations;

namespace Eindproject.Data.Migrations
{
    public partial class plzs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vriend",
                columns: table => new
                {
                    VriendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BevriendId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vriend", x => x.VriendId);
                    table.ForeignKey(
                        name: "FK_Vriend_AspNetUsers_BevriendId",
                        column: x => x.BevriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vriend_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vriend_BevriendId",
                table: "Vriend",
                column: "BevriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Vriend_UserId",
                table: "Vriend",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vriend");
        }
    }
}
