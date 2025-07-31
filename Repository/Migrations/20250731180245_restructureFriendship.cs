using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class restructureFriendship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendProfile");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileRunnerId",
                table: "Friends",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friends_ProfileRunnerId",
                table: "Friends",
                column: "ProfileRunnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_RequestFrom",
                table: "Friends",
                column: "RequestFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_RequestTo",
                table: "Friends",
                column: "RequestTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Profiles_ProfileRunnerId",
                table: "Friends",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Profiles_RequestFrom",
                table: "Friends",
                column: "RequestFrom",
                principalTable: "Profiles",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Profiles_RequestTo",
                table: "Friends",
                column: "RequestTo",
                principalTable: "Profiles",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Profiles_ProfileRunnerId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Profiles_RequestFrom",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Profiles_RequestTo",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_ProfileRunnerId",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_RequestFrom",
                table: "Friends");

            migrationBuilder.DropIndex(
                name: "IX_Friends_RequestTo",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "ProfileRunnerId",
                table: "Friends");

            migrationBuilder.CreateTable(
                name: "FriendProfile",
                columns: table => new
                {
                    FriendsFriendId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfilesRunnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendProfile", x => new { x.FriendsFriendId, x.ProfilesRunnerId });
                    table.ForeignKey(
                        name: "FK_FriendProfile_Friends_FriendsFriendId",
                        column: x => x.FriendsFriendId,
                        principalTable: "Friends",
                        principalColumn: "FriendId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendProfile_Profiles_ProfilesRunnerId",
                        column: x => x.ProfilesRunnerId,
                        principalTable: "Profiles",
                        principalColumn: "RunnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendProfile_ProfilesRunnerId",
                table: "FriendProfile",
                column: "ProfilesRunnerId");
        }
    }
}
