using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models.Product
{
    public class ModelForShoppingCartDetails
    {
        public ShoppingCart ShoppingCart { get; set; }
        public Product product { get; set; }

    }
}