using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ShiftsTemplatesConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftRepititions_TemplateEntity_TemplateEntityId",
                table: "ShiftRepititions");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDayShiftEntity_Shifts_ShiftId",
                table: "SingleDayShiftEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDayShiftEntity_TemplateEntity_TemplateId",
                table: "SingleDayShiftEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateEntity_UserDataEntity_UserDataId",
                table: "TemplateEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateEntity",
                table: "TemplateEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleDayShiftEntity",
                table: "SingleDayShiftEntity");

            migrationBuilder.RenameTable(
                name: "TemplateEntity",
                newName: "Templates");

            migrationBuilder.RenameTable(
                name: "SingleDayShiftEntity",
                newName: "SingleDayShifts");

            migrationBuilder.RenameIndex(
                name: "IX_TemplateEntity_UserDataId",
                table: "Templates",
                newName: "IX_Templates_UserDataId");

            migrationBuilder.RenameIndex(
                name: "IX_SingleDayShiftEntity_TemplateId",
                table: "SingleDayShifts",
                newName: "IX_SingleDayShifts_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_SingleDayShiftEntity_ShiftId",
                table: "SingleDayShifts",
                newName: "IX_SingleDayShifts_ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Templates",
                table: "Templates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleDayShifts",
                table: "SingleDayShifts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShiftEntityTemplateEntity",
                columns: table => new
                {
                    ShiftsId = table.Column<string>(type: "text", nullable: false),
                    TemplatesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftEntityTemplateEntity", x => new { x.ShiftsId, x.TemplatesId });
                    table.ForeignKey(
                        name: "FK_ShiftEntityTemplateEntity_Shifts_ShiftsId",
                        column: x => x.ShiftsId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftEntityTemplateEntity_Templates_TemplatesId",
                        column: x => x.TemplatesId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftEntityTemplateEntity_TemplatesId",
                table: "ShiftEntityTemplateEntity",
                column: "TemplatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftRepititions_Templates_TemplateEntityId",
                table: "ShiftRepititions",
                column: "TemplateEntityId",
                principalTable: "Templates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDayShifts_Shifts_ShiftId",
                table: "SingleDayShifts",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDayShifts_Templates_TemplateId",
                table: "SingleDayShifts",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_UserDataEntity_UserDataId",
                table: "Templates",
                column: "UserDataId",
                principalTable: "UserDataEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftRepititions_Templates_TemplateEntityId",
                table: "ShiftRepititions");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDayShifts_Shifts_ShiftId",
                table: "SingleDayShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleDayShifts_Templates_TemplateId",
                table: "SingleDayShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_UserDataEntity_UserDataId",
                table: "Templates");

            migrationBuilder.DropTable(
                name: "ShiftEntityTemplateEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Templates",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SingleDayShifts",
                table: "SingleDayShifts");

            migrationBuilder.RenameTable(
                name: "Templates",
                newName: "TemplateEntity");

            migrationBuilder.RenameTable(
                name: "SingleDayShifts",
                newName: "SingleDayShiftEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Templates_UserDataId",
                table: "TemplateEntity",
                newName: "IX_TemplateEntity_UserDataId");

            migrationBuilder.RenameIndex(
                name: "IX_SingleDayShifts_TemplateId",
                table: "SingleDayShiftEntity",
                newName: "IX_SingleDayShiftEntity_TemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_SingleDayShifts_ShiftId",
                table: "SingleDayShiftEntity",
                newName: "IX_SingleDayShiftEntity_ShiftId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateEntity",
                table: "TemplateEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SingleDayShiftEntity",
                table: "SingleDayShiftEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftRepititions_TemplateEntity_TemplateEntityId",
                table: "ShiftRepititions",
                column: "TemplateEntityId",
                principalTable: "TemplateEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDayShiftEntity_Shifts_ShiftId",
                table: "SingleDayShiftEntity",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleDayShiftEntity_TemplateEntity_TemplateId",
                table: "SingleDayShiftEntity",
                column: "TemplateId",
                principalTable: "TemplateEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateEntity_UserDataEntity_UserDataId",
                table: "TemplateEntity",
                column: "UserDataId",
                principalTable: "UserDataEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
