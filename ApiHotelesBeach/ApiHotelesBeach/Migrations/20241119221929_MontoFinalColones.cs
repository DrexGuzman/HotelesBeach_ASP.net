using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHotelesBeach.Migrations
{
    /// <inheritdoc />
    public partial class MontoFinalColones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MontoFinalColones",
                table: "Reservas",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "FormasPago",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "FormasPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Numero",
                value: null);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Cedula",
                keyValue: "604880492",
                column: "FechaRegistro",
                value: new DateTime(2024, 11, 19, 16, 19, 27, 770, DateTimeKind.Local).AddTicks(8354));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontoFinalColones",
                table: "Reservas");

            migrationBuilder.AlterColumn<int>(
                name: "Numero",
                table: "FormasPago",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "FormasPago",
                keyColumn: "Id",
                keyValue: 1,
                column: "Numero",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Cedula",
                keyValue: "604880492",
                column: "FechaRegistro",
                value: new DateTime(2024, 11, 19, 11, 28, 6, 256, DateTimeKind.Local).AddTicks(8498));
        }
    }
}
