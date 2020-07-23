using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuyenController : Controller
    {
        //
        // GET: /admin/Quyen/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();     
        public ActionResult Index()
        {
            var lstQuyen = db.Quyens.Where(n=>n.MaQuyen != "QuanTri").ToList();
            return View(lstQuyen);
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemQuyen(Quyen q)
        {
            Quyen q_new = new Quyen();
            q_new.TenQuyen = q.TenQuyen;
            q_new.MaQuyen = q.MaQuyen;
            db.Quyens.Add(q_new);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(string mq)
        {

            var q = db.Quyens.Single(n => n.MaQuyen == mq);
            Session["mq"] = mq;
            return View(q);
        }
        [HttpPost]
        public ActionResult ChinhSua(Quyen q)
        {
            string mq = Session["mq"].ToString();
            Quyen q_update = db.Quyens.Single(n => n.MaQuyen == mq);

            q_update.TenQuyen = q.TenQuyen;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Xoa(string mq)
        {
            Quyen q = db.Quyens.Single(n => n.MaQuyen == mq);
            db.Quyens.Remove(q);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}