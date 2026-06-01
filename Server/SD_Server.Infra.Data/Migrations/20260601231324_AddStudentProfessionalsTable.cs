using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_Server.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentProfessionalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_StudentProfessionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    LinkedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_StudentProfessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_StudentProfessionals_tb_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "tb_Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_StudentProfessionals_tb_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tb_Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_StudentProfessionals_ProfessionalId",
                table: "tb_StudentProfessionals",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_StudentProfessionals_StudentId",
                table: "tb_StudentProfessionals",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_StudentProfessionals");
        }
    }
}
