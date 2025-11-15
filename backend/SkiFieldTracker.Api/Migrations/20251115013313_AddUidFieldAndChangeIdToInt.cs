using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SkiFieldTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUidFieldAndChangeIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete all existing data since we can't convert UUID to integer
            migrationBuilder.Sql("DELETE FROM ski_fields;");

            // Drop the existing primary key constraint
            migrationBuilder.DropPrimaryKey(
                name: "pk_ski_fields",
                table: "ski_fields");

            // Drop the existing unique index on name (will be recreated later if needed)
            migrationBuilder.DropIndex(
                name: "ux_ski_fields_name",
                table: "ski_fields");

            // Alter the id column from uuid to integer
            migrationBuilder.Sql(@"
                ALTER TABLE ski_fields 
                ALTER COLUMN id TYPE integer USING 0;
            ");

            // Add identity generation
            migrationBuilder.Sql(@"
                ALTER TABLE ski_fields 
                ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY;
            ");

            // Recreate primary key
            migrationBuilder.AddPrimaryKey(
                name: "pk_ski_fields",
                table: "ski_fields",
                column: "id");

            // Add uid column with database-generated default
            migrationBuilder.AddColumn<string>(
                name: "uid",
                table: "ski_fields",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValueSql: "generate_nanoid()");

            // Create unique index on uid
            migrationBuilder.CreateIndex(
                name: "ux_ski_fields_uid",
                table: "ski_fields",
                column: "uid",
                unique: true);

            // Recreate unique index on name
            migrationBuilder.CreateIndex(
                name: "ux_ski_fields_name",
                table: "ski_fields",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop indexes
            migrationBuilder.DropIndex(
                name: "ux_ski_fields_uid",
                table: "ski_fields");

            migrationBuilder.DropIndex(
                name: "ux_ski_fields_name",
                table: "ski_fields");

            // Drop primary key
            migrationBuilder.DropPrimaryKey(
                name: "pk_ski_fields",
                table: "ski_fields");

            // Drop uid column
            migrationBuilder.DropColumn(
                name: "uid",
                table: "ski_fields");

            // Drop the nanoid function
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS generate_nanoid(INTEGER);");

            // Remove identity generation
            migrationBuilder.Sql(@"
                ALTER TABLE ski_fields 
                ALTER COLUMN id DROP IDENTITY IF EXISTS;
            ");

            // Convert back to uuid
            migrationBuilder.Sql(@"
                ALTER TABLE ski_fields 
                ALTER COLUMN id TYPE uuid USING gen_random_uuid();
            ");

            // Recreate primary key
            migrationBuilder.AddPrimaryKey(
                name: "pk_ski_fields",
                table: "ski_fields",
                column: "id");

            // Recreate name index
            migrationBuilder.CreateIndex(
                name: "ux_ski_fields_name",
                table: "ski_fields",
                column: "name",
                unique: true);
        }
    }
}
