using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class UserBitcoinAdress
    {
        public int UserId { get; set; }
        public String UserAddress { get; set; }

        public UserBitcoinAdress(int userId, string userAddress)
        {
            UserId = userId;
            UserAddress = userAddress;
        }

        public UserBitcoinAdress()
        {
        }
    }
}