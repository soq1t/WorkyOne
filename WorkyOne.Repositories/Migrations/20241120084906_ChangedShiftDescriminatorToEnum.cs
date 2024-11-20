using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ChangedShiftDescriminatorToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeTemp",
                table: "Shifts",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.Sql(
                @"
                UPDATE ""Shifts""
                SET ""TypeTemp"" = 
                CASE 
                    WHEN ""Type"" = 'Personal' THEN 1
                    ELSE 0
                END;
    "
            );

            migrationBuilder.DropColumn(name: "Type", table: "Shifts");

            migrationBuilder.RenameColumn(name: "TypeTemp", table: "Shifts", newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeTemp",
                table: "Shifts",
                nullable: false,
                defaultValue: "Shared"
            );

            migrationBuilder.Sql(
                @"
                UPDATE ""Shifts""
                SET ""TypeTemp"" = 
                CASE 
                    WHEN ""Type"" = 1 THEN 'Personal'
                    ELSE 'Shared'
                END;
    "
            );

            migrationBuilder.DropColumn(name: "Type", table: "Shifts");

            migrationBuilder.RenameColumn(name: "TypeTemp", table: "Shifts", newName: "Type");
        }
    }
}
