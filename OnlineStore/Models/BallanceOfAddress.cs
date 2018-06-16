using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class BallanceOfAddress
    {
        //Saldoto e vo Satoshi
        public int Balance { get; set; }



        public Double getBalanceAsUSD()
        {
            return Balance;
        }
        public Double getBalanceAsEuro()
        {
            return Balance;
        }
    }
}