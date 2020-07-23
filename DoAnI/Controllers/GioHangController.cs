using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //Nếu session giỏ hàng chưa tồn tại thì khởi tạo giỏ hàng
                List<ItemGioHang> lstGH = new List<ItemGioHang>();
                Session["GioHang"] = lstGH;
                return lstGH;
            }
            return lstGioHang;
        }
        //Lấy giỏ hàng thông thường (load lại trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Trường hợp 1: sản phẩm đã tồn tại trong giỏ hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            //Trường hợp 2: sản phẩm chưa tồn tại trong giỏ hàng
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            itemGH.SoLuong++;
            itemGH.ThanhTien = itemGH.SoLuong * itemGH.DonGia;
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        //Tinh tong so lương
        public double TinhTongSL()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSL() == 0)
            {
                ViewBag.TongSL = 0;
                ViewBag.TongTien = "0";
                return PartialView();
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien().ToString("#,##");
            return PartialView();
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra xem giỏ hàng tồn tại hay chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra xem sản phẩm tồn tại trong CSDL không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ Session
            List<ItemGioHang> lstGH = Session["GioHang"] as List<ItemGioHang>;
            //Kiểm tra xem sản phẩm có tồn tại trong giỏ hàng không
            ItemGioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy list giỏ hàng tạo giao diện
            ViewBag.GioHang = lstGH;
            //Trả về sản phẩm cần sửa
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(int MaSP, int SoLuong)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck.SoLuongTon<SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemUpdate = lstGH.Find(n => n.MaSP == MaSP);
            itemUpdate.SoLuong = SoLuong;
            itemUpdate.ThanhTien = itemUpdate.SoLuong * itemUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra xem giỏ hàng tồn tại hay chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra xem sản phẩm tồn tại trong CSDL không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //Lấy list giỏ hàng từ Session
            List<ItemGioHang> lstGH = Session["GioHang"] as List<ItemGioHang>;
            //Kiểm tra xem sản phẩm có tồn tại trong giỏ hàng không
            ItemGioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xóa sp
            lstGH.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        //Xây dựng chức năng đặt hàng:
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //Đối với khách hàng vãng lai
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaTV = tv.MaLoaiTV;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.UuDai = 0;
            ddh.DaThanhToan = false;
            ddh.DaHuy = false;
            ddh.TinhTrangGiaoHang = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult ThemGioHangAjax(int MaSP)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                //Trường hợp 1: sản phẩm đã tồn tại trong giỏ hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script>alert(\"Sản phẩm không đủ hàng\");</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSL = TinhTongSL();
                ViewBag.TongTien = TinhTongTien().ToString("#,##");
                return PartialView("GioHangPartial");
            }
            //Trường hợp 2: sản phẩm chưa tồn tại trong giỏ hàng
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script>alert(\"Sản phẩm không đủ hàng\");</script>");
            }
            itemGH.SoLuong++;
            itemGH.ThanhTien = itemGH.SoLuong * itemGH.DonGia;
            lstGioHang.Add(itemGH);
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien().ToString("#,##");
            return PartialView("GioHangPartial");
        }
	}
}