using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKeyAppoHospUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CageHospitalization");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitalization_CageId",
                table: "Hospitalization",
                column: "CageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitalization_Cage_CageId",
                table: "Hospitalization",
                column: "CageId",
                principalTable: "Cage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitalization_Cage_CageId",
                table: "Hospitalization");

            migrationBuilder.DropIndex(
                name: "IX_Hospitalization_CageId",
                table: "Hospitalization");

            migrationBuilder.CreateTable(
                name: "CageHospitalization",
                columns: table => new
                {
                    CageId = table.Column<int>(type: "int", nullable: false),
                    CageId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CageHospitalization", x => new { x.CageId, x.CageId1 });
                    table.ForeignKey(
                        name: "FK_CageHospitalization_Cage_CageId1",
                        column: x => x.CageId1,
                        principalTable: "Cage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CageHospitalization_Hospitalization_CageId",
                        column: x => x.CageId,
                        principalTable: "Hospitalization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CageHospitalization_CageId1",
                table: "CageHospitalization",
                column: "CageId1");
        }
    }
}
