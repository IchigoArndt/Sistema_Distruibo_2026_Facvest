using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_Server.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_Professionals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Phone = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    Cref = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    Bio = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_Professionals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Professionals_Email",
                table: "tb_Professionals",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_Professionals_Cref",
                table: "tb_Professionals",
                column: "Cref",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_Professionals");
        }
    }
}
