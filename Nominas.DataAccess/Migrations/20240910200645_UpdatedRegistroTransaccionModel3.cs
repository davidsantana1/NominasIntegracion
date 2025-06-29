using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRegistroTransaccionModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistroTransacciones_Empleados_EmpleadoId",
                table: "RegistroTransacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroTransacciones_Empleados_EmpleadoId",
                table: "RegistroTransacciones",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistroTransacciones_Empleados_EmpleadoId",
                table: "RegistroTransacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroTransacciones_Empleados_EmpleadoId",
                table: "RegistroTransacciones",
                column: "EmpleadoId",
                principalTable: "Empleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
