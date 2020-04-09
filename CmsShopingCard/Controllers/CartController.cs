using CmsShopingCard.Models;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CmsShopingCard.Controllers
{
    public class CartController : Controller
    {
        Db db;
        public CartController()
        {
            db = new Db();
        }
        // GET: Cart
        public ActionResult Index()
        {
            //Init The Cart
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //Ceck If Cart is Empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your Cart is Empty..";
                return View();
            }

            //Calculate the total
            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;

            }
            ViewBag.GrandTotal = total;
            return View(cart);
        }

        public ActionResult CardPartial()
        {
            var model = new CartVM();
            int Qty = 0;
            decimal price = 0m;
            //Check for cart Session
            if (Session["cart"] != null)
            {
                //Get Total Qty and Price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    Qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }
                model.Quantity = Qty;
                model.Price = price;
            }
            else
            {
                //or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0;
            }
            return PartialView(model);
        }

        // GET: Cart
        public ActionResult AddToCartPartial(int id, int? Qyt)
        {
            //Init CartVM Listttt
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();


            //GET the product
            Product product = db.Products.Find(id);
            //Init CartVM
            var model = new CartVM(product);
            //ckeck if product is alredy in the cart
            //هنا علشان تتأكد ما اذا كان هذا المنتج موجود بالفعل من قبل او لا
            var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

            //If Not ,Add new
            if (productInCart == null)
            {
                if (Qyt > product.Quantity)
                {
                    TempData["SM"] = "Your Quantity Not Available Now in Stoce..";
                    return RedirectToAction("product-details", "Shop", model);
                }
                else if (Qyt > 1)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = (int)Qyt,
                        Image = product.ImageName
                    });
                    product.Quantity -= (int)Qyt;
                    db.SaveChanges();
                }
                else
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1,
                        Image = product.ImageName
                    });
                    product.Quantity -= 1;
                    db.SaveChanges();
                }

            }
            else
            {
                if (Qyt > 1)
                {
                    productInCart.Quantity += (int)Qyt;
                    product.Quantity -= (int)Qyt;
                }
                else
                {
                    //if it is ,increment
                    productInCart.Quantity++;
                    product.Quantity -= 1;
                    db.SaveChanges();
                }
            }
            //GET Total qty and price and add to cart
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }

            model.Quantity = qty;
            model.Price = price;
            //SAVE cart back to sesstion
            Session["cart"] = cart;

            return PartialView(model);
        }

        // GET: /Cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            // Init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;


            // Get a specific product form Cart
            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);
            //Decrement Product Quantity from database
            Product ProDuctInDb = db.Products.Find(productId);
            var message = "";

            if (ProDuctInDb.Quantity < 1)
            {
                message = "Sorry,We Dont Have More Of This Item In the Stoce.";
            }
            else
            {
                // Decrement qty From DB
                ProDuctInDb.Quantity--;
                db.SaveChanges();
                // Increment qty For View
                model.Quantity++;
            }

            // Store needed data
            var result = new { qty = model.Quantity, price = model.Price, ms = message };

            // Return json with data
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        // GET: /Cart/DecrementProduct
        public JsonResult DecrementProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;


            // Get cartVM from list
            var model = cart.FirstOrDefault(x => x.ProductId == productId);
            //Object for IncrementProduct from database
            Product ProDuctInDb = db.Products.Find(productId);
            // Decrement qty
            if (model.Quantity > 1)
            {
                model.Quantity--;
                ProDuctInDb.Quantity++;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
                ProDuctInDb.Quantity++;
            }
            db.SaveChanges();

            // Store Needed data In Json Object
            var result = new { qty = model.Quantity, price = model.Price };

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public void RemoveProduct(int productId)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            var model = cart.FirstOrDefault(x => x.ProductId == productId);

            Product ProDuctInDb = db.Products.Find(productId);
            ProDuctInDb.Quantity += model.Quantity;
            db.SaveChanges();

            cart.Remove(model);
        }

        public ActionResult PaypalPartial()
        {
            List<CartVM> cart = (List<CartVM>)Session["cart"];

            return PartialView(cart);
        }


        [HttpPost]
        public void PlaceOrder()
        {
            List<CartVM> cart = (List<CartVM>)Session["cart"];

            string username = User.Identity.Name;

            int orderId = 0;


            Order OrderDTO = new Order();

            //1- get user id
            User Query = db.Users.FirstOrDefault(x => x.UserName == username);
            int userId = Query.UserId;
            //2-Add to OrdreDto and save
            OrderDTO.UserId = userId;
            OrderDTO.CreatedAt = DateTime.Now;

            db.Orders.Add(OrderDTO);
            db.SaveChanges();
            //Get Inserted Order id After Genration iN DataBase
            orderId = OrderDTO.OrderId;
            //Initial OrderDetails  DTO
            OrderDetails OrderDetailsDTO = new OrderDetails();

            foreach (var item in cart)
            {

                OrderDetailsDTO.OrderId = orderId;
                OrderDetailsDTO.UserId = userId;
                OrderDetailsDTO.ProductId = item.ProductId;
                //End OF Three Foregin Keys Of Table OrderDetails
                OrderDetailsDTO.Quantity = item.Quantity;
                //Save Every Single Product
                db.OrderDetails.Add(OrderDetailsDTO);
                db.SaveChanges();
            }

            //email Admin
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("407c320260fefc", "49bd37d59fb009"),
                EnableSsl = true
            };
            client.Send("Admin@example.com", "Aamin@example.com", "New Order", "You Have A new Order. Order Number:" + orderId);


            //Finally Reste The Session (;
            Session["cart"] = null;
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