using Microsoft.EntityFrameworkCore.Migrations;

namespace api.fakebook.Migrations
{
    public partial class addedmentions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Mentions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mentionPostId = table.Column<int>(type: "int", nullable: true),
                    mentionedUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mentions_AspNetUsers_mentionedUserId",
                        column: x => x.mentionedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mentions_Posts_mentionPostId",
                        column: x => x.mentionPostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mentions_mentionedUserId",
                table: "Mentions",
                column: "mentionedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mentions_mentionPostId",
                table: "Mentions",
                column: "mentionPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mentions");

            migrationBuilder.RenameColumn(
                name: "postDate",
                table: "Posts",
                newName: "sent");
        }
    }
}
