using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BanSach.Models;
using System.Web.Mvc;
using System.Security.Cryptography;

namespace BanSach.Areas.Admin.Controllers
{
    public class HomeADController : Controller
    {
        // GET: Admin/HomeAD
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            return View();
        }

        // GET: Admin/HomeAD
        public ActionResult Charts()
        {
            DateTime from_date = new DateTime();
            String s = Request.QueryString["from_date"];
            DateTime to_date = new DateTime();
            String s1 = Request.QueryString["to_date"];
            var list = new List<HoaDon>();
            dbbansachEntities2 db = new dbbansachEntities2();
            if (s != null && s1 != null)
            {
                from_date = DateTime.Parse(s);
                to_date= DateTime.Parse(s1);
                list = db.HoaDon.Where(x => x.ngayThanhToan >= from_date && x.ngayThanhToan <= to_date).ToList();
            }
            else
            {
                list = db.HoaDon.ToList();
            }
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            return View(list);
        }


        public ActionResult User()
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            List<User> user = db.User.ToList();
            return View(user);
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(admin ad)
        {
            BanSach.Models.dbbansachEntities2 db = new dbbansachEntities2();
            var a = db.admin.FirstOrDefault(x => x.username.Equals(ad.username) && x.password.Equals(ad.password));
            {
                if (a != null)
                {
                    Session["admin"] = a.id;
                    Session["username"] = a.username;
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

        public ActionResult Register()
        {
            return View(new User());
        }
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (string.IsNullOrEmpty(user.username) == true || string.IsNullOrEmpty(user.password) == true || string.IsNullOrEmpty(user.ten) == true
                 || string.IsNullOrEmpty(user.sdt) == true)
            {
                ModelState.AddModelError("", "Không được để ô trống");
                return View(user);
            }
            BanSach.Models.dbbansachEntities2 db1 = new BanSach.Models.dbbansachEntities2();
            var u= db1.User.ToList();
            foreach (var i in u)
            {
                if(user.username.Equals(i.username))
                {
                    ModelState.AddModelError("", "Username đã được sử dụng");
                    return View(user);
                }
            }
            if (user.sdt.Length != 10)
            {
                ModelState.AddModelError("", "Số điện thoại không tồn tại");
                return View(user);
            }
            dbbansachEntities2 db = new dbbansachEntities2();
            //Hàm thêm user
            db.User.Add(user);
            //Hàm lưu dữ liệu
            db.SaveChanges();
            if (user.id > 0)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "Không lưu được dữ liệu");
                return View(user);
            }
            return RedirectToAction("User");

        }

        public ActionResult CapNhatUser(int id)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var ids = db.User.Find(id);
            return View(ids);
        }
        [HttpPost]
        public ActionResult CapNhatUser(User user)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            User u = db.User.FirstOrDefault(x => x.id == user.id);
            if (string.IsNullOrEmpty(user.ten) == true || string.IsNullOrEmpty(user.username) == true || string.IsNullOrEmpty(user.password) == true
                || string.IsNullOrEmpty(user.sdt) == true)
            {
                ModelState.AddModelError("", "Không được để trống ô nào!!");
                return View(user);
            }
            else
            {
                var ur = db.User.ToList();
                foreach (User r in ur) {
                    if (r.username.Equals(user.username) && r.id != user.id) {
                        ModelState.AddModelError("", "Username đã tồn tại");
                        return View(user);
                    }
                }
                if (user.sdt.Length != 10)
                {
                    ModelState.AddModelError("", "Số điện thoại không tồn tại");
                    return View(user);
                }
                u.ten = user.ten;
                u.username = user.username;
                u.password = user.password;
                u.ngaysinh = user.ngaysinh;
                u.sdt = user.sdt;
            }
                db.SaveChanges();
                return RedirectToAction("User");
            
        }

        public ActionResult Xoa(int id)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var s = db.User.Find(id);
            db.User.Remove(s);
            db.SaveChanges();
            return RedirectToAction("User");

        }

        public ActionResult CapNhatAdmin(int id)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var ids = db.admin.Find(id);
            return View(ids);
        }
        [HttpPost]
        public ActionResult CapNhatAdmin(admin ad)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            admin u = db.admin.FirstOrDefault(x => x.id == ad.id);
            if (string.IsNullOrEmpty(ad.ten) == true || string.IsNullOrEmpty(ad.username) == true || string.IsNullOrEmpty(ad.password) == true
                || string.IsNullOrEmpty(ad.sdt) == true || string.IsNullOrEmpty(ad.email) == true)
            {
                ModelState.AddModelError("", "Không được để trống");
                return View(ad);
            }
            else
            {
                var t = db.admin.ToList();
                foreach (admin a in t)
                {
                    if (a.username.Equals(ad.username) && a.id != ad.id )
                    {
                        ModelState.AddModelError("", "Username đã tồn tại");
                        return View(ad);
                    }
                }
                if (ad.sdt.Length != 10)
                {
                    ModelState.AddModelError("", "Số điện thoại không tồn tại");
                    return View(ad);
                }
                u.ten = ad.ten;
                u.username = ad.username;
                u.password = ad.password;
                u.diachi = ad.diachi;
                u.luong = ad.luong;
                u.ngaylamviec = ad.ngaylamviec;
                u.sdt = ad.sdt;
            }
            db.SaveChanges();
            return RedirectToAction("User");

        }

        public ActionResult ThemAD()
        {
            return View(new admin() { luong=0 });
        }
        [HttpPost]
        public ActionResult ThemAD(admin ad)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            //lưu dữ liệu vào db
            if (string.IsNullOrEmpty(ad.ten) == true || string.IsNullOrEmpty(ad.username) == true || string.IsNullOrEmpty(ad.password) == true
                 || string.IsNullOrEmpty(ad.sdt) == true || string.IsNullOrEmpty(ad.email) == true)
            {
                ModelState.AddModelError("", "Không được để trống");
                return View(ad);
            }
            if (ad.luong <0 )
            {
                ModelState.AddModelError("", "Lương không được < 0");
                return View(ad);
            }
            var t = db.admin.ToList();
            foreach (admin a in t)
            {
                if (a.username.Equals(ad.username) && a.id != ad.id)
                {
                    ModelState.AddModelError("", "Username đã tồn tại");
                    return View(ad);
                }
            }
            if (ad.sdt.Length != 10)
            {
                ModelState.AddModelError("", "Số điện thoại không tồn tại");
                return View(ad);
            }
            //Hàm thêm 
            db.admin.Add(ad);
            //Hàm thêm dữ liệu
            db.SaveChanges();
            if (ad.id > 0)
            {
                return RedirectToAction("User");
            }
            else
            {
                ModelState.AddModelError("", "Không lưu được dữ liệu");
                return View(ad);
            }
        }


        public ActionResult XoaAD(int id)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var s = db.admin.Find(id);
            db.admin.Remove(s);
            db.SaveChanges();
            return RedirectToAction("User");

        }

      
    }
}