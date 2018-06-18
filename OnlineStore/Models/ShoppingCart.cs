using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace OnlineStore.Models
{
    [Serializable]
    public class ShoppingCart
    {
        //Whoose shoping cart it is

        [Key]
        public int ID { get; set; }
        public String UserID { get; set; }

        public virtual ICollection<Product.Product> ChoosedProducts { get; set; }

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

        public int umberOfSameItems(int id)
        {
            int n = 0;
            foreach (var item in ChoosedProducts)
            {
                if (item.Id == id)
                    n++;
            }
            return n;
        }
    }
}