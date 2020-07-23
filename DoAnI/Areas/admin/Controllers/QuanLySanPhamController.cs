using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnI.Models;
using System.Net;

namespace DoAnI.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: /admin/QuanLySanPham/
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(p=>p.DaXoa==false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load dropdownlist Nhà cung cấp, Loại SP, nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            //Load dropdownlist Nhà cung cấp, Loại SP, nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            //kiểm tra hình ảnh đã tồn tại trong csdl chưa
            if (HinhAnh.ContentLength > 0)
            {
                //lấy tên hình ảnh
                var filename = System.IO.Path.GetFileName(HinhAnh.FileName);
                //lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = System.IO.Path.Combine(Server.MapPath("~/Content/images"), filename);
                //NẾU thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình ảnh đưa vào mục hình ảnh
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = filename;
                }
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            Response.Cookies["HinhAnh"].Value = sp.HinhAnh;
            //Load dropdownlist Nhà cung cấp, Loại SP, nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            //kiểm tra hình ảnh đã tồn tại trong csdl chưa
            if (HinhAnh != null)
            {

                if (HinhAnh.ContentLength > 0)
                {
                    //lấy tên hình ảnh
                    var filename = System.IO.Path.GetFileName(HinhAnh.FileName);
                    //lấy hình ảnh chuyển vào thư mục hình ảnh
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/HinhAnh"), filename);

                    //Lấy hình ảnh đưa vào mục hình ảnh
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = filename;

                }

            }
            else
            {
                var ha = Request.Cookies["HinhAnh"].Value;
                if (ha != null)
                {
                    sp.HinhAnh = ha.ToString();
                }
            }
            //Load dropdownlist Nhà cung cấp, Loại SP, nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            //Nếu dữ liệu đầu vào chắc chắn chuẩn
            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //Load dropdownlist Nhà cung cấp, Loại SP, nhà sản xuất
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.MaNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int? id,string s)
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
            db.SanPhams.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
	}
}
	
