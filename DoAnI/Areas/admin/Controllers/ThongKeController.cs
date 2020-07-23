using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanLySanPham")]
    public class ThongKeController : Controller
    {
        //
        // GET: /admin/ThongKe/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            ViewBag.LuongTruyCap = HttpContext.Application["LuongTruyCap"];
            ViewBag.LuongTruyCapOnline = HttpContext.Application["LuongTruyCapOnline"];
            ViewBag.TongDoanhThu = ThongKeDoanhThu().ToString("#,##");
            ViewBag.TongThanhVien = ThongKeThanhVien();
            ViewBag.TongDonHang = ThongKeDonHang();
            return View();
        }
        public ActionResult ThongKePartial()
        {
            //lấy số lượng người truy cập từ application được tạo
            ViewBag.LuongTruyCap = HttpContext.Application["LuongTruyCap"];
            ViewBag.LuongTruyCapOnline = HttpContext.Application["LuongTruyCapOnline"];
            ViewBag.TongDoanhThu = ThongKeDoanhThu().ToString("#,##");
            ViewBag.TongThanhVien = ThongKeThanhVien();
            ViewBag.TongDonHang = ThongKeDonHang();
            return PartialView();
        }
        public double ThongKeThanhVien()
        {
            double slTV = db.ThanhViens.Count();
            return slTV;
        }
        public double ThongKeDonHang()
        {
            double sldh = db.DonDatHangs.Count();
            return sldh;
        }
        public decimal ThongKeDoanhThu()
        {
            //Tổng doanh thu từ khi websites thành lập
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n=>n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public decimal ThongKeDoanhThuThang(int Thang, int Nam)
        {
            var lstDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongDoanhThu = 0;
            foreach (var item in lstDH)
            {
                TongDoanhThu += item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            }
            return TongDoanhThu;
        }
        //Phương thức xóa bớt biến ta k dùng nữa , để web chạy nhanh hơn, controller nào cx nên có
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                }
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}