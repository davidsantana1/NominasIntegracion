using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPorcentajeToTipoDeDeduccionAndTipoDeIngreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeIngreso",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeDeducciones",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 1,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 2,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 3,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeDeducciones",
                keyColumn: "Id",
                keyValue: 4,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 1,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 2,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 3,
                column: "Porcentaje",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "TiposDeIngreso",
                keyColumn: "Id",
                keyValue: 4,
                column: "Porcentaje",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Porcentaje",
                table: "TiposDeIngreso");

            migrationBuilder.DropColumn(
                name: "Porcentaje",
                table: "TiposDeDeducciones");
        }
    }
}
