using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class MyWallet
    {
        public String ID { get;  }
        public String Password { get;  }

        public MyWallet(string iD, string password)
        {
            ID = iD;
            Password = password;
        }
    }
}