﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class KhachHang
  {
    public KhachHang()
    {
      NganHangKh = new HashSet<NganHangKh>();
      PhieuXuat = new HashSet<PhieuXuat>();
    }

    public int Id { get; set; }
    [DisplayName("NV Sale")]
    public int? Idnv { get; set; }

    [DisplayName("Mã KH")]
    public string MaKh { get; set; }

    [DisplayName("Tên KH")]
    public string TenKh { get; set; }

    [DisplayName("Tên TK")]
    public string UserName { get; set; }

    [DisplayName("Địa chỉ")]
    public string Address { get; set; }

    [DisplayName("SĐT")]
    public string Phone { get; set; }

    public string Mail { get; set; }

    [DisplayName("Ghi chú")]
    public string Note { get; set; }

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

    public virtual NhanVien IdnvNavigation { get; set; }
    public virtual TaiKhoan UserNameNavigation { get; set; }
    public virtual ICollection<NganHangKh> NganHangKh { get; set; }
    public virtual ICollection<PhieuXuat> PhieuXuat { get; set; }
  }
}
