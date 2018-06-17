using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class UserBitcoinAdress
    {
        public String UserId { get; set; }
        public String UserAddress { get; set; }

        public int ShoppingCartId { get; set; }

        public UserBitcoinAdress(ShoppingCart shopping)
        {
            this.ShoppingCartId = shopping.ID;
            UserId = shopping.UserID;
        }

        public UserBitcoinAdress()
        {
        }
    }
}