using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveToOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "owners",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
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
                name: "OwnerId",
                table: "owners");
        }
    }
}
