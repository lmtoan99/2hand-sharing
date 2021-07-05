using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddFulltextIndexOnEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE Events ADD FULLTEXT Index_Events(EventName) WITH PARSER NGRAM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE Events DROP INDEX Index_Events", suppressTransaction: true);
        }
    }
}
