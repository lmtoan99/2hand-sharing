using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddJoinStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JoinStatus",
                table: "GroupMemberDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinStatus",
                table: "GroupMemberDetails");
        }
    }
}
