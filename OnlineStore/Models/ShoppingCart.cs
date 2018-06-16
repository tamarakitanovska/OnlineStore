using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace OnlineStore.Models
{
    public class ShoppingCart
    {
        //Whoose shoping cart it is

        [Key]
        public String ID { get; set; }
        public String UserID { get; set; }

        public List<Product.Product> ChoosedProducts { get; set; }

        public bool Paid { get; set; }

        public ShoppingCart()
        {
            Paid = false;
            ChoosedProducts = new List<Product.Product>();
        }

        //in dollars
        public double toBePaid()
        {
            double sum = 0;
            foreach (var item in ChoosedProducts)
            {
                sum += item.Price;
            }
            return sum;
        }
    }
}