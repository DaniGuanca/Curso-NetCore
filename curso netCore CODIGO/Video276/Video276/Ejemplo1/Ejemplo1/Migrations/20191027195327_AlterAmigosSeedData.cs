using Microsoft.EntityFrameworkCore.Migrations;

namespace Ejemplo1.Migrations
{
    public partial class AlterAmigosSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Email", "Nombre" },
                values: new object[] { 2, 29, "mail@mimail2.es", "juan" });

            migrationBuilder.InsertData(
                table: "Amigos",
                columns: new[] { "Id", "Ciudad", "Email", "Nombre" },
                values: new object[] { 3, 14, "mail@mimail3.es", "Laura" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Amigos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amigos",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
