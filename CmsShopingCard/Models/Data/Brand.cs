using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Data
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public string ImageName { get; set; }

        public virtual ICollection<CategoryBrand> CategoryBrand { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}