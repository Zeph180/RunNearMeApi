using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class init120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Profiles_ProfileRunnerId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ProfileRunnerId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ProfileRunnerId",
                table: "Notifications");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RunnerId",
                table: "Notifications",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Profiles_RunnerId",
                table: "Notifications",
                column: "RunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Profiles_RunnerId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RunnerId",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileRunnerId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ProfileRunnerId",
                table: "Notifications",
                column: "ProfileRunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Profiles_ProfileRunnerId",
                table: "Notifications",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");
        }
    }
}
