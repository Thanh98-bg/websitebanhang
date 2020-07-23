using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;
using PagedList;
namespace DoAnI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //Session["Flag"] = 0;
            ViewBag.lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == true && n.DaXoa == false);
            ViewBag.lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == true && n.DaXoa == false);
            ViewBag.lstMTBM = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == true && n.DaXoa == false);
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
       [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
       public ActionResult DangKy(ThanhVien tv)
       {
           ViewBag.CauHoi = new SelectList(LoadCauHoi());
           if (this.IsCaptchaValid("Captcha is not valid"))
           {
               if (ModelState.IsValid) { 
               ViewBag.ThongBao = "Bạn đã đăng ký thành công";
               tv.MaLoaiTV = 2;
               db.ThanhViens.Add(tv);
               db.SaveChanges();
               }
               else {
                   ViewBag.ThongBao = "Đăng ký thất bại";
               }
               return View();
           }
           ViewBag.ThongBao = "Đăng ký thất bại";
           ViewBag.Captcha = "Sai mã captcha";
           return View();
          
       }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật bạn yêu thích nhất ?");
            lstCauHoi.Add("Ca sĩ bạn yêu thích nhất ?");
            lstCauHoi.Add("Bạn đang làm công việc gì ?");
            return lstCauHoi;
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaiKhoan = f["txtTaiKhoan"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                //Response.Cookies["TaiKhoan"].Value = tv.TaiKhoan;
                //return Content("<script>window.location.reload();</script>");
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //Duyệt list quyền
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);//cắt đi dấu phẩy cuối
                PhanQuyen(tv.TaiKhoan, Quyen);
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tên đăng nhập hoặc mật khẩu không đúng");
        }
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                TaiKhoan,//user
                DateTime.Now,//begin
                DateTime.Now.AddHours(3),//timeout
                false,//remember ?
                Quyen,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);  
        }
        //tạo trang bẫy người dùng khi truy cập vào trang không đủ quyền
        public ActionResult KhongDuQuyen()
        {
            return View();
        }
	
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            //TaiKhoan.Expires = DateTime.Now.AddDays(-1);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
	}
}