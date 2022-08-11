using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejemplo1.Migrations
{
    public partial class Extend_IndentityUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ayudaPass",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ayudaPass",
                table: "AspNetUsers");
        }
    }
}
