using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejemplo1.Migrations
{
    public partial class Extend_Identity_UsuarioAplicacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ayudaPass",
                table: "AspNetUsers",
                type: "nvarchar(max)",
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
