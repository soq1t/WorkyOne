using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: true),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDataEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDataEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserDataId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateEntity_UserDataEntity_UserDataId",
                        column: x => x.UserDataId,
                        principalTable: "UserDataEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftRepititions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ShiftId = table.Column<string>(type: "text", nullable: false),
                    RepetitionAmount = table.Column<int>(type: "integer", nullable: false),
                    TemplateEntityId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftRepititions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftRepititions_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftRepititions_TemplateEntity_TemplateEntityId",
                        column: x => x.TemplateEntityId,
                        principalTable: "TemplateEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SingleDayShiftEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ShiftId = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    ShiftDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleDayShiftEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleDayShiftEntity_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SingleDayShiftEntity_TemplateEntity_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "TemplateEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftRepititions_ShiftId",
                table: "ShiftRepititions",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftRepititions_TemplateEntityId",
                table: "ShiftRepititions",
                column: "TemplateEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleDayShiftEntity_ShiftId",
                table: "SingleDayShiftEntity",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_SingleDayShiftEntity_TemplateId",
                table: "SingleDayShiftEntity",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateEntity_UserDataId",
                table: "TemplateEntity",
                column: "UserDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftRepititions");

            migrationBuilder.DropTable(
                name: "SingleDayShiftEntity");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "TemplateEntity");

            migrationBuilder.DropTable(
                name: "UserDataEntity");
        }
    }
}
