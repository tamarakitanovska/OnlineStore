using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class StatusOfTransaction
    {
        public String AddressForPayment { get; set; }

        public BallanceOfAddress Balance { get; set; }

        public bool BitcoinsSent { get; set; }
        public int BlockConfirmations { get; set; }
    }
}