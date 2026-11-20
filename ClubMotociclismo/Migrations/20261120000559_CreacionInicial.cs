using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClubMotociclismo.Migrations
{
    /// <inheritdoc />
    public partial class CreacionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lugar = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Planes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CuotaMensual = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Miembros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMoto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cilindraje = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    CuotaMensual = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miembros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miembros_Planes_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Planes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "Id", "Descripcion", "Fecha", "Lugar", "Tipo", "Titulo" },
                values: new object[,]
                {
                    { 1, "Salida temprano para ver el amanecer y desayunar arriba.", new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parque principal", "Rodada", "Ruta a la montaña" },
                    { 2, "Aprende a revisar cadena, frenos y aceite de tu moto.", new DateTime(2026, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sede del club", "Taller", "Mantenimiento básico" },
                    { 3, "Fin de semana rodando por los pueblos cercanos.", new DateTime(2026, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Terminal norte", "Viaje", "Viaje regional" }
                });

            migrationBuilder.InsertData(
                table: "Planes",
                columns: new[] { "Id", "CuotaMensual", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, 30000m, "Rodadas grupales de fin de semana.", "Básica" },
                    { 2, 60000m, "Rodadas, talleres de mantenimiento y descuentos en tiendas aliadas.", "Gold" },
                    { 3, 90000m, "Todo lo del plan Gold más viajes regionales con asistencia.", "Aventura" }
                });

            migrationBuilder.InsertData(
                table: "Miembros",
                columns: new[] { "Id", "Cilindraje", "Correo", "CuotaMensual", "Edad", "FechaIngreso", "Nombre", "PlanId", "Telefono", "TipoMoto" },
                values: new object[,]
                {
                    { 1, 650, "andres@correo.com", 70000m, 34, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andrés Gómez", 2, "3001112233", "Touring" },
                    { 2, 300, "laura@correo.com", 30000m, 27, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laura Pérez", 1, "3104445566", "Naked" },
                    { 3, 450, "julian@correo.com", 90000m, 41, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Julián Torres", 3, "3207778899", "Enduro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_Correo",
                table: "Miembros",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_PlanId",
                table: "Miembros",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Planes_Nombre",
                table: "Planes",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Miembros");

            migrationBuilder.DropTable(
                name: "Planes");
        }
    }
}
