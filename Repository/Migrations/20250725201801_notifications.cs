using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class notifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Runners_RunnerId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_RunnerId",
                table: "Notifications",
                newName: "IX_Notifications_RunnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Runners_RunnerId",
                table: "Notifications",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Runners_RunnerId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_RunnerId",
                table: "Notification",
                newName: "IX_Notification_RunnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Runners_RunnerId",
                table: "Notification",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
