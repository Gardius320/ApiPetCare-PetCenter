using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class addApointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_PetId",
                table: "appointments",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_pets_PetId",
                table: "appointments",
                column: "PetId",
                principalTable: "pets",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_pets_PetId",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_PetId",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "appointments");
        }
    }
}
