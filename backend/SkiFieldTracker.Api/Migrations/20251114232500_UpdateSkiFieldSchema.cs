using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkiFieldTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSkiFieldSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country",
                table: "ski_fields");

            migrationBuilder.RenameColumn(
                name: "adult_full_day_pass_usd",
                table: "ski_fields",
                newName: "full_day_pass_price");

            migrationBuilder.AddColumn<string>(
                name: "country_code",
                table: "ski_fields",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "ski_fields",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "nearest_town",
                table: "ski_fields",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_code",
                table: "ski_fields");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "ski_fields");

            migrationBuilder.DropColumn(
                name: "nearest_town",
                table: "ski_fields");

            migrationBuilder.RenameColumn(
                name: "full_day_pass_price",
                table: "ski_fields",
                newName: "adult_full_day_pass_usd");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "ski_fields",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }
    }
}
