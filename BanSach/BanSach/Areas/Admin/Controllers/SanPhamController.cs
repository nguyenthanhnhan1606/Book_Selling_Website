using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
namespace BanSach.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        public ActionResult Java(string keyword = "")
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var s = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(s.ToList());
            }
            
            List<Sach> sach = db.Sach.ToList();
            return View(sach);
        }
        // GET: Admin/SanPham
        public ActionResult C(string keyword = "")
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var s = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(s.ToList());
            }   
             List<Sach> sach = db.Sach.ToList();
            return View(sach);
        }

        // GET: Admin/SanPham
        public ActionResult JavaScript(string keyword = "")
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var s = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(s.ToList());
            }
            List<Sach> sach = db.Sach.ToList();
            return View(sach);
        }
        // GET: Admin/SanPham
        public ActionResult Python(string keyword = "")
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var s = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(s.ToList());
            }
            List<Sach> sach = db.Sach.ToList();
            return View(sach);
        }

        public ActionResult ALL(string keyword = "")
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            if (keyword != "")
            {
                var s = db.Sach.Where(x => x.tenSach.ToUpper().Contains(keyword.ToUpper()));
                return View(s.ToList());
            }
            List<Sach> sach = db.Sach.ToList();
            return View(sach);
        }

        public ActionResult Them()
        {
            return View(new Sach() { GiaTien=0} );
        }
        [HttpPost]
        public ActionResult Them(Sach s)
        {
            //lưu dữ liệu vào db
            if (string.IsNullOrEmpty(s.tenSach) == true || string.IsNullOrEmpty(s.tenTacGia) == true || string.IsNullOrEmpty(s.image) == true)
            {
                ModelState.AddModelError("", "Tên sản phẩm hoặc tên tác giả không được để trống");
                return View(s);
            }
            if (s.GiaTien <= 0 )
            {
                ModelState.AddModelError("", "Gía bán không được nhỏ hơn 0");
                return View(s);
            }
            //LƯU
            dbbansachEntities2 db = new dbbansachEntities2();
            //Hàm thêm sách
            db.Sach.Add(s);
            //Hàm thêm dữ liệu
            db.SaveChanges();
            if (s.maSach > 0 )
            {
                return RedirectToAction("ALL");
            }
            else
            {
                ModelState.AddModelError("", "Không lưu được dữ liệu");
                return View(s);
            }
        }
      
        public ActionResult CapNhat(int id) 
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var ids = db.Sach.Find(id);
            return View(ids);
        }
        [HttpPost]
        public ActionResult CapNhat(Sach s)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            Sach sa = db.Sach.FirstOrDefault(x => x.maSach==s.maSach);
            if (string.IsNullOrEmpty(s.tenSach) == true || string.IsNullOrEmpty(s.tenTacGia) == true || string.IsNullOrEmpty(s.image)==true)
            {
                ModelState.AddModelError("", "Tên sản phẩm hoặc tên tác giả, image không được để trống");
                return View(s);
            }
            else
            {
                sa.tenSach = s.tenSach;
                sa.tenTacGia = s.tenTacGia;
                sa.image = s.image;
            }
            if (s.GiaTien <= 0)
            {
                ModelState.AddModelError("", "Gía bán không được nhỏ hơn 0");
                return View(s);
            }
            else
            {
                sa.GiaTien = s.GiaTien;
            }
            sa.sach_ncc = s.sach_ncc;
            sa.sach_TLS = s.sach_TLS;
            
            db.SaveChanges();
            return RedirectToAction("ALL");
        }

        public ActionResult Xoa(int id)
        {
            dbbansachEntities2 db = new dbbansachEntities2();
            var s = db.Sach.Find(id);
            db.Sach.Remove(s);
            db.SaveChanges();
            return RedirectToAction("ALL");

        }

    }
}