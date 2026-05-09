using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_Server.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAvaliationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Avaliations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    TypeAvaliation = table.Column<int>(type: "int", nullable: false),
                    StudentObjective = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BodyComposition_Weight = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_Height = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_Waist = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_Abdomen = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_Hips = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_Chest = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_ArmRight = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_ArmLeft = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_RightThigh = table.Column<double>(type: "float", nullable: true),
                    BodyComposition_LeftThigh = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Pectoral = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_MidAxillary = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Triceps = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Subscapular = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Suprailiac = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Abdominal = table.Column<double>(type: "float", nullable: true),
                    Skinfolds_Thigh = table.Column<double>(type: "float", nullable: true),
                    Anamnesis_HasInjury = table.Column<bool>(type: "bit", nullable: true),
                    Anamnesis_InjuryDescription = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Anamnesis_HasMedication = table.Column<bool>(type: "bit", nullable: true),
                    Anamnesis_MedicationDescription = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                    Anamnesis_ActivityLevel = table.Column<int>(type: "int", nullable: true),
                    TechnicalOpinion = table.Column<string>(type: "VARCHAR(MAX)", nullable: true),
                    DateNextAvaliation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IMC = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    BodyFatPercentage = table.Column<string>(type: "VARCHAR(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Avaliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_Avaliations_tb_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "tb_Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_Avaliations_tb_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Avaliations_ProfessionalId",
                table: "tb_Avaliations",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Avaliations_StudentId",
                table: "tb_Avaliations",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_Avaliations");
        }
    }
}
