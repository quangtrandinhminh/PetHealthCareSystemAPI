using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f730139-6a5c-493d-98e1-f12e033777ba", null, "Vet", "VET" },
                    { "4e9476fa-c60c-4c3b-a1ea-d2e8a250cb17", null, "Staff", "STAFF" },
                    { "68b61c20-7b7e-4471-bc4b-3e093ef0d041", null, "Customer", "CUSTOMER" },
                    { "eebe7dad-b384-4b1e-8724-a0e51dfd5375", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f730139-6a5c-493d-98e1-f12e033777ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e9476fa-c60c-4c3b-a1ea-d2e8a250cb17");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68b61c20-7b7e-4471-bc4b-3e093ef0d041");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "eebe7dad-b384-4b1e-8724-a0e51dfd5375");
        }
    }
}
