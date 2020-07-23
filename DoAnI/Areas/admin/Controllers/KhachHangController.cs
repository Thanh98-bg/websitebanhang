using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "CongTacVien")]
    public class KhachHangController : Controller
    {
        //
        // GET: /admin/KhachHang/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //var lstKH = from kh in db.KhachHangs select kh;
            var lstKH = db.KhachHangs.Where(n => n.DaXoa == false).ToList();
            return View(lstKH);
        }
        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lst = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lst);
        }
        public ActionResult Xoa(int? id)
        {
            KhachHang kh = db.KhachHangs.Single(n => n.MaKH == id);
            kh.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Sua(int? id)
        {
            KhachHang kh = db.KhachHangs.Single(n => n.MaKH == id);
            return View(kh);
        }
        [HttpPost]
        public ActionResult Sua(KhachHang kh)
        {
            KhachHang kh_update = db.KhachHangs.Single(n => n.MaKH == kh.MaKH);
            kh_update.TenKH = kh.TenKH;
            kh_update.DiaChi = kh.DiaChi;
            kh_update.SoDienThoai = kh.SoDienThoai;
            kh_update.MaTV = kh.MaTV;
            kh_update.Email = kh.Email;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}