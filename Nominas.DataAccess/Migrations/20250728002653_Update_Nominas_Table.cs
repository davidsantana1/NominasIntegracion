using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nominas.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Update_Nominas_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdAsiento",
                table: "Nominas",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAsiento",
                table: "Nominas");
        }
    }
}
