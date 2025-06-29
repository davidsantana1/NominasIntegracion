using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedTipoIngresoDeduccionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TiposDeDeducciones",
                columns: new[] { "Id", "DependeDeSalario", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, true, true, "AFP" },
                    { 2, true, true, "ARS" },
                    { 3, false, true, "Seguro de Vida" },
                    { 4, false, true, "Préstamo Personal" }
                });

            migrationBuilder.InsertData(
                table: "TiposDeIngreso",
                columns: new[] { "Id", "DependeDeSalario", "Estado", "Nombre" },
                values: new object[,]
                {
                    { 1, true, true, "Bono de Desempeño" },
                    { 2, true, true, "Gratificación Anual" },
                    { 3, true, true, "Comisiones" },
                    { 4, false, true, "Reembolso de Gastos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
