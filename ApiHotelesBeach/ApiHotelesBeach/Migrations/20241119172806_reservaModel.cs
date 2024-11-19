using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHotelesBeach.Migrations
{
    /// <inheritdoc />
    public partial class reservaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PagoMes",
                table: "Reservas",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Prima",
                table: "Reservas",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Cedula",
                keyValue: "604880492",
                column: "FechaRegistro",
                value: new DateTime(2024, 11, 19, 11, 28, 6, 256, DateTimeKind.Local).AddTicks(8498));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagoMes",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Prima",
                table: "Reservas");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Cedula",
                keyValue: "604880492",
                column: "FechaRegistro",
                value: new DateTime(2024, 11, 18, 15, 45, 44, 47, DateTimeKind.Local).AddTicks(660));
        }
    }
}
