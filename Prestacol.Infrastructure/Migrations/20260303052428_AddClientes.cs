using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prestacol.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Clientes",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "Cedula",
                table: "Clientes",
                newName: "Documento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "Clientes",
                newName: "FechaRegistro");

            migrationBuilder.RenameColumn(
                name: "Documento",
                table: "Clientes",
                newName: "Cedula");
        }
    }
}
