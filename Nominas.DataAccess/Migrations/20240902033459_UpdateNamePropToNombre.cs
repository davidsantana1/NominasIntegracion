using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNamePropToNombre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Departamentos",
                newName: "Nombre");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Puestos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Puestos");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Departamentos",
                newName: "Name");
        }
    }
}
