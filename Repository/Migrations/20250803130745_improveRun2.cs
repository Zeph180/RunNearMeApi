using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class improveRun2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunRoutePoints_Runs_RunId",
                table: "RunRoutePoints");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Runs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Runs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RunId",
                table: "RunRoutePoints",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "RunRoutePoints",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "RunRoutePoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Profiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Profiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RunRoutePoints_Runs_RunId",
                table: "RunRoutePoints",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "RunId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunRoutePoints_Runs_RunId",
                table: "RunRoutePoints");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "RunRoutePoints");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "RunRoutePoints");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Profiles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Runs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RunId",
                table: "RunRoutePoints",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_RunRoutePoints_Runs_RunId",
                table: "RunRoutePoints",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "RunId");
        }
    }
}
