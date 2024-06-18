using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "31fcbe78-5861-447d-91b8-78f319d31c4c", new DateTimeOffset(new DateTime(2024, 6, 18, 20, 47, 3, 564, DateTimeKind.Unspecified).AddTicks(9937), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 6, 18, 20, 47, 3, 564, DateTimeKind.Unspecified).AddTicks(9937), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$nETz./dqkTcz4/PVwqE46eljFXbq3GS95DukiIzHDJysSkdP3lwn." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedTime", "LastUpdatedTime", "PasswordHash" },
                values: new object[] { "b43c8e65-9188-4a90-af4d-06e51f3dcd9b", new DateTimeOffset(new DateTime(2024, 6, 18, 3, 26, 6, 535, DateTimeKind.Unspecified).AddTicks(5707), new TimeSpan(0, 7, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 6, 18, 3, 26, 6, 535, DateTimeKind.Unspecified).AddTicks(5707), new TimeSpan(0, 7, 0, 0, 0)), "$2a$11$vevZ.NGOVnJir0ag8fFmdeb57tNS7fM2V/H.WRKptOONkaBEIict6" });
        }
    }
}
