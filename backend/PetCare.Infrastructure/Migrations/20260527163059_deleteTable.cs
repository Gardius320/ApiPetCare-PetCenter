using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class deleteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__pets__pet_state_id",
                table: "pets");

            migrationBuilder.DropTable(
                name: "pet_states");

            migrationBuilder.DropIndex(
                name: "IX_pets_pet_state_id",
                table: "pets");

            migrationBuilder.DropColumn(
                name: "pet_state_id",
                table: "pets");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "pets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "pets");

            migrationBuilder.AddColumn<int>(
                name: "pet_state_id",
                table: "pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "pet_states",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "text", nullable: true),
                    state_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pets_pet_state_id",
                table: "pets",
                column: "pet_state_id");

            migrationBuilder.AddForeignKey(
                name: "FK__pets__pet_state_id",
                table: "pets",
                column: "pet_state_id",
                principalTable: "pet_states",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
