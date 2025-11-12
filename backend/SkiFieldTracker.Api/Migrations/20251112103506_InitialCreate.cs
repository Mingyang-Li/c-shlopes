using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkiFieldTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ski_fields",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    country = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    region = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    adult_full_day_pass_usd = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ski_fields", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ux_ski_fields_name",
                table: "ski_fields",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ski_fields");
        }
    }
}
