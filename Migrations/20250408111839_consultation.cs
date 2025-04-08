using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmazeCare.Migrations
{
    public partial class consultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecommendedTests",
                table: "PrescriptionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalTests",
                columns: table => new
                {
                    MedicalTestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTests", x => x.MedicalTestId);
                });

            migrationBuilder.CreateTable(
                name: "RecommendedTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicalRecordId = table.Column<int>(type: "int", nullable: false),
                    MedicalTestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendedTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendedTests_MedicalRecords_MedicalRecordId",
                        column: x => x.MedicalRecordId,
                        principalTable: "MedicalRecords",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendedTests_MedicalTests_MedicalTestId",
                        column: x => x.MedicalTestId,
                        principalTable: "MedicalTests",
                        principalColumn: "MedicalTestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedTests_MedicalRecordId",
                table: "RecommendedTests",
                column: "MedicalRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendedTests_MedicalTestId",
                table: "RecommendedTests",
                column: "MedicalTestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendedTests");

            migrationBuilder.DropTable(
                name: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "RecommendedTests",
                table: "PrescriptionDetails");
        }
    }
}
