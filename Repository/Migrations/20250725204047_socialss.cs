using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class socialss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Friend_Runners_RunnerId",
                table: "Friend");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Comment_CommentId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Post_PostId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Runners_RunnerId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friend",
                table: "Friend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "Likes");

            migrationBuilder.RenameTable(
                name: "Friend",
                newName: "Friends");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Post_RunnerId",
                table: "Posts",
                newName: "IX_Posts_RunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_PostId",
                table: "Likes",
                newName: "IX_Likes_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_CommentId",
                table: "Likes",
                newName: "IX_Likes_CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Friend_RunnerId",
                table: "Friends",
                newName: "IX_Friends_RunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "Comments",
                newName: "IX_Comments_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "LikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                column: "FriendId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Runners_RunnerId",
                table: "Friends",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Runners_RunnerId",
                table: "Posts",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Runners_RunnerId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Comments_CommentId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_PostId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Runners_RunnerId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Like");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "Friend");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_RunnerId",
                table: "Post",
                newName: "IX_Post_RunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_PostId",
                table: "Like",
                newName: "IX_Like_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_CommentId",
                table: "Like",
                newName: "IX_Like_CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_RunnerId",
                table: "Friend",
                newName: "IX_Friend_RunnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PostId",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "LikeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friend",
                table: "Friend",
                column: "FriendId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friend_Runners_RunnerId",
                table: "Friend",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Comment_CommentId",
                table: "Like",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Post_PostId",
                table: "Like",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Runners_RunnerId",
                table: "Post",
                column: "RunnerId",
                principalTable: "Runners",
                principalColumn: "RunnerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
