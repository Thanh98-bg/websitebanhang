using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnI.Models
{
    public class ItemGioHang
    {
      
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        //phương thức khởi tạo giỏ hàng 1
        public ItemGioHang(int iMasp)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMasp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMasp);
                this.TenSP = sp.TenSP;
                this.DonGia = sp.DonGia.Value;
                this.HinhAnh = sp.HinhAnh;
                //this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        //phương thức khởi tạo giỏ hàng 2
        public ItemGioHang(int iMasp, int sl)
        {
            using (QuanLyBanHangEntities db = new QuanLyBanHangEntities())
            {
                this.MaSP = iMasp;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMasp);
                this.TenSP = sp.TenSP;
                this.DonGia = sp.DonGia.Value;
                this.HinhAnh = sp.HinhAnh;
                this.SoLuong = sl;
                this.ThanhTien = this.DonGia * this.SoLuong;
            }
        }

       

       
    }
}