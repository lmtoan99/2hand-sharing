using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Address
{
    public class AddressDTO
    {
        public string Street { get; set; }
        public int WardId { get; set; }
        public int DistrictId { get; set; }
        public int CityId { get; set; }
    }
}
