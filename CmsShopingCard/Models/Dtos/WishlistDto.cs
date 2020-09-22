using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Dtos
{
    public class WishlistDto
    {
        public int Id { get; set; }
        public string Image { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string Slug { get; set; }
        public string BrandName { get; set; }
    }
}