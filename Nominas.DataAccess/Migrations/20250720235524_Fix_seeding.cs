using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RegistroTransacciones",
                columns: new[] { "Id", "Deduccion", "EmpleadoId", "Estado", "Fecha", "Ingreso", "Monto", "TipoTransaccion" },
                values: new object[] { 3, 50m, 1, "Pendiente", new DateTime(2024, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, 450m, 0 });
        }
    }
}
