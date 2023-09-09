using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BanSach.Models;
using System.Web.Mvc;

namespace BanSach.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string keyword="", int Ma_TL=0)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var sach = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(sach.ToList());
            }
            if (Ma_TL != 0)
            {
                var sach = db.Sach.Where(x => x.sach_TLS==Ma_TL);
                if (sach == null)
                {
                    return View();
                }
                else
                    return View(sach.ToList());
            }
            List<Sach> s = db.Sach.ToList();
            return View(s);
        }

        //Login user
        public ActionResult LoginUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(User user)
        {
            BanSach.Models.dbbansachEntities2 db = new dbbansachEntities2();
            var a = db.User.FirstOrDefault(x => x.username.Equals(user.username) && x.password.Equals(user.password));  
            {
                if (a != null)
                {
                    Session["user"] = a;
                    Session["username1"] = a.ten;
                    Session["id"] = a.id;
                    return RedirectToAction("Index");

                }
                else
                {

                    ModelState.AddModelError("Error", "Tài khoản hoặc mật khẩu không đúng");
                    return View();
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult RegisterUser()
        {
            return View(new User());
        }
        [HttpPost]
        public ActionResult RegisterUser(User user)
        {
            if (string.IsNullOrEmpty(user.username) == true || string.IsNullOrEmpty(user.password) == true || string.IsNullOrEmpty(user.ten) == true
                 || string.IsNullOrEmpty(user.sdt) == true)
            {
                ModelState.AddModelError("", "Không được để ô trống");
                return View(user);
            }
            BanSach.Models.dbbansachEntities2 db1 = new BanSach.Models.dbbansachEntities2();
            var u = db1.User.ToList();
            foreach (var i in u)
            {
                if (user.username.Equals(i.username))
                {
                    ModelState.AddModelError("", "Username đã được sử dụng");
                    return View(user);
                }
            }
            dbbansachEntities2 db = new dbbansachEntities2();
            //Hàm thêm user
            db.User.Add(user);
            //Hàm lưu dữ liệu
            db.SaveChanges();
            if (user.id > 0)
            {
                return RedirectToAction("LoginUser");
            }
            else
            {
                ModelState.AddModelError("", "Không lưu được dữ liệu");
                return View(user);
            }
            return RedirectToAction("Index");

        }

        public ActionResult Detail(int id) 
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var s = db.Sach.Find(id);
            Session["id"] = s.maSach;
            return View(s);
        }

        public ActionResult Cart()
        {
            return View();
        }
        
    }
}