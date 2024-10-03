using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddedExampleShiftEntity : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExampleShifts");
        }
    }
}
