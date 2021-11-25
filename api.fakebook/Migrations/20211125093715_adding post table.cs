using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.fakebook.Migrations
{
    public partial class addingposttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    postDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postedById = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_postedById",
                        column: x => x.postedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_postedById",
                table: "Posts",
                column: "postedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
