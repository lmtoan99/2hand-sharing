using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Address = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Rules = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Awards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DonateTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Awards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SendDate = table.Column<int>(type: "int", nullable: false),
                    SendFromAccountId = table.Column<int>(type: "int", nullable: false),
                    SendToAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Accounts_SendFromAccountId",
                        column: x => x.SendFromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Accounts_SendToAccountId",
                        column: x => x.SendToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ReportFromAccountId = table.Column<int>(type: "int", nullable: false),
                    ReportToAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAccounts_Accounts_ReportFromAccountId",
                        column: x => x.ReportFromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportAccounts_Accounts_ReportToAccountId",
                        column: x => x.ReportToAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ReceiveAddress = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DonateAccountId = table.Column<int>(type: "int", nullable: false),
                    PostTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Accounts_DonateAccountId",
                        column: x => x.DonateAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupAdminDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    AppointDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAdminDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupAdminDetails_Accounts_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAdminDetails_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMemberDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ReportStatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberDetails_Accounts_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberDetails_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PostTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    PostByAccountId = table.Column<int>(type: "int", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupPosts_Accounts_PostByAccountId",
                        column: x => x.PostByAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPosts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonatePostInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonatePostInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonatePostInformation_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemImageRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemImageRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemImageRelationships_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemImageRelationships_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReportFromAccountId = table.Column<int>(type: "int", nullable: false),
                    ReportToItemId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemReports_Accounts_ReportFromAccountId",
                        column: x => x.ReportFromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemReports_Items_ReportToItemId",
                        column: x => x.ReportToItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonateEventInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonateEventInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonateEventInformation_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonateEventInformation_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PostByAccontId = table.Column<int>(type: "int", nullable: false),
                    PostTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    PostByAccountId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Accounts_PostByAccountId",
                        column: x => x.PostByAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_GroupPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "GroupPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPostImageRelationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImageId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPostImageRelationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupPostImageRelationships_GroupPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "GroupPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPostImageRelationships_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveItemInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceiveStatus = table.Column<int>(type: "int", nullable: false),
                    Thanks = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ReceiveReason = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DonatePostInformationId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveItemInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiveItemInformation_Accounts_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveItemInformation_DonatePostInformation_DonatePostInfor~",
                        column: x => x.DonatePostInformationId,
                        principalTable: "DonatePostInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssignmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DonateEventInformationId = table.Column<int>(type: "int", nullable: false),
                    AssignedMemberId = table.Column<int>(type: "int", nullable: false),
                    AssignByAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Accounts_AssignByAccountId",
                        column: x => x.AssignByAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Accounts_AssignedMemberId",
                        column: x => x.AssignedMemberId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_DonateEventInformation_DonateEventInformationId",
                        column: x => x.DonateEventInformationId,
                        principalTable: "DonateEventInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignByAccountId",
                table: "Assignments",
                column: "AssignByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_AssignedMemberId",
                table: "Assignments",
                column: "AssignedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DonateEventInformationId",
                table: "Assignments",
                column: "DonateEventInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Awards_AccountId",
                table: "Awards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostByAccountId",
                table: "Comments",
                column: "PostByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_DonateEventInformation_EventId",
                table: "DonateEventInformation",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_DonateEventInformation_ItemId",
                table: "DonateEventInformation",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonatePostInformation_ItemId",
                table: "DonatePostInformation",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_GroupId",
                table: "Events",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAdminDetails_AdminId",
                table: "GroupAdminDetails",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAdminDetails_GroupId",
                table: "GroupAdminDetails",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberDetails_GroupId",
                table: "GroupMemberDetails",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberDetails_MemberId",
                table: "GroupMemberDetails",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostImageRelationships_ImageId",
                table: "GroupPostImageRelationships",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupPostImageRelationships_PostId",
                table: "GroupPostImageRelationships",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_GroupId",
                table: "GroupPosts",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPosts_PostByAccountId",
                table: "GroupPosts",
                column: "PostByAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemImageRelationships_ImageId",
                table: "ItemImageRelationships",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemImageRelationships_ItemId",
                table: "ItemImageRelationships",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReports_ReportFromAccountId",
                table: "ItemReports",
                column: "ReportFromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemReports_ReportToItemId",
                table: "ItemReports",
                column: "ReportToItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_DonateAccountId",
                table: "Items",
                column: "DonateAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendFromAccountId",
                table: "Messages",
                column: "SendFromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SendToAccountId",
                table: "Messages",
                column: "SendToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveItemInformation_DonatePostInformationId",
                table: "ReceiveItemInformation",
                column: "DonatePostInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveItemInformation_ReceiverId",
                table: "ReceiveItemInformation",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAccounts_ReportFromAccountId",
                table: "ReportAccounts",
                column: "ReportFromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAccounts_ReportToAccountId",
                table: "ReportAccounts",
                column: "ReportToAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Awards");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GroupAdminDetails");

            migrationBuilder.DropTable(
                name: "GroupMemberDetails");

            migrationBuilder.DropTable(
                name: "GroupPostImageRelationships");

            migrationBuilder.DropTable(
                name: "ItemImageRelationships");

            migrationBuilder.DropTable(
                name: "ItemReports");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ReceiveItemInformation");

            migrationBuilder.DropTable(
                name: "ReportAccounts");

            migrationBuilder.DropTable(
                name: "DonateEventInformation");

            migrationBuilder.DropTable(
                name: "GroupPosts");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "DonatePostInformation");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
