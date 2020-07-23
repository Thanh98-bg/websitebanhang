using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnI.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            //Danh sach thuộc tính
            public int MaTV { get; set; }

            [DisplayName("Tài khoản")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} phải từ {2} đến {1} kí tự")]
            public string TaiKhoan { get; set; }

            [DisplayName("Mật khẩu")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string MatKhau { get; set; }

            [DisplayName("Họ tên")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string HoTen { get; set; }

            [DisplayName("Địa chỉ")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string DiaChi { get; set; }

            [DisplayName("Email")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Vui lòng nhập đúng Email ")]
            public string Email { get; set; }

            [DisplayName("Số điện thoại")]
            [Required(ErrorMessage = "{0} không được bỏ trống")]
            public string SoDienThoai { get; set; }

            public string CauHoi { get; set; }

            public string CauTraLoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }


        }
    }
}