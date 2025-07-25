using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Groups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challenge",
                columns: table => new
                {
                    ChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenge", x => x.ChallengeId);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeRunner",
                columns: table => new
                {
                    ChallengesChallengeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunnersRunnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRunner", x => new { x.ChallengesChallengeId, x.RunnersRunnerId });
                    table.ForeignKey(
                        name: "FK_ChallengeRunner_Challenge_ChallengesChallengeId",
                        column: x => x.ChallengesChallengeId,
                        principalTable: "Challenge",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChallengeRunner_Runners_RunnersRunnerId",
                        column: x => x.RunnersRunnerId,
                        principalTable: "Runners",
                        principalColumn: "RunnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRunner",
                columns: table => new
                {
                    GroupsGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RunnersRunnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRunner", x => new { x.GroupsGroupId, x.RunnersRunnerId });
                    table.ForeignKey(
                        name: "FK_GroupRunner_Group_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRunner_Runners_RunnersRunnerId",
                        column: x => x.RunnersRunnerId,
                        principalTable: "Runners",
                        principalColumn: "RunnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRunner_RunnersRunnerId",
                table: "ChallengeRunner",
                column: "RunnersRunnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRunner_RunnersRunnerId",
                table: "GroupRunner",
                column: "RunnersRunnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeRunner");

            migrationBuilder.DropTable(
                name: "GroupRunner");

            migrationBuilder.DropTable(
                name: "Challenge");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
