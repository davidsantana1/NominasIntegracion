using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Nomina_Empleado_FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nominas_Empleados_EmpleadoId1",
                table: "Nominas");

            migrationBuilder.DropIndex(
                name: "IX_Nominas_EmpleadoId1",
                table: "Nominas");

            migrationBuilder.DropColumn(
                name: "EmpleadoId1",
                table: "Nominas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeIngreso",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeDeducciones",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Nominas_EmpleadoId",
                table: "Nominas",
                column: "EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nominas_Empleados_EmpleadoId",
                table: "Nominas",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nominas_Empleados_EmpleadoId",
                table: "Nominas");

            migrationBuilder.DropIndex(
                name: "IX_Nominas_EmpleadoId",
                table: "Nominas");

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeIngreso",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "TiposDeDeducciones",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId1",
                table: "Nominas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Nominas_EmpleadoId1",
                table: "Nominas",
                column: "EmpleadoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Nominas_Empleados_EmpleadoId1",
                table: "Nominas",
                column: "EmpleadoId1",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
