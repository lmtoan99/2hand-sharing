﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Username{get;set;}
        public string Password{get;set;}
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Message> MessageSends { get; set; }
        public virtual ICollection<Message> MessageReceives { get; set; }
        public virtual ICollection<ReportAccount> ReportSends { get; set; }
        public virtual ICollection<ReportAccount> ReportReceives { get; set; }
        public virtual ICollection<ReceiveItemInformation> ReceiveItemInformations { get; set; }
        public virtual ICollection<ItemReport> ItemReports { get; set; }
        public virtual ICollection<Assignment> AdminAssigns { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GroupPost> GroupPosts { get; set; }
        public virtual ICollection<GroupAdminDetail> GroupAdminDetails { get; set; }
        public virtual ICollection<GroupMemberDetail> GroupMemberDetails { get; set; }
        public virtual ICollection<Award> Awards { get; set; }
        public virtual ICollection<Item> DonateItems { get; set; }
    }
}
