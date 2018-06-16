using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class AddressForCustumer
    {
        public String NewAddress { get; set; }

       
        public String Label { get; set; }

      

        public AddressForCustumer(string address, string label)
        {
            NewAddress = address;
            Label = label;
        }
    }
}