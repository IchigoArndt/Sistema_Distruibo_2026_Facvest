using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SD_Server.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionalExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "tb_Professionals",
                type: "VARCHAR(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Methodology",
                table: "tb_Professionals",
                type: "VARCHAR(100)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "tb_Professionals",
                type: "DECIMAL(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "tb_Professionals",
                type: "VARCHAR(100)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "tb_Professionals");

            migrationBuilder.DropColumn(
                name: "Methodology",
                table: "tb_Professionals");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "tb_Professionals");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "tb_Professionals");
        }
    }
}
