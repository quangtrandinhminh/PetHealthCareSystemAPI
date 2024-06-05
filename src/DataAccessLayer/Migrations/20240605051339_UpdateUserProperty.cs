using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedTime",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdatedTime",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6bbf6679-9b27-48fb-b431-ccdc13ad5da4", null, "Admin", "ADMIN" },
                    { "8961d879-0acb-47c1-9d86-47fa8c27fef8", null, "Customer", "CUSTOMER" },
                    { "bad06676-e635-473e-8999-53a858729692", null, "Staff", "STAFF" },
                    { "c50e79ef-4c4a-4468-a66f-96840e64944a", null, "Vet", "VET" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6bbf6679-9b27-48fb-b431-ccdc13ad5da4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8961d879-0acb-47c1-9d86-47fa8c27fef8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bad06676-e635-473e-8999-53a858729692");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c50e79ef-4c4a-4468-a66f-96840e64944a");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "AspNetUsers");

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
    }
}
