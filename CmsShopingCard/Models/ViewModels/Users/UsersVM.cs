using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Users
{
    public class UsersVM
    {

        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password And Confirm password Is Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }


        public System.Guid ActivetionCode { get; set; }

        public bool IsEmailVerified { get; set; }


        public UsersVM()
        {

        }
        public UsersVM(User row)
        {
            UserId = row.UserId;
            FirstName = row.FirstName;
            LastName = row.LastName;
            UserName = row.UserName;
            Email = row.Email;
            Password = row.Password;
            ActivetionCode = row.ActivetionCode;
            IsEmailVerified = row.IsEmailVerified;

        }

       
    }
}