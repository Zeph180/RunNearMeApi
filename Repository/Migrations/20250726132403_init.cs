using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeProfile_Profiles_ProfilesProfileId",
                table: "ChallengeProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Profiles_ProfileId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupProfile_Profiles_ProfilesProfileId",
                table: "GroupProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Profiles_ProfileId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Profiles_ProfileId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Profiles_ProfileId",
                table: "Runs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_RunnerId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Runs",
                newName: "ProfileRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Runs_ProfileId",
                table: "Runs",
                newName: "IX_Runs_ProfileRunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Posts",
                newName: "ProfileRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ProfileId",
                table: "Posts",
                newName: "IX_Posts_ProfileRunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Notifications",
                newName: "ProfileRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ProfileId",
                table: "Notifications",
                newName: "IX_Notifications_ProfileRunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfilesProfileId",
                table: "GroupProfile",
                newName: "ProfilesRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupProfile_ProfilesProfileId",
                table: "GroupProfile",
                newName: "IX_GroupProfile_ProfilesRunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Friends",
                newName: "ProfileRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_ProfileId",
                table: "Friends",
                newName: "IX_Friends_ProfileRunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfilesProfileId",
                table: "ChallengeProfile",
                newName: "ProfilesRunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_ChallengeProfile_ProfilesProfileId",
                table: "ChallengeProfile",
                newName: "IX_ChallengeProfile_ProfilesRunnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeProfile_Profiles_ProfilesRunnerId",
                table: "ChallengeProfile",
                column: "ProfilesRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Profiles_ProfileRunnerId",
                table: "Friends",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupProfile_Profiles_ProfilesRunnerId",
                table: "GroupProfile",
                column: "ProfilesRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Profiles_ProfileRunnerId",
                table: "Notifications",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Profiles_ProfileRunnerId",
                table: "Posts",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Profiles_ProfileRunnerId",
                table: "Runs",
                column: "ProfileRunnerId",
                principalTable: "Profiles",
                principalColumn: "RunnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChallengeProfile_Profiles_ProfilesRunnerId",
                table: "ChallengeProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Profiles_ProfileRunnerId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupProfile_Profiles_ProfilesRunnerId",
                table: "GroupProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Profiles_ProfileRunnerId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Profiles_ProfileRunnerId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Runs_Profiles_ProfileRunnerId",
                table: "Runs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "ProfileRunnerId",
                table: "Runs",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Runs_ProfileRunnerId",
                table: "Runs",
                newName: "IX_Runs_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfileRunnerId",
                table: "Posts",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ProfileRunnerId",
                table: "Posts",
                newName: "IX_Posts_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfileRunnerId",
                table: "Notifications",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_ProfileRunnerId",
                table: "Notifications",
                newName: "IX_Notifications_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfilesRunnerId",
                table: "GroupProfile",
                newName: "ProfilesProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupProfile_ProfilesRunnerId",
                table: "GroupProfile",
                newName: "IX_GroupProfile_ProfilesProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfileRunnerId",
                table: "Friends",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_ProfileRunnerId",
                table: "Friends",
                newName: "IX_Friends_ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfilesRunnerId",
                table: "ChallengeProfile",
                newName: "ProfilesProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_ChallengeProfile_ProfilesRunnerId",
                table: "ChallengeProfile",
                newName: "IX_ChallengeProfile_ProfilesProfileId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Profiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                table: "Profiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_RunnerId",
                table: "Profiles",
                column: "RunnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChallengeProfile_Profiles_ProfilesProfileId",
                table: "ChallengeProfile",
                column: "ProfilesProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Profiles_ProfileId",
                table: "Friends",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupProfile_Profiles_ProfilesProfileId",
                table: "GroupProfile",
                column: "ProfilesProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Profiles_ProfileId",
                table: "Notifications",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Profiles_ProfileId",
                table: "Posts",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Runs_Profiles_ProfileId",
                table: "Runs",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId");
        }
    }
}
