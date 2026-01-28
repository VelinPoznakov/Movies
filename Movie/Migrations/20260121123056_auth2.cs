using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Movie.Migrations
{
    /// <inheritdoc />
    public partial class auth2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "081922ff-ef6a-46a3-a818-88a832364b60");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0aa42cf-9001-419c-9b53-9b6729fa06f7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", "c9a2d84f-ed3c-4f28-afcc-291e7185db0a", "Admin", "ADMIN" },
                    { "2", "01ab1adf-bf30-47e8-993d-2af9f0245395", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "081922ff-ef6a-46a3-a818-88a832364b60", "a2845605-74a5-423c-b6cc-a306b164938c", "User", "USER" },
                    { "a0aa42cf-9001-419c-9b53-9b6729fa06f7", "44f2d068-d6dc-4a2a-92bb-6ea746e8744e", "Admin", "ADMIN" }
                });
        }
    }
}
