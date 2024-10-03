using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddedShiftSequenceConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftsQuery",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "QueryCode",
                table: "TemplatedShifts");

            migrationBuilder.CreateTable(
                name: "ShiftSequenceEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    ShiftId = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftSequenceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftSequenceEntity_TemplatedShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "TemplatedShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftSequenceEntity_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSequenceEntity_ShiftId",
                table: "ShiftSequenceEntity",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSequenceEntity_TemplateId",
                table: "ShiftSequenceEntity",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftSequenceEntity");

            migrationBuilder.AddColumn<string>(
                name: "ShiftsQuery",
                table: "Templates",
                type: "character varying(31)",
                maxLength: 31,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<char>(
                name: "QueryCode",
                table: "TemplatedShifts",
                type: "character(1)",
                nullable: false,
                defaultValue: '\0');
        }
    }
}
