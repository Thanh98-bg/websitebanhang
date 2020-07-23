using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
     [Authorize(Roles = "QuanTri")]
    public class PhanQuyenController : Controller
    {
        //
        // GET: /admin/PhanQuyen/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var lstLTV = db.LoaiThanhViens.ToList();
            return View(lstLTV);
        }
        [HttpGet]
        public ActionResult PhanQuyen(int id)
        {
            ViewBag.lstPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
            ViewBag.lstQuyen = db.Quyens.Where(n => n.MaQuyen != "QuanTri").ToList();
            LoaiThanhVien LTV = db.LoaiThanhViens.Single(n => n.MaLoaiTV == id);
            return View(LTV);
        }
        [HttpPost]
        public ActionResult PhanQuyen(int MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {
            //Trường hợp nếu loại thành viên đã phân quyền rồi nhưng muốn phân quyền lại
            //Bước 1: Xóa các quyền của ltv đó
            var lstQuyenCu = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV && n.MaQuyen != "QuanTri");
            if (lstQuyenCu != null)
            {
                db.LoaiThanhVien_Quyen.RemoveRange(lstQuyenCu);
                db.SaveChanges();
            }
            //bước 2: Thêm các quyền cho ltv đó
            foreach (var item in lstPhanQuyen)
            {
                item.MaLoaiTV = MaLTV;
                db.LoaiThanhVien_Quyen.Add(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}