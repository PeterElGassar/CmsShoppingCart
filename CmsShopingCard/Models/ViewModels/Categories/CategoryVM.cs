using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Categories
{
    public class CategoryVM
    {
        public bool IsAuth { get; set; }

        public ILookup<int, WishList> WishLists { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}