using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels;
using CmsShopingCard.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CmsShopingCard.Controllers
{
    public class UserController : Controller
    {
        Db db;

        public UserController()
        {
            db = new Db();
        }

        // GET: User/Registration
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }


        // POST: User/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserVM model)
        {
            bool status = false;
            string message = "";

            //Check Model State
            if (!ModelState.IsValid)
            {
                message = "Invalid Requst";
                return View(model);
            }

            #region Check Model Email~~~~~~~~
            var isExist = IsEmailExist(model.Email);

            if (isExist)
            {
                ModelState.AddModelError("EmailExist", "Email Is Already Exist");
            }
            #endregion
            //Generate Activetion Code
            model.ActivetionCode = Guid.NewGuid();

            //Password Hashing
            model.Password = Crypto.Hash(model.Password);
            model.ConfirmPassword = Crypto.Hash(model.ConfirmPassword);//Haching confirm Because dbcontext Validate agein with password
            ////////////////

            model.IsEmailVerified = false;
            ////Save In DataBase

            User UserInDb = new User(model);
            db.Users.Add(UserInDb);
            db.SaveChanges();

            //Add To User_RolesDto               
            UserRole UserRolesDto = new UserRole()
            {
                RoleId = 2,
                UserId = UserInDb.UserId

            };
            db.UserRoles.Add(UserRolesDto);

            db.SaveChanges();
            ///Send Email to User
            //SendVerificationLinkEmail(UserInDb.Email, UserInDb.ActivetionCode.ToString());

            //message = "Registration Successfully done.Account activation link has been send to your Email" +
            //    model.Email;
            message = "Registration Successfully";
            status = true;


            ViewBag.Message = message;
            ViewBag.Status = status;

            return View();
        }

        //GET: User/Verify Account
        //excute this function when user ckilcked on a link in Gmail
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;

            db.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                            // Confirm password does not match issue on save changes
            var v = db.Users.Where(a => a.ActivetionCode == new Guid(id)).FirstOrDefault();
            if (v != null)
            {
                v.IsEmailVerified = true;
                db.SaveChanges();
                Status = true;
            }
            else
            {
                ViewBag.Message = "Invalid Request";
            }

            ViewBag.Status = Status;
            return View();
        }


        // GET: User/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserVM model, string ReturnUrl = "")
        {
            string message = "";

            var UserIdDb = db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();
            if (UserIdDb != null)
            {
                if (string.Compare(Crypto.Hash(model.Password), UserIdDb.Password) == 0)
                {
                    int TimeOut = model.RememberMy ? 525600 : 20;//525600 minutes in one Year
                    var ticket = new FormsAuthenticationTicket(model.UserName, model.RememberMy, TimeOut);
                    var encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(TimeOut);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Pages");
                    }
                }
                else
                {
                    message = "invaild Password";
                }
            }
            else
            {
                message = "invaild credential providerd";
            }

            ViewBag.Message = message;
            return View();
        }


        //GET: User/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");

        }

        //GET: User/UserProfile
        [HttpGet]
        public ActionResult UserProfile()
        {
            string userName = User.Identity.Name;
            //Declare VM Model
            UserProfileVM model;


            var UserInDb = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
            //Init VM model
            model = new UserProfileVM(UserInDb);

            return View(model);
        }
        //POST: User/UserProfile
        [HttpPost]
        public ActionResult UserProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Get username
            string username = User.Identity.Name;
            // Make sure username is unique
            if (db.Users.Where(x => x.UserId != model.Id).Any(x => x.UserName == model.UserName))
            {
                ModelState.AddModelError("UserNameErorr", "UserName " + model.UserName + "Is Already Exist.");
                return View(model);
            }
            // Make sure Email is unique
            if (IsEmailExist(model.EmailAddress, model.Id))
            {
                ModelState.AddModelError("EmailExist", "Email Address Is Already Exist.");
                return View(model);
            }

            var UserInDb = db.Users.Find(model.Id);

            //make sure if user Enterd New Password or not
            if (!string.IsNullOrWhiteSpace(model.Password) && Crypto.Hash(model.CurrentPassword) != UserInDb.Password)
            {
                ModelState.AddModelError("CurrentPasswordErorr", "The Current Password Is Not Vaild..!!");
                return View(model);
            }

            UserInDb.FirstName = model.FirstName;
            UserInDb.LastName = model.LastName;
            UserInDb.UserName = model.UserName;
            UserInDb.Email = model.EmailAddress;

            //make sure if user Enterd New Password or not
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                UserInDb.Password = Crypto.Hash(model.Password);
            }

            db.SaveChanges();

            TempData["SM"] = "Profile Updated Succssec";

            return RedirectToAction("UserProfile");
        }

        ///////===============
        [NonAction]
        public bool IsEmailExist(string emailID, int? id = 0)
        {

            var CheckEmail = new User();
            bool CheckEmailById;
            if (id > 0)
            {
                return CheckEmailById = db.Users.Where(x => x.UserId != id).Any(x => x.Email == emailID);
            }
            else
            {
                CheckEmail = db.Users.Where(x => x.Email == emailID).FirstOrDefault();
            }

            return CheckEmail != null ? true : false;

        }


        //[NonAction]
        //public void SendVerificationLinkEmail(string emailID, string activationCode)
        //{
        //    var verifiUrl = "/User/VerifyAccount/" + activationCode;

        //    var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifiUrl);

        //    var fromEmail = new MailAddress("pekonagy@gmail.com", "Mini Market");
        //    var toEmail = new MailAddress(emailID);


        //    var fromEmailPassword = "1*2*3*4*5*";

        //    string subject = "Your Account is Successflly Created";

        //    string body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
        // " successfully created. Please click on the below link to verify your account" +
        // " <br/><br/><a href='" + link + "'>" + link + "</a> ";


        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
        //    };

        //    using (var message = new MailMessage(fromEmail, toEmail)
        //    {
        //        Subject = subject,
        //        Body = body,
        //        IsBodyHtml = true
        //    })

        //        smtp.Send(message);
        //}
    }
}

