using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class NhanVien
  {
    public NhanVien()
    {
      KhachHang = new HashSet<KhachHang>();
      PhieuNhap = new HashSet<PhieuNhap>();
      PhieuXuat = new HashSet<PhieuXuat>();
    }

    public int Id { get; set; }

    [DisplayName("Nhóm NV")]
    public int? Idnnv { get; set; }

    [DisplayName("Mã NV")]
    public string MaNv { get; set; }

    [DisplayName("Tên NV")]
    public string TenNv { get; set; }

    [DisplayName("Giới tính")]
    public bool? Sex { get; set; }

    [DisplayName("Ngày sinh")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime? DateOfBirth { get; set; }

    [DisplayName("Địa chỉ")]
    public string Address { get; set; }

    [DisplayName("Quê quán")]
    public string HomeTown { get; set; }

    [DisplayName("SĐT")]
    public string Phone { get; set; }

    [DisplayName("Mail")]
    public string Mail { get; set; }

    [DisplayName("CCCD")]
    public string Cccd { get; set; }

    [DisplayName("Hình ảnh")]
    public string Image { get; set; }

    [DisplayName("Tên tài khoản")]
    public string UserName { get; set; }

    public string MoreImages { get; set; }
    [DisplayName("Ngày tạo")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
    public DateTime? CreatedDate { get; set; }

    [DisplayName("NV tạo")]
    public int? CreatedBy { get; set; }

    [DisplayName("Ngày sửa")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("NV sửa")]
    public int? ModifiedBy { get; set; }

    [DisplayName("Chi Nhánh")]
    public int? Idcn { get; set; }

    [DisplayName("Trạng thái")]
    public bool? Active { get; set; }

    public virtual NhomNhanVien IdnnvNavigation { get; set; }
    public virtual TaiKhoan UserNameNavigation { get; set; }
    public virtual ICollection<KhachHang> KhachHang { get; set; }
    public virtual ICollection<PhieuNhap> PhieuNhap { get; set; }
    public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
  }
}
