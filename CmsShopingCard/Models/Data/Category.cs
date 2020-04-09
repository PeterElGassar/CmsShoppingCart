using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Data
{
    //[Table("tblCategories")]
    public class Category
    {
        public int Id { get; set; }
        public string  Slug { get; set; }
        public int Sorting { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<CategoryBrand> CategoryBrand { get; set; }

    }
}