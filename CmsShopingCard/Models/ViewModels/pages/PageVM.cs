using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Models.ViewModels.pages
{
    public class PageVM
    {


        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Titel { get; set; }

        public string Sulg { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }

        public int Sorting { get; set; }

        [Display(Name = "Sidebar")]
        public bool HasSidebar { get; set; }

        public virtual IEnumerable<SliderGallery> SliderGallery { get; set; }
        public PageVM()
        {

        }
        public PageVM(Page row)
        {
            Id = row.Id;
            Titel = row.Titel;
            Sulg = row.Sulg;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }
    }
}