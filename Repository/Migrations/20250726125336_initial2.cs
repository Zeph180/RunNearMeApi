using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Challenges_Profiles_ProfileId",
                table: "Challenges");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Profiles_ProfileId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Runners_Challenges_ChallengeId",
                table: "Runners");

            migrationBuilder.DropForeignKey(
                name: "FK_Runners_Groups_GroupId",
                table: "Runners");

            migrationBuilder.DropIndex(
                name: "IX_Runners_ChallengeId",
                table: "Runners");

            migrationBuilder.DropIndex(
                name: "IX_Runners_GroupId",
                table: "Runners");

            migrationBuilder.DropIndex(
                name: "IX_Groups_ProfileId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Challenges_ProfileId",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "ChallengeId",
                table: "Runners");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Runners");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Challenges");

            migrationBuilder.CreateTable(
                name: "ChallengeProfile",
                columns: table => new
                {
                    ChallengesChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfilesProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeProfile", x => new { x.ChallengesChallengeId, x.ProfilesProfileId });
                    table.ForeignKey(
                        name: "FK_ChallengeProfile_Challenges_ChallengesChallengeId",
                        column: x => x.ChallengesChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeProfile_Profiles_ProfilesProfileId",
                        column: x => x.ProfilesProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupProfile",
                columns: table => new
                {
                    GroupsGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfilesProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupProfile", x => new { x.GroupsGroupId, x.ProfilesProfileId });
                    table.ForeignKey(
                        name: "FK_GroupProfile_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupProfile_Profiles_ProfilesProfileId",
                        column: x => x.ProfilesProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeProfile_ProfilesProfileId",
                table: "ChallengeProfile",
                column: "ProfilesProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupProfile_ProfilesProfileId",
                table: "GroupProfile",
                column: "ProfilesProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeProfile");

            migrationBuilder.DropTable(
                name: "GroupProfile");

            migrationBuilder.AddColumn<Guid>(
                name: "ChallengeId",
                table: "Runners",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Runners",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Challenges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Runners_ChallengeId",
                table: "Runners",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_Runners_GroupId",
                table: "Runners",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ProfileId",
                table: "Groups",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_ProfileId",
                table: "Challenges",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Challenges_Profiles_ProfileId",
                table: "Challenges",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Profiles_ProfileId",
                table: "Groups",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Runners_Challenges_ChallengeId",
                table: "Runners",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "ChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Runners_Groups_GroupId",
                table: "Runners",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }
    }
}
