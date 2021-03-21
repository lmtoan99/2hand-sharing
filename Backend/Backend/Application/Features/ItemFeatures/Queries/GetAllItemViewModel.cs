using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ItemFeatures.Queries
{
    public class GetAllItemViewModel
    {
        public string ItemName { get; set; }
        public string ReceiveAddress { get; set; }
        public DateTime PostTime { get; set; }
        public List<Image> Images { get; set; }
    }
}
