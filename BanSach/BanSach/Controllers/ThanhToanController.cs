using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanSach.Models;
using Newtonsoft.Json.Linq;

namespace BanSach.Controllers
{
    public class ThanhToanController : Controller
    {
        dbbansachEntities2 db = new dbbansachEntities2();
        // GET: ThanhToan
        public ActionResult ThanhToanHD()
        {
            int dem = 0;
            double total = 0;
            var t=db.HoaDon.ToList();
            dem = t.Count();
            BanSach.Models.Cart test = new BanSach.Models.Cart();
            test =  Session["cart"] as Cart;
            HoaDon hd = new HoaDon() ;
            hd.maHD = dem+1;
            hd.ngayThanhToan = DateTime.Now;
            hd.user_hd = (int)Session["id"];
            db.HoaDon.Add(hd);
            db.SaveChanges();
            Session["maHd"] = hd.maHD;

            int cthd = hd.maHD;
            List<ChiTietHoaDon> chitietHD = new List<ChiTietHoaDon>();
            foreach (var item in test.Items)
            {
                if (item != null)
                {
                    ChiTietHoaDon chitiet = new ChiTietHoaDon();
                    chitiet.id_admin = 1;
                    chitiet.id_maHD = cthd;
                    chitiet.id_sach = item.sach_dat.maSach;
                    chitiet.soluong = item.soLuong;
                    chitiet.gia = item.sach_dat.GiaTien;
                    total += Double.Parse((item.soLuong * item.sach_dat.GiaTien).ToString());
                    chitietHD.Add(chitiet);
                }
                else
                {
                    ModelState.AddModelError("Error", "Tài khoản hoặc mật khẩu không đúng");
                    return RedirectToAction("Index","Home");
                }
                
            }
            db.ChiTietHoaDon.AddRange(chitietHD);
            db.SaveChanges();
            Session["cart"] = null;
            Session["tongsl"] = null;



            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO5RGX20191128";
            string accessKey = "M8brj9K6E22vXoDB";
            string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
            string orderInfo = "Thanh toán ngày " + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
            string redirectUrl = "https://localhost:44332/";
            string ipnUrl = "https://localhost:44332/";
            string requestType = "captureWallet";

            string amount = total.ToString();
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            MoMo.MoMoSecurity crypto = new MoMo.MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            string responseFromMomo = MoMo.PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            JObject jmessage = JObject.Parse(responseFromMomo);
            return Redirect(jmessage.GetValue("payUrl").ToString());

        }
    }
}