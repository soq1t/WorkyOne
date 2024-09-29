using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class TemplateStartMirroring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_UserDataEntity_UserDataId",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDataEntity",
                table: "UserDataEntity");

            migrationBuilder.RenameTable(
                name: "UserDataEntity",
                newName: "UserDatas");

            migrationBuilder.AddColumn<bool>(
                name: "IsMirrored",
                table: "Templates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Templates",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_UserDatas_UserDataId",
                table: "Templates",
                column: "UserDataId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_UserDatas_UserDataId",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDatas",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "IsMirrored",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Templates");

            migrationBuilder.RenameTable(
                name: "UserDatas",
                newName: "UserDataEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDataEntity",
                table: "UserDataEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_UserDataEntity_UserDataId",
                table: "Templates",
                column: "UserDataId",
                principalTable: "UserDataEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
