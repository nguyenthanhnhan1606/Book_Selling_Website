using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;

namespace BanSach.Controllers
{
    public class CartController : Controller
    {
        dbbansachEntities2 db = new dbbansachEntities2();
        // GET: Cart

        public Cart GetCart()
        {
            Cart cart = Session["cart"] as Cart;
            if (cart == null || Session["cart"] == null)
            {
                cart = new Cart();
                Session["cart"] = cart;
            }
            Session["tongsl"] = cart.TongSP()+1;
            return cart;
        }

        public ActionResult AddToCart(int id)
        {
            var s = db.Sach.SingleOrDefault(x => x.maSach == id);
            if (s != null)
            {
                GetCart().Add(s);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ShowToCart", "Cart");
        }

        public ActionResult ShowToCart()
        {
            Cart cart = Session["cart"] as Cart;
            return View(cart);
        }

        public ActionResult ThanhToanSS()
        {
            return View();
        }

        public ActionResult UpdateCart(FormCollection form)
        {
            Cart cart = Session["cart"] as Cart;
            int id = int.Parse(form["id"]);
            int quantity = int.Parse(form["quantity"]);
            if (quantity < 1 || quantity>5)
            {
                ModelState.AddModelError("", "Lỗi");
                return RedirectToAction("ShowToCart", "Cart");
            }
            cart.UpdateQuantity(id, quantity);
            Session["tongsl"] = cart.TongSP() ;
            return RedirectToAction("ShowToCart", "Cart");
        }

        public ActionResult Remove(int id)
        {
            Cart cart = Session["cart"] as Cart;
            cart.RemoveCart(id);
            Session["tongsl"] = cart.TongSP() ;
            return RedirectToAction("ShowToCart", "Cart");
        }
    }
}