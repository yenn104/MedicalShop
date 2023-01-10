using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class NhaCungCap
  {
    public NhaCungCap()
    {
      NganHangNcc = new HashSet<NganHangNcc>();
      PhieuNhap = new HashSet<PhieuNhap>();
    }

    public int Id { get; set; }

    [DisplayName("Mã NCC")]
    public string MaNcc { get; set; }

    [DisplayName("Tên NCC")]
    public string TenNcc { get; set; }

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

    public virtual ICollection<NganHangNcc> NganHangNcc { get; set; }
    public virtual ICollection<PhieuNhap> PhieuNhap { get; set; }
  }
}
