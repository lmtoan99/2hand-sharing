using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string AccountId { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public int? AvatarId { get; set; }
        [ForeignKey("AvatarId")]
        [InverseProperty("UserAvatar")]
        public virtual Image Avatar { get; set; }
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        [InverseProperty("UserAddress")]
        public virtual Address Address { get; set; }
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
