using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Award
{
    public class GetAwardsViewModel
    {
        public int AccountId { get; set; }
        public string DonateAccountName { get; set; }
        public string AvatarUrl { get; set; }
        public int DonateTime { get; set; }
    }
}
