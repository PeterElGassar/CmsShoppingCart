using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Areas.Admin.ViewModels.Shop
{
    public class OrdersForAdmin
    {
        public int OrderNumber { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }

        public Dictionary<string, int> ProductAndQuantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}