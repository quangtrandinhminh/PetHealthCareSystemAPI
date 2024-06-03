using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitalization_Pet_PetId",
                table: "Hospitalization");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Pet_PetId",
                table: "MedicalRecord");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecord_AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "MedicalRecord");

            migrationBuilder.RenameColumn(
                name: "PetId",
                table: "Hospitalization",
                newName: "MedicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_Hospitalization_PetId",
                table: "Hospitalization",
                newName: "IX_Hospitalization_MedicalRecordId");

            migrationBuilder.AlterColumn<string>(
                name: "PetId",
                table: "MedicalRecord",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PetWeight",
                table: "MedicalRecord",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitalization_MedicalRecord_MedicalRecordId",
                table: "Hospitalization",
                column: "MedicalRecordId",
                principalTable: "MedicalRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Pet_PetId",
                table: "MedicalRecord",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitalization_MedicalRecord_MedicalRecordId",
                table: "Hospitalization");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecord_Pet_PetId",
                table: "MedicalRecord");

            migrationBuilder.DropColumn(
                name: "PetWeight",
                table: "MedicalRecord");

            migrationBuilder.RenameColumn(
                name: "MedicalRecordId",
                table: "Hospitalization",
                newName: "PetId");

            migrationBuilder.RenameIndex(
                name: "IX_Hospitalization_MedicalRecordId",
                table: "Hospitalization",
                newName: "IX_Hospitalization_PetId");

            migrationBuilder.AlterColumn<string>(
                name: "PetId",
                table: "MedicalRecord",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppointmentId",
                table: "MedicalRecord",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecord_AppointmentId",
                table: "MedicalRecord",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitalization_Pet_PetId",
                table: "Hospitalization",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Appointment_AppointmentId",
                table: "MedicalRecord",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecord_Pet_PetId",
                table: "MedicalRecord",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }
    }
}
