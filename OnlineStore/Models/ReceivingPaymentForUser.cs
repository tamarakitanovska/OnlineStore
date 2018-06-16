using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class ReceivingPaymentForUser
    {
        public String AddressForReceiving { get; set; }

        public int UserId { get; set; }

        public String Label { get; set; }

        public String UserAddress { get; set; }
    }
}