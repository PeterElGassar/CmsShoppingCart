using CmsShopingCard.Areas.Admin.ViewModels.Brand;
using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Shop
{
    public class CategoriesVM
    {
       
        public int Id { get; set; }
        public string Sulg { get; set; }
        public int Sorting { get; set; }
        public string Name { get; set; }
       

        public List<CheckBoxVM> Brands { get; set; }
        public CategoriesVM()
        {

        }
        public CategoriesVM(Category row)
        {
            Id = row.Id;
            Sulg = row.Slug;
            Sorting = row.Sorting;
            Name = row.Name;
        }


        //for wishList
        public bool IsAuth { get; set; }

        public ILookup<int, WishList> WishLists { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}