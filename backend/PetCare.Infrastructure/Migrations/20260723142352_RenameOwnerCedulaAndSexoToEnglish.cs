using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class RenameOwnerCedulaAndSexoToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sexo",
                table: "owners",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "cedula",
                table: "owners",
                newName: "id_card");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "gender",
                table: "owners",
                newName: "sexo");

            migrationBuilder.RenameColumn(
                name: "id_card",
                table: "owners",
                newName: "cedula");
        }
    }
}
