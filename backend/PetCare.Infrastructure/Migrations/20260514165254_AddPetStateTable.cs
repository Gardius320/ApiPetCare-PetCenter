using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCare.Migrations
{
    /// <inheritdoc />
    public partial class AddPetStateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "owners",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    owner_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__owners__3213E83FD227B16C", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pet_states",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pet_states", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specie_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    create_at = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__species__3213E83FAABCB7CE", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    id_state = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    state_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__states__12FD6C4943565E84", x => x.id_state);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pet_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    specie_id = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    pet_state_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pets__3213E83FB644E679", x => x.id);
                    table.ForeignKey(
                        name: "FK__pets__owner_id__2B3F6F97",
                        column: x => x.owner_id,
                        principalTable: "owners",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__pets__pet_state_id",
                        column: x => x.pet_state_id,
                        principalTable: "pet_states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK__pets__specie_id__2A4B4B5E",
                        column: x => x.specie_id,
                        principalTable: "species",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appoinment_date = table.Column<DateOnly>(type: "date", nullable: false),
                    observation = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    create_at = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    state_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__appointm__3213E83F7AACF5C9", x => x.id);
                    table.ForeignKey(
                        name: "FK__appointme__owner__33D4B598",
                        column: x => x.owner_id,
                        principalTable: "owners",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__appointme__state__32E0915F",
                        column: x => x.state_id,
                        principalTable: "states",
                        principalColumn: "id_state");
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_owner_id",
                table: "appointments",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_state_id",
                table: "appointments",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_pets_owner_id",
                table: "pets",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_pets_pet_state_id",
                table: "pets",
                column: "pet_state_id");

            migrationBuilder.CreateIndex(
                name: "IX_pets_specie_id",
                table: "pets",
                column: "specie_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "owners");

            migrationBuilder.DropTable(
                name: "pet_states");

            migrationBuilder.DropTable(
                name: "species");
        }
    }
}
