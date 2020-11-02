using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Models.ViewModels.Shop
{
    public class ProductVM
    {
        
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Product Name")]
        public string Name { get; set; }

        [Display(Name = "Product Slug")]
        public string Slug { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string CategoryName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int? BrandId { get; set; }

        public bool InWishlist { get; set; }

        public string ImageName { get; set; }

        //To make Drop Down List Of Category
        public IEnumerable<SelectListItem> Categories { get; set; }

        //To make Drop Down List Of Brands
        public IEnumerable<Brand> Brands { get; set; }

        public IEnumerable<string> GalleryImages { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public virtual ILookup<int,WishList>  WishLists { get; set; }


        public ProductVM()
        {

        }
        public ProductVM(Product row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            Quantity = row.Quantity;
            CategoryName = row.CategoryName;
            CategoryId = row.CategoryId;
            ImageName = row.ImageName;
            Category = row.Category;
            WishLists = row.UserWishLists;
            if (row.BrandId > 0)
                BrandId = row.BrandId; Brand = row.Brand;

          
        }
    }
}