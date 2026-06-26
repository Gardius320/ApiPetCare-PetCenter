using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSexoyActividad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "owners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Sexo",
                table: "owners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "owners");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "owners");
        }
    }
}
