using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Data
{
    public class CategoryBrand
    {
        [Key]
        public int CategoryBrandId { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}