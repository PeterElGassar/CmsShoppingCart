using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }
        //Come After update data and Erorr Happend to get method
        public UserProfileVM(User row)
        {
            Id = row.UserId;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.Email;
            UserName = row.UserName;
        }



        //Dont Forget To Remove Required Atrr from Password Here
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}