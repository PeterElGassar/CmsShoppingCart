using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels.Account;
using CmsShopingCard.Models.ViewModels.Cart;
using CmsShopingCard.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CmsShopingCard.Controllers
{
    public class AccountController : Controller
    {
        Db db;
        public AccountController()
        {
            db = new Db();
        }

        // GET: account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: account/login
        public ActionResult Login()
        {
            //confirem user is not logged in
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                //return RedirectToAction("user-profile");
                return RedirectToAction("Index", "Pages");
            }
            return View();
        }

        // POST: account/login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserVM model, string ReturnUrl = "")
        {
            // Check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string message = "";
            bool IsVaild = false;

            var UserIdDb = db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

            if (UserIdDb != null && string.Compare(Crypto.Hash(model.Password), UserIdDb.Password) == 0)
            {
                IsVaild = true;
            }


            if (!IsVaild)
            {
                //ModelState.AddModelError("", "invaild credential providerd Or invaild Password");
                message = "invaild credential providerd Or invaild Password";
                ViewBag.Message = message;
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMy);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.RememberMy));
            }

        }


        // GET: account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }


        // POST: account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
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
                model.Email = "";
                model.Password = "";
                model.ConfirmPassword = "";
                return View("CreateAccount", model);
            }
            #endregion
            //Generate Activetion Code
            model.ActivetionCode = Guid.NewGuid();

            //Password Hashing
            model.Password = Crypto.Hash(model.Password);
            model.ConfirmPassword = Crypto.Hash(model.ConfirmPassword);//Haching confirm Because dbcontext Validate agein with password
            ////////////////

            model.IsEmailVerified = false;


            //Ckeck UserName Is Unique 
            if (db.Users.Where(x => x.UserId != model.Id).Any(x => x.UserName == model.UserName))
            {
                ModelState.AddModelError("UserNameExist", "User Name Is Already Exist");
                model.UserName = "";
                model.Password = "";
                model.ConfirmPassword = "";
                return View("CreateAccount", model);
            }

            //Save In DB
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

            message = "Registration Successfully You can Login Now..";
            status = true;


            ViewBag.Message = message;
            ViewBag.Status = status;

            return View("CreateAccount");
        }

        // GET: account/LogOut
        [Authorize]
        public ActionResult LogOut()
        {
            var cart = (List<CartVM>)Session["cart"];
            if (cart != null)
            {
                foreach (var product in cart)
                {
                    Product pro = db.Products.Find(product.ProductId);
                    pro.Quantity += product.Quantity;
                }
                db.SaveChanges();
            }
            FormsAuthentication.SignOut();         
            
            Session["cart"] = null;
           
            return Redirect("~/account/login");
        }


        [Authorize]
        public ActionResult UserNavPartial()
        {
            string UserName = User.Identity.Name;
            User UserInDb = new User();          

            UserInDb = db.Users.FirstOrDefault(x => x.UserName == UserName);          
            UserNavPartialVM model = new UserNavPartialVM()
            {
                FirstName = UserInDb.FirstName,
                LastName = UserInDb.LastName
            };


            //Return Partial View With Model
            return PartialView(model);
        }

        //GET: account/UserProfile
        [HttpGet]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile()
        {
            string userName = User.Identity.Name;
            //Declare VM Model

            var UserInDb = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
            //Init VM model
            UserProfileVM model = new UserProfileVM(UserInDb);

            // Return view with model
            return View("UserProfile", model);

            ////Get The current UserName
            //string username = User.Identity.Name;
            //int CurrentUserId = 0;

            //UserDTO dto = new UserDTO();

            //if (TempData["NewName"] != null)
            //{
            //    username = (string)TempData["NewName"];
            //}

            //dto = db.Users.FirstOrDefault(x => x.UserName == username);

            //if (Session["CurrentId"] != null && TempData["NewName"] == null)
            //{
            //    CurrentUserId = (int)Session["CurrentId"];
            //    dto = db.Users.FirstOrDefault(x => x.Id == CurrentUserId);
            //}
            ////UserProfileVM model;


            ////Inti the model
            //var model = new UserProfileVM(dto);

            //return View("UserProfile", model);
        }

        //استخدم اليوزر نيم للفحص فقط انو مميز البقاي استخدم ID
        [HttpPost]
        [ActionName("user-profile")]
        [Authorize]
        public ActionResult UserProfile(UserProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile",model);
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
                return View("UserProfile",model);
            }

            UserInDb.FirstName = model.FirstName;
            UserInDb.LastName = model.LastName;
            UserInDb.Email = model.EmailAddress;

            //make sure if user Enterd New Password or not
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                UserInDb.Password = Crypto.Hash(model.Password);
            }

            db.SaveChanges();

            TempData["SM"] = "Profile Updated Succssec";

            return Redirect("~/account/user-profile");

        }



        // GET: account/Orders
        [Authorize(Roles = "User")]
        public ActionResult Orders()
        {
            //Initial List of OrdersForUserVM
            List<OrdersForUserVM> OrdersForUser = new List<OrdersForUserVM>();

            //Get User ID
            User userInDb = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

            int userID = userInDb.UserId;

            //Initial List Of OrderVM
            List<OrderVM> orders = db.Orders.Where(x => x.UserId == userID).ToArray()
                .Select(x => new OrderVM(x)).ToList();

            /////////
            //loop through List Of OrderVM
            foreach (var order in orders)
            {
                //Initial Products And Qty Dic
                Dictionary<string, int> productAndQty = new Dictionary<string, int>();
                //total 
                decimal total = 0m;
                //Initial List Of Orders Details 
                List<OrderDetails> OrdersDetailsDTO = db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                //Loop though list of OrderetilasDTO
                foreach (var orderDetails in OrdersDetailsDTO)
                {
                    //  GET Product Price 
                    Product product = db.Products.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();
                    //  GET Product Price 
                    decimal price = product.Price;
                    //  GET Product Name 
                    string productName = product.Name;
                    //Add To Product Dictionary
                    productAndQty.Add(productName, orderDetails.Quantity);
                    //Get Total 
                    total += orderDetails.Quantity * price;
                }
                //Add to OrdersForUser
                OrdersForUser.Add(new OrdersForUserVM()
                {
                    OrderNumber = order.OrderId,
                    Total = total,
                    ProductAndQuantity = productAndQty,
                    CreatedAt = order.CreateAt
                });
            }




            return View(OrdersForUser);
        }

        //+++++++++++++++++++++++++++
        public bool IsEmailExist(string emailID, int? id = 0)
        {
            var CheckEmail = new User();

            bool CheckEmailById;
            if (id > 0)
            {
                return CheckEmailById = db.Users
                    .Where(x => x.UserId != id)
                    .Any(x => x.Email == emailID);
            }
            else
            {
                CheckEmail = db.Users.Where(x => x.Email == emailID).FirstOrDefault();
            }

            return CheckEmail != null ? true : false;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}





//string message = "";
//            using (Db db = new Db())
//            {
//                var UserIdDb = db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

//                if (UserIdDb != null)
//                {
//                    if (string.Compare(Crypto.Hash(model.Password), UserIdDb.Password) == 0)
//                    {
//                        int TimeOut = model.RememberMy ? 525600 : 20;//525600 minutes in one Year
//var ticket = new FormsAuthenticationTicket(model.UserName, model.RememberMy, TimeOut);
//var encrypted = FormsAuthentication.Encrypt(ticket);
//var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);

//cookie.Expires = DateTime.Now.AddMinutes(TimeOut);
//                        cookie.HttpOnly = true;
//                        Response.Cookies.Add(cookie);
//                        if (Url.IsLocalUrl(ReturnUrl))
//                        {
//                            return Redirect(ReturnUrl);
//                        }
//                        else
//                        {
//                            return RedirectToAction("Index", "Pages");
//                        }
//                    }
//                    else
//                    {
//                        message = "invaild Password";
//                    }
//                }
//                else
//                {
//                    message = "invaild credential providerd";
//                }
//            }
//            ViewBag.Message = message;
//            return View();