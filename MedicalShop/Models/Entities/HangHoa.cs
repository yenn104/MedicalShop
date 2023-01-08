using System;
using System.Collections.Generic;
using System.ComponentModel;

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
    public int? Idnhh { get; set; }
    public int? Idhsx { get; set; }
    public int? Idnsx { get; set; }
    public int? Iddvtc { get; set; }

    [DisplayName("Mã HH")]
    public string MaHh { get; set; }

    [DisplayName("Tên HH")]
    public string TenHh { get; set; }

    public string BarCode { get; set; }

    [DisplayName("Hình ảnh")]
    public string Image { get; set; }

    [DisplayName("Số lượng")]
    public int? Quantity { get; set; }

    [DisplayName("Chi tiết")]
    public string Detail { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime? CreatedDate { get; set; }

    [DisplayName("NV tạo")]
    public int? CreatedBy { get; set; }

    [DisplayName("Ngày sửa")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("NV sửa")]
    public int? ModifiedBy { get; set; }

    [DisplayName("Mã CN")]
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
