using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;
using CmsShopingCard.Models.ViewModels.Account;

namespace CmsShopingCard.Models.Data
{
    //[Table("tblUsers")]
    public class User
    {

        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public System.Guid ActivetionCode { get; set; }
        public bool IsEmailVerified { get; set; }

        public User()
        {

        }

        public User(UserVM row)
        {
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