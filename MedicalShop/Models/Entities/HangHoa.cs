using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class HangHoa
  {
    public HangHoa()
    {
      ChiTietPhieuNhap = new HashSet<ChiTietPhieuNhap>();
      ChiTietPhieuXuat = new HashSet<ChiTietPhieuXuat>();
      HhDvt = new HashSet<HhDvt>();
      HhImage = new HashSet<HhImage>();
    }


    public int Id { get; set; }

    [DisplayName("Nhóm HH")]
    public int? Idnhh { get; set; }

    [DisplayName("HSX")]
    public int? Idhsx { get; set; }

    [DisplayName("NSX")]
    public int? Idnsx { get; set; }

    [DisplayName("ĐVT")]
    public int? Iddvtc { get; set; }

    [DisplayName("Mã HH")]
    public string MaHh { get; set; }

    [DisplayName("Tên HH")]
    public string TenHh { get; set; }

    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-----")]
    public string BarCode { get; set; }

    [DisplayName("Hình ảnh")]
    public string Image { get; set; }

    [DisplayName("SL")]
    public int? Quantity { get; set; }

    [DisplayName("Ghi chú")]
    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "-----")]
    public string Detail { get; set; }

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

    public virtual Hsx IdhsxNavigation { get; set; }
    public virtual NhomHangHoa IdnhhNavigation { get; set; }
    public virtual Nsx IdnsxNavigation { get; set; }
    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhap { get; set; }
    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuat { get; set; }
    public virtual ICollection<HhDvt> HhDvt { get; set; }
    public virtual ICollection<HhImage> HhImage { get; set; }
  }
}
