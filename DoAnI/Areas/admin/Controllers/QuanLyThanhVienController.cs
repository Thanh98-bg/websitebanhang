using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
     [Authorize(Roles = "QuanTri")]
    public class QuanLyThanhVienController : Controller
    {
        //
        // GET: /admin/QuanLyThanhVien/

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var lsttv = db.ThanhViens.ToList();
            return View(lsttv);
        }
        [HttpGet]
        public ActionResult Sua(int? id)
        {
            ThanhVien kh = db.ThanhViens.Single(n => n.MaTV == id);
            return View(kh);
        }
        [HttpPost]
        public ActionResult Sua(ThanhVien tv)
        {
            ThanhVien kh_update = db.ThanhViens.Single(n => n.MaTV == tv.MaTV);
            kh_update.HoTen = tv.HoTen;
            kh_update.DiaChi = tv.DiaChi;
            kh_update.SoDienThoai = tv.SoDienThoai;
            kh_update.Email = tv.Email;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}