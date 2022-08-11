using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejemplo1.Migrations
{
    public partial class SeedAmigosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Email", "Nombre" },
                values: new object[] { 1, 30, "mail@mimail.es", "Pepe" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amigos",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
