using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;
using System.Net.Mail;
namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyDonHangController : Controller
    {
        //
        // GET: /QuanLyDonHang/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            var lstdh = db.DonDatHangs.Where(w => w.DaThanhToan == false).OrderBy(n => n.NgayDat).ToList();
            return View(lstdh);
        }
        public ActionResult ChuaGiao()
        {
            var lstdh = db.DonDatHangs.Where(w => w.TinhTrangGiaoHang == 0 && w.DaThanhToan == true).OrderBy(n => n.NgayDat).ToList();
            return View(lstdh);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstdh = db.DonDatHangs.Where(w => w.TinhTrangGiaoHang == 1 && w.DaThanhToan == true).OrderBy(n => n.NgayDat).ToList();
            return View(lstdh);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            var ddh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            ViewBag.lstCTDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id).ToList();
            return View(ddh);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            if (ddhUpdate.TinhTrangGiaoHang == 1)
            {
                ddhUpdate.NgayGiao = DateTime.Now;
            }
            db.SaveChanges();
            // GuiMail("Xác nhận đơn hàng", "thanhhust98@gmail.com", "ksshop.com.vn@gmail.com", "google123456", "Đơn hàng của bạn đã được phê duyệt");
            return RedirectToAction("ChuaThanhToan");
        }
        public void GuiMail(string title, string ToMail, string FromMail, string password, string Content)
        {
            //gọi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToMail);//địa chỉ nhận
            mail.From = new MailAddress(ToMail);//địa chỉ gửi
            mail.Subject = title;//Tiêu đề
            mail.Body = Content;//Nội dung mail
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";//host gửi của Gmail
            smtp.Port = 587;//port của gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromMail, password);//tk pass người gửi
            smtp.EnableSsl = true;//kích hoạt giao tiếp an toàn ssl
            smtp.Send(mail);//gửi mail đi


        }
    }
}