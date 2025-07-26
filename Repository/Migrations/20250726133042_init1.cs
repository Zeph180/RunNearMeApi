using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Groups",
                newName: "RunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Challenges",
                newName: "RunnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Groups",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Challenges",
                newName: "ProfileId");
        }
    }
}
