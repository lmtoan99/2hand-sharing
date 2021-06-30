using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddSearchFullTextIndexOnItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "ALTER TABLE Items ADD FULLTEXT Index_Items(ItemName) WITH PARSER NGRAM", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(sql: "DROP INDEX ON Items(ItemName) KEY INDEX Index_Items;", suppressTransaction: true);
        }
    }
}
