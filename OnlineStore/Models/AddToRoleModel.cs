using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class AddToRoleModel
    {
        public String Email { get; set; }
        public List<String> Roles { get; set; }

        public String SelectedRole { get; set; }

        public AddToRoleModel()
        {
            Roles = new List<string>();
        }
    }
}