using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Areas.Admin.ViewModels.Brand
{
    public class CheckBoxVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }

        public bool Checked { get; set; }
    }
}