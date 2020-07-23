using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;
using PagedList;
namespace DoAnI.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 2;
            int PageNumber = (page ?? 1);
            //Tìm kiếm theo tên sản phẩm
            ViewBag.TuKhoa = sTuKhoa;

            var lstKQ = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            if (lstKQ.Count()==0)
            {
                return View("KhongTimThay");
            }
            return View(lstKQ.OrderBy(n=>n.TenSP).ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            return RedirectToAction("KQTimKiemPartial", new { @sTuKhoa = sTuKhoa });
        }
        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            var lstKQ = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            if (lstKQ.Count() == 0)
            {
                return PartialView("KhongTimThay");
            }
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstKQ);
        }
	}
}