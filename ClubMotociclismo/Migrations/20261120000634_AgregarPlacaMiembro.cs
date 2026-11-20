using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClubMotociclismo.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPlacaMiembro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Placa",
                table: "Miembros",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Miembros",
                keyColumn: "Id",
                keyValue: 1,
                column: "Placa",
                value: null);

            migrationBuilder.UpdateData(
                table: "Miembros",
                keyColumn: "Id",
                keyValue: 2,
                column: "Placa",
                value: null);

            migrationBuilder.UpdateData(
                table: "Miembros",
                keyColumn: "Id",
                keyValue: 3,
                column: "Placa",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placa",
                table: "Miembros");
        }
    }
}
