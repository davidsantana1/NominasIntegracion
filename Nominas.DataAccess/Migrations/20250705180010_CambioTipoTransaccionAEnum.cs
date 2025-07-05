using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CambioTipoTransaccionAEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoTransaccion",
                table: "RegistroTransacciones",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoTransaccion",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoTransaccion",
                value: 0);

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoTransaccion",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoTransaccion",
                table: "RegistroTransacciones",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoTransaccion",
                value: "Ingreso");

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoTransaccion",
                value: "Bono");

            migrationBuilder.UpdateData(
                table: "RegistroTransacciones",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoTransaccion",
                value: "Horas Extras");
        }
    }
}
