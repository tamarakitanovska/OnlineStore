using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.Product;

namespace OnlineStore.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingCarts
        public ActionResult ViewCarts()
        {
            return View(db.ShoppingCarts.ToList());
        }


        //OK
        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                ShoppingCart shoppingCartNew = new ShoppingCart();
                shoppingCartNew.UserID = HttpContext.User.Identity.Name;
            }
            //model generating html for shoping cart
            ModelForShoppingCartDetails model = new ModelForShoppingCartDetails();
            model.ShoppingCart = shoppingCart;
            model.product = new Product();
            return View(model);
        }

        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
