using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class ReceivingPaymentForUser
    {
        public String AddressForReceiving { get; set; }

        public String UserId { get; set; }

        public String Label { get; set; }

        public String UserAddress { get; set; }
        public String ShoppingCartId { get; set; }

        //in dollars
        public double ammountToBePaid { get; set; }

        public double ammountToBePaidBitcoin { get; set; }

        public ReceivingPaymentForUser()
        {

        }
    }
}