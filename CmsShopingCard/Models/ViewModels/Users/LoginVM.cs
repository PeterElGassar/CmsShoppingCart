using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Users
{
    public class LoginVM
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }


        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }

    }
}