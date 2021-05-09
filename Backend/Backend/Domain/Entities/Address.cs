using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Street { get; set; }
        public int WardId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
        public virtual ICollection<Item> DonateAddress { get; set; }
        public virtual User UserAddress { get; set; }
    }
}
