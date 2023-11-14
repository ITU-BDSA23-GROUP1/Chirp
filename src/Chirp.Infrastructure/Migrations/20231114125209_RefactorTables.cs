using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Cheeps",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_AuthorId",
                table: "Cheeps",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorId",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_AuthorId",
                table: "Cheeps");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "Cheeps",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                table: "Cheeps",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_AuthorId1",
                table: "Cheeps",
                column: "AuthorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_AspNetUsers_AuthorId1",
                table: "Cheeps",
                column: "AuthorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
