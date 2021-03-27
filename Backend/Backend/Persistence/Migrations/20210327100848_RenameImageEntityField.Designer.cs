﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Context;

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210327100848_RenameImageEntityField")]
    partial class RenameImageEntityField
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Domain.Entities.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AssignByAccountId")
                        .HasColumnType("int");

                    b.Property<int>("AssignedMemberId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignmentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DonateEventInformationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssignByAccountId");

                    b.HasIndex("AssignedMemberId");

                    b.HasIndex("DonateEventInformationId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("Domain.Entities.Award", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DonateTime")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Awards");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PostByAccontId")
                        .HasColumnType("int");

                    b.Property<int?>("PostByAccountId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PostTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PostByAccountId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.Entities.DonateEventInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("ItemId")
                        .IsUnique();

                    b.ToTable("DonateEventInformation");
                });

            modelBuilder.Entity("Domain.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EventName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Domain.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("GroupName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Rules")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Domain.Entities.GroupAdminDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AppointDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupAdminDetails");
                });

            modelBuilder.Entity("Domain.Entities.GroupMemberDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<bool>("ReportStatus")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("MemberId");

                    b.ToTable("GroupMemberDetails");
                });

            modelBuilder.Entity("Domain.Entities.GroupPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("PostByAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PostTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("PostByAccountId");

                    b.ToTable("GroupPosts");
                });

            modelBuilder.Entity("Domain.Entities.GroupPostImageRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("PostId");

                    b.ToTable("GroupPostImageRelationships");
                });

            modelBuilder.Entity("Domain.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("DonateAccountId")
                        .HasColumnType("int");

                    b.Property<int>("DonateType")
                        .HasColumnType("int");

                    b.Property<string>("ItemName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("PostTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReceiveAddress")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DonateAccountId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Domain.Entities.ItemImageRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageId")
                        .IsUnique();

                    b.HasIndex("ItemId");

                    b.ToTable("ItemImageRelationships");
                });

            modelBuilder.Entity("Domain.Entities.ItemReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ReportFromAccountId")
                        .HasColumnType("int");

                    b.Property<int>("ReportToItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportFromAccountId");

                    b.HasIndex("ReportToItemId");

                    b.ToTable("ItemReports");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SendDate")
                        .HasColumnType("int");

                    b.Property<int>("SendFromAccountId")
                        .HasColumnType("int");

                    b.Property<int>("SendToAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SendFromAccountId");

                    b.HasIndex("SendToAccountId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Domain.Entities.ReceiveItemInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("ReceiveReason")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ReceiveStatus")
                        .HasColumnType("int");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<string>("Thanks")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("ReceiveItemInformation");
                });

            modelBuilder.Entity("Domain.Entities.ReportAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ReportFromAccountId")
                        .HasColumnType("int");

                    b.Property<int>("ReportToAccountId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportFromAccountId");

                    b.HasIndex("ReportToAccountId");

                    b.ToTable("ReportAccounts");
                });

            modelBuilder.Entity("Domain.Entities.Assignment", b =>
                {
                    b.HasOne("Domain.Entities.Account", "AssignByAccount")
                        .WithMany("AdminAssigns")
                        .HasForeignKey("AssignByAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "AssignedMember")
                        .WithMany("Assignments")
                        .HasForeignKey("AssignedMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.DonateEventInformation", "DonateEventInformation")
                        .WithMany("Assignments")
                        .HasForeignKey("DonateEventInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignByAccount");

                    b.Navigation("AssignedMember");

                    b.Navigation("DonateEventInformation");
                });

            modelBuilder.Entity("Domain.Entities.Award", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Account")
                        .WithMany("Awards")
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.Account", "PostByAccount")
                        .WithMany("Comments")
                        .HasForeignKey("PostByAccountId");

                    b.HasOne("Domain.Entities.GroupPost", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("PostByAccount");
                });

            modelBuilder.Entity("Domain.Entities.DonateEventInformation", b =>
                {
                    b.HasOne("Domain.Entities.Event", "Event")
                        .WithMany("DonateEventInformations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Item", "Item")
                        .WithOne("DonateEventInformation")
                        .HasForeignKey("Domain.Entities.DonateEventInformation", "ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Domain.Entities.Event", b =>
                {
                    b.HasOne("Domain.Entities.Group", "Group")
                        .WithMany("Events")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Entities.GroupAdminDetail", b =>
                {
                    b.HasOne("Domain.Entities.Account", "Admin")
                        .WithMany("GroupAdminDetails")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Group", "Group")
                        .WithMany("GroupAdminDetails")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Domain.Entities.GroupMemberDetail", b =>
                {
                    b.HasOne("Domain.Entities.Group", "Group")
                        .WithMany("GroupMemberDetails")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "Member")
                        .WithMany("GroupMemberDetails")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("Domain.Entities.GroupPost", b =>
                {
                    b.HasOne("Domain.Entities.Group", "Group")
                        .WithMany("GroupPosts")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "PostByAccount")
                        .WithMany("GroupPosts")
                        .HasForeignKey("PostByAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("PostByAccount");
                });

            modelBuilder.Entity("Domain.Entities.GroupPostImageRelationship", b =>
                {
                    b.HasOne("Domain.Entities.Image", "Image")
                        .WithOne("GroupPostImageRelationship")
                        .HasForeignKey("Domain.Entities.GroupPostImageRelationship", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.GroupPost", "Post")
                        .WithMany("GroupPostImageRelationships")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "DonateAccount")
                        .WithMany("DonateItems")
                        .HasForeignKey("DonateAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("DonateAccount");
                });

            modelBuilder.Entity("Domain.Entities.ItemImageRelationship", b =>
                {
                    b.HasOne("Domain.Entities.Image", "Image")
                        .WithOne("ItemImageRelationship")
                        .HasForeignKey("Domain.Entities.ItemImageRelationship", "ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Item", "Item")
                        .WithMany("ItemImageRelationships")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Domain.Entities.ItemReport", b =>
                {
                    b.HasOne("Domain.Entities.Account", "ReportFromAccount")
                        .WithMany("ItemReports")
                        .HasForeignKey("ReportFromAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Item", "ReportToItem")
                        .WithMany("ItemReports")
                        .HasForeignKey("ReportToItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportFromAccount");

                    b.Navigation("ReportToItem");
                });

            modelBuilder.Entity("Domain.Entities.Message", b =>
                {
                    b.HasOne("Domain.Entities.Account", "SendFromAccount")
                        .WithMany("MessageSends")
                        .HasForeignKey("SendFromAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "SendToAccount")
                        .WithMany("MessageReceives")
                        .HasForeignKey("SendToAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SendFromAccount");

                    b.Navigation("SendToAccount");
                });

            modelBuilder.Entity("Domain.Entities.ReceiveItemInformation", b =>
                {
                    b.HasOne("Domain.Entities.Item", "Items")
                        .WithMany("ReceiveItemInformations")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "Receiver")
                        .WithMany("ReceiveItemInformations")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Items");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("Domain.Entities.ReportAccount", b =>
                {
                    b.HasOne("Domain.Entities.Account", "ReportFromAccount")
                        .WithMany("ReportSends")
                        .HasForeignKey("ReportFromAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Account", "ReportToAccount")
                        .WithMany("ReportReceives")
                        .HasForeignKey("ReportToAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportFromAccount");

                    b.Navigation("ReportToAccount");
                });

            modelBuilder.Entity("Domain.Entities.Account", b =>
                {
                    b.Navigation("AdminAssigns");

                    b.Navigation("Assignments");

                    b.Navigation("Awards");

                    b.Navigation("Comments");

                    b.Navigation("DonateItems");

                    b.Navigation("GroupAdminDetails");

                    b.Navigation("GroupMemberDetails");

                    b.Navigation("GroupPosts");

                    b.Navigation("ItemReports");

                    b.Navigation("MessageReceives");

                    b.Navigation("MessageSends");

                    b.Navigation("ReceiveItemInformations");

                    b.Navigation("ReportReceives");

                    b.Navigation("ReportSends");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Entities.DonateEventInformation", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Domain.Entities.Event", b =>
                {
                    b.Navigation("DonateEventInformations");
                });

            modelBuilder.Entity("Domain.Entities.Group", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("GroupAdminDetails");

                    b.Navigation("GroupMemberDetails");

                    b.Navigation("GroupPosts");
                });

            modelBuilder.Entity("Domain.Entities.GroupPost", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("GroupPostImageRelationships");
                });

            modelBuilder.Entity("Domain.Entities.Image", b =>
                {
                    b.Navigation("GroupPostImageRelationship");

                    b.Navigation("ItemImageRelationship");
                });

            modelBuilder.Entity("Domain.Entities.Item", b =>
                {
                    b.Navigation("DonateEventInformation");

                    b.Navigation("ItemImageRelationships");

                    b.Navigation("ItemReports");

                    b.Navigation("ReceiveItemInformations");
                });
#pragma warning restore 612, 618
        }
    }
}
