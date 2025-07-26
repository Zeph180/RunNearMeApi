using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class init12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Likes",
                newName: "RunnerId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "Comments",
                newName: "RunnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Likes",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Comments",
                newName: "ProfileId");
        }
    }
}
