//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            this.ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }
    
        public int MaPN { get; set; }
        public Nullable<int> MaNCC { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public Nullable<bool> DaXoa { get; set; }
    
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
