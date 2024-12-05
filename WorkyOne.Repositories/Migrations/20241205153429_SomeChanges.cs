using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SomeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas",
                column: "FavoriteScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas",
                column: "FavoriteScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
