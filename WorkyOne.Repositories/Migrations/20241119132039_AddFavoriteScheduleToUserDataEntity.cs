using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteScheduleToUserDataEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FavoriteScheduleId",
                table: "UserDatas",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDatas_FavoriteScheduleId",
                table: "UserDatas",
                column: "FavoriteScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas",
                column: "FavoriteScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Schedules_FavoriteScheduleId",
                table: "UserDatas");

            migrationBuilder.DropIndex(
                name: "IX_UserDatas_FavoriteScheduleId",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "FavoriteScheduleId",
                table: "UserDatas");
        }
    }
}
