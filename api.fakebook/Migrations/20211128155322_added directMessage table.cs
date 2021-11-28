using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.fakebook.Migrations
{
    public partial class addeddirectMessagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DirectMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    toId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    sent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectMessages_AspNetUsers_fromId",
                        column: x => x.fromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectMessages_AspNetUsers_toId",
                        column: x => x.toId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_fromId",
                table: "DirectMessages",
                column: "fromId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_toId",
                table: "DirectMessages",
                column: "toId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DirectMessages");
        }
    }
}
