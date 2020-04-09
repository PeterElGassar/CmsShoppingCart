using CmsShopingCard.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CmsShopingCard.Models.ViewModels.Cart
{
    public class CartVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get { return Quantity * Price; } }
        public string Image { get; set; }

        public CartVM()
        {

        }
        public CartVM(Product row)
        {
            ProductId = row.Id;
            ProductName = row.Name;
            Quantity = row.Quantity;
            Price = row.Price;
            Image = row.ImageName;
        }
    }
}