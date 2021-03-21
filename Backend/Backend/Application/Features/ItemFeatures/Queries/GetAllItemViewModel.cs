using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetAllItemViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ReceiveAddress { get; set; }
        public DateTime PostTime { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
