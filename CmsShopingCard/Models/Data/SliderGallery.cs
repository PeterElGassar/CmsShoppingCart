using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Data
{
    public class SliderGallery
    {
        [Key]
        public int SliderId { get; set; }

        public string ImageName { get; set; }
        public string Title { get; set; }

        [Display(Name = "Url Link")]
        public string UrlLink { get; set; }

    }
}