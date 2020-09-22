using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Models.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        [AllowHtml]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }


        public string ImageName { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }


        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }


        public virtual ICollection<WishList> WishLists { get; set; }
        public virtual ILookup<int, WishList> UserWishLists { get; set; }

    }
}