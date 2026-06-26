using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class NombreMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__pets__specie_id__2A4B4B5E",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "appoinment_date",
                table: "appointments");

            migrationBuilder.AddColumn<DateTime>(
                name: "appointment_date",
                table: "appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_pets_species_specie_id",
                table: "pets",
                column: "specie_id",
                principalTable: "species",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pets_species_specie_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "appointment_date",
                table: "appointments");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "owners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "appoinment_date",
                table: "appointments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddForeignKey(
                name: "FK__pets__specie_id__2A4B4B5E",
                table: "pets",
                column: "specie_id",
                principalTable: "species",
                principalColumn: "id");
        }
    }
}
