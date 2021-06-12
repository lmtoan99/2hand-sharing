using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class ModifyMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                table: "Messages",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SendDate",
                table: "Messages",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
