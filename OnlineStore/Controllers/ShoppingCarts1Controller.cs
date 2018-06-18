using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using OnlineStore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace OnlineStore.Controllers
{
    public class ShoppingCarts1Controller : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        //POST: api/ShoppingCarts1/5
        [ResponseType(typeof(ShoppingCart))]
        public IHttpActionResult PostToShoppingCart(int Id)
        {

            String UserID = base.ControllerContext.RequestContext.Principal.Identity.Name;
            ShoppingCart shoppingCart = db.ShoppingCarts.ToList().Find(x => x.UserID == UserID);
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = UserID;
                db.ShoppingCarts.Add(shoppingCart);
                
            }
            shoppingCart.ChoosedProducts.Add(db.Products.Find(Id));
            db.SaveChanges();

            return Ok();
        }


        // DELETE: api/ShoppingCarts1/5
        [ResponseType(typeof(ShoppingCart))]
        public IHttpActionResult DeleteShoppingCart(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            db.ShoppingCarts.Remove(shoppingCart);
            db.SaveChanges();

            return Ok(shoppingCart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShoppingCartExists(int id)
        {
            return db.ShoppingCarts.Count(e => e.ID == id) > 0;
        }
    }
}