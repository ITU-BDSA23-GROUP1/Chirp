using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    authorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.authorID);
                });

            migrationBuilder.CreateTable(
                name: "cheeps",
                columns: table => new
                {
                    cheepID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    authorID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cheeps", x => x.cheepID);
                    table.ForeignKey(
                        name: "FK_cheeps_authors_authorID",
                        column: x => x.authorID,
                        principalTable: "authors",
                        principalColumn: "authorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cheeps_authorID",
                table: "cheeps",
                column: "authorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cheeps");

            migrationBuilder.DropTable(
                name: "authors");
        }
    }
}
