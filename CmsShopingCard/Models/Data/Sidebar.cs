using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.Data
{
    //[Table("tblSidbar")]
    public class Sidebar
    {
        [Key]
        public int Id { get; set; }
        public string body { get; set; }
    }
}