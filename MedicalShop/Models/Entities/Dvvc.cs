using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MedicalShop.Models.Entities
{
  public partial class Dvvc
  {
    public Dvvc()
    {
      TrangThai = new HashSet<TrangThai>();
    }

    public int Id { get; set; }

    [DisplayName("Mã DVVC")]
    public string MaDvvc { get; set; }

    [DisplayName("Tên DVVC")]
    public string TenDvvc { get; set; }

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

    public virtual ICollection<TrangThai> TrangThai { get; set; }
  }
}
