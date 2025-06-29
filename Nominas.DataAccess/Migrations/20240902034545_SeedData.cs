using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departamentos",
                columns: new[] { "Id", "Nombre", "ResponsableDeAreaId", "UbicacionFisica" },
                values: new object[,]
                {
                    { 1, "IT", 1, "Edificio A" },
                    { 2, "Recursos Humanos", 2, "Edificio B" }
                });

            migrationBuilder.InsertData(
                table: "Puestos",
                columns: new[] { "Id", "NivelDeRiesgo", "NivelMaximoSalario", "NivelMinimoSalario", "Nombre" },
                values: new object[,]
                {
                    { 1, "Medio", 60000m, 30000m, "Desarrollador" },
                    { 2, "Bajo", 50000m, 25000m, "Analista" }
                });

            migrationBuilder.InsertData(
                table: "Empleados",
                columns: new[] { "Id", "Cedula", "DepartamentoId", "Nombre", "NominaId", "PuestoId", "SalarioMensual" },
                values: new object[,]
                {
                    { 1, "001-1234567-8", 1, "Juan Pérez", 1, 1, 35000m },
                    { 2, "002-2345678-9", 2, "Ana Gómez", 1, 2, 28000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departamentos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Puestos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Puestos",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
