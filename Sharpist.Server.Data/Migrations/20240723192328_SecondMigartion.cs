using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sharpist.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigartion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "HRs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "HRs");
        }
    }
}
