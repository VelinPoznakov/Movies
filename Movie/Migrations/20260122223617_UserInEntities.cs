using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Migrations
{
    /// <inheritdoc />
    public partial class UserInEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Movies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Genres",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Directors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_UserId",
                table: "Genres",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Directors_UserId",
                table: "Directors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Directors_AspNetUsers_UserId",
                table: "Directors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_AspNetUsers_UserId",
                table: "Genres",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directors_AspNetUsers_UserId",
                table: "Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Genres_AspNetUsers_UserId",
                table: "Genres");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_UserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_UserId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Genres_UserId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Directors_UserId",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "AspNetUsers");
        }
    }
}
