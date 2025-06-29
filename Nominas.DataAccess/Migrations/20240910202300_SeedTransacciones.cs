using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedTransacciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RegistroTransacciones",
                columns: new[] { "Id", "Deduccion", "EmpleadoId", "Estado", "Fecha", "Ingreso", "Monto", "TipoTransaccion" },
                values: new object[,]
                {
                    { 1, 200m, 1, "Aprobado", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000m, 800m, "Ingreso" },
                    { 2, 150m, 2, "Aprobado", new DateTime(2024, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1200m, 1050m, "Bono" },
                    { 3, 50m, 1, "Pendiente", new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, 450m, "Horas Extras" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
