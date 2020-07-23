using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLyNhapHangController : Controller
    {
        //
        // GET: /admin/QuanLyNhapHang/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSP = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSP = db.SanPhams;
            //Sau khi đã kiểm tra các dữ liệu đầu vào, gán đã xóa = false
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            SanPham sp;
            //Thêm list chi tiết phiếu nhập vào db
            foreach (var item in lstModel)
            {
                //cập nhập lại số lượng tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult NhapHangDon()
        {
            var lstSP = db.SanPhams.Where(n => n.SoLuongTon <= 5).ToList();
            return View(lstSP);
        }
        [HttpGet]
        public ActionResult NhapThemHang(int? id)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaNCC = db.NhaCungCaps;
            Session["MaSP"] = id;
            return View(sp);
        }
        [HttpPost]
        public ActionResult NhapThemHang(PhieuNhap pn, ChiTietPhieuNhap ctpn)
        {
            pn.DaXoa = false;
            db.PhieuNhaps.Add(pn);
            db.SaveChanges();
            ctpn.MaPN = pn.MaPN;
            ctpn.MaSP = (int)Session["MaSP"];
            //cập nhập lại số lượng tồn
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return RedirectToAction("NhapHangDon");
        }
	}

}