﻿using System;
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

        [Authorize(Roles ="Admin")]
        // GET: ShoppingCarts
        public ActionResult ViewCarts()
        {
            return View(db.ShoppingCarts.ToList());
        }
        [Authorize]
        [HttpPost]
        public ActionResult PostProductToCart(int Id)
        {
            String UserId = db.Users.ToList().Find(x => x.UserName == HttpContext.User.Identity.Name).Id;
            ShoppingCart shoppingCart = db.ShoppingCarts.FirstOrDefault(x => x.UserID == UserId);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = UserId;
                db.ShoppingCarts.Add(shoppingCart);
                db.SaveChanges();
            }

            shoppingCart.ChoosedProducts.Add(db.Products.Find(Id));
            
            db.SaveChanges();
            return RedirectToAction("Details", "Products", new { Id = Id });
        }

       

        
        public ActionResult RemoveFromCart(int Id)
        {
            String UserId = db.Users.ToList().Find(x => x.UserName == HttpContext.User.Identity.Name).Id;
            ShoppingCart shoppingCart = db.ShoppingCarts.FirstOrDefault(x => x.UserID == UserId);


            shoppingCart.ChoosedProducts.Remove(db.Products.Find(Id));

            db.SaveChanges();
            return RedirectToAction("Details", "ShoppingCarts", new { Id = shoppingCart.ID });
        }
        [Authorize]
        // GET: ShoppingCarts/Details/5
        public ActionResult Details()
        {
            String UserId = db.Users.ToList().Find(x=>x.UserName==HttpContext.User.Identity.Name).Id;
            ShoppingCart shoppingCart = db.ShoppingCarts.ToList().Find(x => x.UserID == UserId);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = UserId;
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
