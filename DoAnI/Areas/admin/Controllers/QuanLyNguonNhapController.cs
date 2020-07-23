using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyNguonNhapController : Controller
    {
        //
        // GET: /admin/QuanLyNguonNhap/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            ViewBag.LstNCC = db.NhaCungCaps.Where(n=>n.DaXoa== false).ToList();
            ViewBag.LstNSX = db.NhaSanXuats.Where(n=>n.DaXoa==false).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult ThemNCC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNCC(NhaCungCap ncc)
        {
            ncc.DaXoa = false;
            db.NhaCungCaps.Add(ncc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ThemNSX()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNSX(NhaSanXuat nsx)
        {
            nsx.DaXoa = false;
            db.NhaSanXuats.Add(nsx);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult SuaNCC(int? id)
        {
            NhaCungCap ncc = db.NhaCungCaps.Single(n => n.MaNCC == id);
            return View(ncc);
        }
        [HttpPost]
        public ActionResult SuaNCC(NhaCungCap ncc)
        {
            NhaCungCap nccUpdate = db.NhaCungCaps.Single(n => n.MaNCC == ncc.MaNCC);
            nccUpdate.TenNCC = ncc.TenNCC;
            nccUpdate.DiaChi = ncc.DiaChi;
            nccUpdate.Email = ncc.Email;
            nccUpdate.SoDienThoai = ncc.SoDienThoai;
            nccUpdate.Fax = ncc.Fax;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult SuaNSX(int? id)
        {
            NhaSanXuat nsx = db.NhaSanXuats.Single(n => n.MaNSX == id);
            return View(nsx);
        }
        [HttpPost]
        public ActionResult SuaNSX(NhaSanXuat nsx)
        {
            NhaSanXuat nsx_update = db.NhaSanXuats.Single(n => n.MaNSX == nsx.MaNSX);
            nsx_update.ThongTin = nsx.ThongTin;
            nsx_update.TenNSX = nsx.TenNSX;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult XoaNCC(int? id)
        {
            NhaCungCap ncc_delete = db.NhaCungCaps.Single(n => n.MaNCC == id);
            ncc_delete.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XoaNSX(int? id)
        {
            NhaSanXuat nsx_delete = db.NhaSanXuats.Single(n => n.MaNSX == id);
            nsx_delete.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}