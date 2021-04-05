using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RenameAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Accounts_AssignByAccountId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Accounts_AssignedMemberId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Accounts_AccountId",
                table: "Awards");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Accounts_PostByAccountId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupAdminDetails_Accounts_AdminId",
                table: "GroupAdminDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberDetails_Accounts_MemberId",
                table: "GroupMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupPosts_Accounts_PostByAccountId",
                table: "GroupPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemReports_Accounts_ReportFromAccountId",
                table: "ItemReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Accounts_DonateAccountId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Accounts_SendFromAccountId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Accounts_SendToAccountId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveItemInformation_Accounts_ReceiverId",
                table: "ReceiveItemInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAccounts_Accounts_ReportFromAccountId",
                table: "ReportAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAccounts_Accounts_ReportToAccountId",
                table: "ReportAccounts");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Address = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_AssignByAccountId",
                table: "Assignments",
                column: "AssignByAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_AssignedMemberId",
                table: "Assignments",
                column: "AssignedMemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Users_AccountId",
                table: "Awards",
                column: "AccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_PostByAccountId",
                table: "Comments",
                column: "PostByAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAdminDetails_Users_AdminId",
                table: "GroupAdminDetails",
                column: "AdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberDetails_Users_MemberId",
                table: "GroupMemberDetails",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPosts_Users_PostByAccountId",
                table: "GroupPosts",
                column: "PostByAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemReports_Users_ReportFromAccountId",
                table: "ItemReports",
                column: "ReportFromAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_DonateAccountId",
                table: "Items",
                column: "DonateAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SendFromAccountId",
                table: "Messages",
                column: "SendFromAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SendToAccountId",
                table: "Messages",
                column: "SendToAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveItemInformation_Users_ReceiverId",
                table: "ReceiveItemInformation",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAccounts_Users_ReportFromAccountId",
                table: "ReportAccounts",
                column: "ReportFromAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAccounts_Users_ReportToAccountId",
                table: "ReportAccounts",
                column: "ReportToAccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_AssignByAccountId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_AssignedMemberId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Awards_Users_AccountId",
                table: "Awards");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_PostByAccountId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupAdminDetails_Users_AdminId",
                table: "GroupAdminDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberDetails_Users_MemberId",
                table: "GroupMemberDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupPosts_Users_PostByAccountId",
                table: "GroupPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemReports_Users_ReportFromAccountId",
                table: "ItemReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_DonateAccountId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SendFromAccountId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SendToAccountId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveItemInformation_Users_ReceiverId",
                table: "ReceiveItemInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAccounts_Users_ReportFromAccountId",
                table: "ReportAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAccounts_Users_ReportToAccountId",
                table: "ReportAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Username = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Accounts_AssignByAccountId",
                table: "Assignments",
                column: "AssignByAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Accounts_AssignedMemberId",
                table: "Assignments",
                column: "AssignedMemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Awards_Accounts_AccountId",
                table: "Awards",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Accounts_PostByAccountId",
                table: "Comments",
                column: "PostByAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAdminDetails_Accounts_AdminId",
                table: "GroupAdminDetails",
                column: "AdminId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberDetails_Accounts_MemberId",
                table: "GroupMemberDetails",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupPosts_Accounts_PostByAccountId",
                table: "GroupPosts",
                column: "PostByAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemReports_Accounts_ReportFromAccountId",
                table: "ItemReports",
                column: "ReportFromAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Accounts_DonateAccountId",
                table: "Items",
                column: "DonateAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Accounts_SendFromAccountId",
                table: "Messages",
                column: "SendFromAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Accounts_SendToAccountId",
                table: "Messages",
                column: "SendToAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveItemInformation_Accounts_ReceiverId",
                table: "ReceiveItemInformation",
                column: "ReceiverId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAccounts_Accounts_ReportFromAccountId",
                table: "ReportAccounts",
                column: "ReportFromAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAccounts_Accounts_ReportToAccountId",
                table: "ReportAccounts",
                column: "ReportToAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
