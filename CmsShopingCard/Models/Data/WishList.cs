using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CmsShopingCard.Models.Data
{
    public class WishList
    {
        
        [Key]
        [Column(Order=2)]
        [Required]
        public int UserId { get; set; }


        [Key]
        [Column(Order = 1)]
        [Required]
        public int productId { get; set; }

        [ForeignKey("productId")]
        public virtual Product Product { get; set; }


        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}