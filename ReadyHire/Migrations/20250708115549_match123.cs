using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadyHire.Migrations
{
    /// <inheritdoc />
    public partial class match123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MatchRatio",
                table: "JobApplications",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchRatio",
                table: "JobApplications");
        }
    }
}
