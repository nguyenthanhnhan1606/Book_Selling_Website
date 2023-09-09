using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanSach.Models
{
    public class CartItem
    {
        public Sach sach_dat { get; set; }
        public int soLuong { get; set; }
    }

    //Gio hàng
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Sach s, int sl = 1)
        {
            var item = items.FirstOrDefault(x => x.sach_dat.maSach == s.maSach);
            if (item == null)
            {
                items.Add(new CartItem
                {
                    sach_dat = s,
                    soLuong = sl
                });
            }
            else
            {
                if (item.soLuong < 5)
                {
                   item.soLuong += sl;
                }
            }
                
        }

        public void UpdateQuantity(int id, int quantity)
        {
            var item = items.Find(s => s.sach_dat.maSach == id);
            if (item != null)
            {
                item.soLuong = quantity;
            }
        }

        public double TongTien()
        {
            var tongTien = items.Sum(s => s.sach_dat.GiaTien * s.soLuong);
            return (double)tongTien;
        }

        public double TongSP()
        {
            var tongSP = items.Sum(s => s.soLuong);
            return (double)tongSP;
        }

        public void RemoveCart(int id)
        {
            items.RemoveAll(s => s.sach_dat.maSach == id);
                 
        }
    }
}