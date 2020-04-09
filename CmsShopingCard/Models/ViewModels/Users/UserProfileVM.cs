using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Users
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }
        public UserProfileVM(User row)
        {
            Id = row.UserId;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Email = row.Email;
            UserName = row.UserName;
            ActivetionCode = row.ActivetionCode;
            IsEmailVerified = row.IsEmailVerified;
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
        public string Email { get; set; }

        
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
        [Display(Name ="New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }


        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        public Guid ActivetionCode { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}