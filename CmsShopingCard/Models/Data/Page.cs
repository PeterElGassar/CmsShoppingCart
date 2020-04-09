using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CmsShopingCard.Models.Data
{
    //[Table("tblPages")]
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Sulg { get; set; }
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }

    }
}