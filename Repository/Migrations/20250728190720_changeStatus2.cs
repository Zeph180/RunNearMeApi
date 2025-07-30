using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class changeStatus2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RunnerId",
                table: "Friends",
                newName: "RequestTo");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestFrom",
                table: "Friends",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestFrom",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "RequestTo",
                table: "Friends",
                newName: "RunnerId");
        }
    }
}
