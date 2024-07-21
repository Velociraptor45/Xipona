using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Items
{
    /// <inheritdoc />
    public partial class ConvertPriceToDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE AvailableAts ADD Price2 VARCHAR(255);
                UPDATE AvailableAts SET Price2 = CAST(Price AS CHAR);
                ALTER TABLE AvailableAts MODIFY Price2 DECIMAL(65,30);
                ALTER TABLE AvailableAts DROP COLUMN Price;
                ALTER TABLE AvailableAts CHANGE COLUMN Price2 Price DECIMAL(65,30);
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE ItemTypeAvailableAts ADD Price2 VARCHAR(255);
                UPDATE ItemTypeAvailableAts SET Price2 = CAST(Price AS CHAR);
                ALTER TABLE ItemTypeAvailableAts MODIFY Price2 DECIMAL(65,30);
                ALTER TABLE ItemTypeAvailableAts DROP COLUMN Price;
                ALTER TABLE ItemTypeAvailableAts CHANGE COLUMN Price2 Price DECIMAL(65,30);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE AvailableAts ADD Price2 VARCHAR(255);
                UPDATE AvailableAts SET Price2 = CAST(Price AS CHAR);
                ALTER TABLE AvailableAts MODIFY Price2 float;
                ALTER TABLE AvailableAts DROP COLUMN Price;
                ALTER TABLE AvailableAts CHANGE COLUMN Price2 Price float;
                """);

            migrationBuilder.Sql(
                """
                ALTER TABLE ItemTypeAvailableAts ADD Price2 VARCHAR(255);
                UPDATE ItemTypeAvailableAts SET Price2 = CAST(Price AS CHAR);
                ALTER TABLE ItemTypeAvailableAts MODIFY Price2 float;
                ALTER TABLE ItemTypeAvailableAts DROP COLUMN Price;
                ALTER TABLE ItemTypeAvailableAts CHANGE COLUMN Price2 Price float;
                """);
        }
    }
}