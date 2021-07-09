using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddFulltextIndexOnGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE `Groups` ADD FULLTEXT Index_Groups(GroupName) WITH PARSER NGRAM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE `Groups` DROP INDEX Index_Groups", suppressTransaction: true);
        }
    }
}
