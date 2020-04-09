using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Account
{
    public class UserVM
    {
        public UserVM()
        {

        }
        public UserVM(User row)
        {
            Id = row.UserId;
            FirstName = row.FirstName;
            LastName = row.LastName;
            Email = row.Email;
            UserName = row.UserName;
            Password = row.Password;
            ActivetionCode = row.ActivetionCode;
            IsEmailVerified = row.IsEmailVerified;
        }

        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password And Confirm password Is Not Match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public System.Guid ActivetionCode { get; set; }

        public bool IsEmailVerified { get; set; }

    }
}