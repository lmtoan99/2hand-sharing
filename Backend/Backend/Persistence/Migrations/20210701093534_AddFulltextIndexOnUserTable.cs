using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddFulltextIndexOnUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE Users ADD FULLTEXT Index_Users(FullName, PhoneNumber) WITH PARSER NGRAM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE Users DROP INDEX Index_Users", suppressTransaction: true);
        }
    }
}
