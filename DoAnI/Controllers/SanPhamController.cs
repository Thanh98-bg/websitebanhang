using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;
using System.Net;
using PagedList;

namespace DoAnI.Controllers
{

   // [Authorize(Roles = "QuanLySanPham")]
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //[Authorize(Roles = "QuanTri")]
        public ActionResult XemChiTiet(int? id, string bidanh)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            else
            return View(sp);
        }
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP == null)
            {
                return HttpNotFound();
            }
            
            //Thực hiện chức năng phân trang
            //Tạo biến số sp trên 1 trang
            int PageSize = 2;
            //Tạo biến lưu số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
        }
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            return PartialView();
        }
        
	}
}