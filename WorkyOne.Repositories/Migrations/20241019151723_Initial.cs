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
                name: "ExampleShifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: true),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExampleShifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ScheduleId = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDatas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplatedShifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: true),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatedShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplatedShifts_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserDataId = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Schedules_UserDatas_UserDataId",
                        column: x => x.UserDataId,
                        principalTable: "UserDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftSequences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TemplateId = table.Column<string>(type: "text", nullable: false),
                    ShiftId = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftSequences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftSequences_TemplatedShifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "TemplatedShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftSequences_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ScheduleId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: false),
                    IsBusyDay = table.Column<bool>(type: "boolean", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    ShiftProlongation = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyInfos_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatedShifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ScheduleId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: true),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatedShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DatedShifts_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeriodicShifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ScheduleId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ColorCode = table.Column<string>(type: "text", nullable: true),
                    Beginning = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    Ending = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodicShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodicShifts_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyInfos_ScheduleId",
                table: "DailyInfos",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DatedShifts_ScheduleId",
                table: "DatedShifts",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodicShifts_ScheduleId",
                table: "PeriodicShifts",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TemplateId",
                table: "Schedules",
                column: "TemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_UserDataId",
                table: "Schedules",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSequences_ShiftId",
                table: "ShiftSequences",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftSequences_TemplateId",
                table: "ShiftSequences",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplatedShifts_TemplateId",
                table: "TemplatedShifts",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyInfos");

            migrationBuilder.DropTable(
                name: "DatedShifts");

            migrationBuilder.DropTable(
                name: "ExampleShifts");

            migrationBuilder.DropTable(
                name: "PeriodicShifts");

            migrationBuilder.DropTable(
                name: "ShiftSequences");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "TemplatedShifts");

            migrationBuilder.DropTable(
                name: "UserDatas");

            migrationBuilder.DropTable(
                name: "Templates");
        }
    }
}
