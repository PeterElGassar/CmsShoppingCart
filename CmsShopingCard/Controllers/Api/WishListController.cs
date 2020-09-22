using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CmsShopingCard.Models.Data;
using CmsShopingCard.Models.Dtos;

namespace CmsShopingCard.Controllers.Api
{
    public class WishListController : ApiController
    {
        private Db _context;
        public WishListController()
        {
            _context = new Db();
        }


        [HttpPost]
        public IHttpActionResult AddToWishlist([FromBody]int id)
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return BadRequest("Please Login To add this product");
            }

            string username = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(x => x.UserName == username);


            if (user != null)
            {
                var wishList = new WishList()
                {
                    productId = id,
                    UserId = user.UserId

                };
                _context.WishLists.Add(wishList);
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete]
        public IHttpActionResult RemoveFromWishLIst([FromBody]int id)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return BadRequest("Please Login To add this product");
            }
            //Current Login User
            var userName = HttpContext.Current.User.Identity.Name;
            int userId = _context.Users
                .FirstOrDefault(u => u.UserName == userName).UserId;


            var prodect = _context.WishLists
                .FirstOrDefault(w => w.productId == id && w.UserId == userId);

            if (prodect != null)
            {
                _context.WishLists.Remove(prodect);
                _context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        public IHttpActionResult GetWishListProducts()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.Current.User.Identity.Name;
                var userId = _context.Users.FirstOrDefault(u => u.UserName == userName).UserId;

                var wishListPro = _context.WishLists
                    .Where(w => w.UserId == userId)
                    .Select(dto => new WishlistDto
                    {
                        ProductName = dto.Product.Name,
                        CategoryName = dto.Product.CategoryName,
                        Id = dto.productId,
                        Price = dto.Product.Price,
                        Image = dto.Product.ImageName,
                        Slug = dto.Product.Slug,
                        BrandName = dto.Product.Brand.Name
                    });


                return Json(new { wishListProducts = wishListPro });
            }
            return Json(new { status = true });
        }
    }
}
