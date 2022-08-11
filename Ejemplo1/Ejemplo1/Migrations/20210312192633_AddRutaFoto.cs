using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejemplo1.Migrations
{
    public partial class AddRutaFoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rutaFoto",
                table: "Amigos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rutaFoto",
                table: "Amigos");
        }
    }
}
